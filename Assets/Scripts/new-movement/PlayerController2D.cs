using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Handles control using the Controller2D class
/// 
/// Author: Juan Rodriguez
/// Date: 10/09/2017
/// </summary>
public class PlayerController2D : Controller2D
{
    [Header("Movement")]
    [SerializeField] private bool ALLOW_INPUT = true;
    [SerializeField] private float m_MovementSpeed = 6;
    [SerializeField] private float m_JumpUnitHeight = 4;
    [SerializeField] private float m_JumpPeakTime = 0.5f;
    [SerializeField] private float m_JumpVelocityFromWall;
    [Range(0, 1.0f)]
    [SerializeField] private float m_AccelerationTimeAirborne = 0.2f;
    [Range(0, 1.0f)]
    [SerializeField] private float m_AccelerationTimeGrounded = 0.1f;

    [Header("Gravity and jumping")]
    [HideInInspector][SerializeField] private float m_Gravity;
    [HideInInspector][SerializeField] private float m_JumpVelocity;
    [SerializeField] private bool m_WillPerformFirstJump = true;
    [SerializeField] private int m_JumpCount = 0;
    [SerializeField] private float m_AirTime = 0;

    [SerializeField] private Vector3 m_Velocity;
    [SerializeField] private float m_VelocityXSmoothing;

    [SerializeField] private bool m_IsFacingRight = true;

    [Header("Particles")]
    [SerializeField] private bool m_HasPlayedMovementParticles = false;
    [SerializeField] ParticleSystem m_MovementParticles;
    [SerializeField] TrailRenderer m_MovementTrail;
    [SerializeField] ParticleSystem m_CollisionParticles;

    private Vector2 direction;
    private bool m_HasBegunCollisionBelow;
    bool m_HasProcessedCollisionBelow;
    bool firstCollision = true;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        InitializePhysics();
    }

    private void InitializePhysics()
    {
        m_Gravity = -((2.0f * m_JumpUnitHeight) / (Mathf.Pow(m_JumpPeakTime, 2.0f)));
        m_JumpVelocity = Mathf.Abs(m_Gravity) * m_JumpPeakTime;
    }

    private void Jump()
    {
        m_CollisionParticles.Stop();
        m_CollisionParticles.Play();
        m_Velocity.y = m_JumpVelocity;
        m_JumpCount++;
    }

    /// <summary>
    /// Jumps from a wall
    /// </summary>
    /// <param name="_isJumpingFromRight"></param>
    private void JumpFromWall(bool _isJumpingFromRight)
    {
        Jump();

        if (_isJumpingFromRight)
        {
            m_Velocity.x -= m_JumpVelocityFromWall;
        }
        else
        {
            m_Velocity.x += m_JumpVelocityFromWall;
        }
    }

    private void ToggleFacingRight()
    {
        m_IsFacingRight = !m_IsFacingRight;
    }

    public void HandleBelowCollision()
    {
        if (m_CollisionInfo.below && !m_HasBegunCollisionBelow)
        {
            m_HasBegunCollisionBelow = true;
            m_HasProcessedCollisionBelow = true;
        }

        if (m_HasBegunCollisionBelow && m_HasProcessedCollisionBelow)
        {
            if (firstCollision)
            {
                firstCollision = false;
                m_HasProcessedCollisionBelow = false;
                return;
            }
            
            AudioManager.Play("Collision");

            m_HasPlayedMovementParticles = false;

            m_CollisionParticles.Play();

            // m_MovementParticles.Stop();

            StartCoroutine(AnimateDrop());
        }
    }

    private IEnumerator AnimateDrop()
    {
        m_CanMove = false;

        m_HasProcessedCollisionBelow = false;

        // AudioManager.Instance.Play("Collision");

        var cpm = m_CollisionParticles.main;
        cpm.startSpeed = 3 * m_AirTime;
        m_CollisionParticles.Play();

        CameraController.Instance.Shake();

        float length = 0.2f;
        float randomX = Random.Range(0.2f, 0.3f);
        float randomY = Random.Range(0.6f, 0.7f);

        transform.DOScaleX(randomX, length);
        transform.DOScaleY(randomY, length);

        yield return new WaitForSeconds(length);

        transform.DOScaleX(1, 0.2f);
        transform.DOScaleY(1, 0.2f);

        yield return new WaitForSeconds(0.2f);

        m_AirTime = 0;

        m_CanMove = true;
    }

    void Update()
    {
        if (!m_CanMove) { return; }

        HandleBelowCollision();
        
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        // If is colliding with something below or above
        // Vertical velocity will be 0
        if (m_CollisionInfo.above || m_CollisionInfo.below)
        {
            m_Velocity.y = 0;
        }

        if (m_Velocity.y < -2.0f && !m_HasPlayedMovementParticles)
        {
            m_HasBegunCollisionBelow = false;
            // m_MovementParticles.Play();
            m_HasPlayedMovementParticles = true;
        }

        // Perform jumps
        if (ALLOW_INPUT)
        {
            if (Input.GetButtonDown("Jump") || MobileInputController.Instance.SwipeUp)
            {
                // First jump
                if ((m_CollisionInfo.below && m_JumpCount == 0))
                {
                    Jump();
                    m_WillPerformFirstJump = false;
                }
                // Consecutive jumps
                if (m_AirTime > 0.05f && m_JumpCount >= 1)
                {
                    if (m_JumpCount == 1)
                    {
                        if (m_CollisionInfo.right)
                        {
                            JumpFromWall(true);
                            return;
                        }
                        else if (m_CollisionInfo.left)
                        {
                            JumpFromWall(false);
                            return;
                        }
                        else
                        {
                            Jump();
                        }
                    }
                    else if (m_JumpCount <= 2)
                    {
                        if (m_CollisionInfo.right)
                        {
                            JumpFromWall(true);
                            return;
                        }
                        else if (m_CollisionInfo.left)
                        {
                            JumpFromWall(false);
                            return;
                        }
                    }
                }
            }

            // Determine the time elapsed after the first jump
            if (!m_WillPerformFirstJump)
            {
                m_AirTime += Time.deltaTime;
            }

            // Once the player hits the ground, reset the jump variables
            if ((m_CollisionInfo.below) && (m_AirTime > 0.05f))
            {
                m_WillPerformFirstJump = true;
                m_AirTime = 0;
                m_JumpCount = 0;
            }

            if (input.x > 0)
            {
                if (!m_IsFacingRight) { ToggleFacingRight(); }
            }
            else if (input.x < 0)
            {
                if (m_IsFacingRight) { ToggleFacingRight(); }
            }

            m_Velocity.x = Mathf.SmoothDamp(m_Velocity.x, input.x * m_MovementSpeed, ref m_VelocityXSmoothing, (m_CollisionInfo.below) ? m_AccelerationTimeGrounded : m_AccelerationTimeAirborne);
        }
        // Movement NOT allowed
        else
        {
            input = Vector2.zero;
        }

        // Always apply gravity
        m_Velocity.y += m_Gravity * Time.deltaTime;

        Move(m_Velocity * Time.deltaTime);
    }

    void OnValidate()
    {
        if (ALLOW_INPUT)
        {
            InitializePhysics();

            if (m_JumpPeakTime < 0.1f)
            {
                m_JumpPeakTime = 0.1f;
            }

            if (m_JumpUnitHeight < 1)
            {
                m_JumpUnitHeight = 1;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position - (transform.InverseTransformDirection(transform.up)));
    }

}