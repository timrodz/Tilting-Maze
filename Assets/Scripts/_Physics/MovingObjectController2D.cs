/// <summary>
/// Handles control using the Controller2D class
/// 
/// Author: Juan Rodriguez
/// Date: 10/09/2017
/// </summary>

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class    MovingObjectController2D : Controller2D
{
    [Header ("Movement")]
    [SerializeField] private float m_JumpUnitHeight = 4;
    [SerializeField] private float m_JumpPeakTime = 0.5f;
    [SerializeField] protected bool m_HasTriggeredButton = false;

    [Header ("Gravity and jumping")]
    [HideInInspector][SerializeField] private float m_Gravity;
    [HideInInspector][SerializeField] private float m_AirTime = 0;
    [HideInInspector][SerializeField] protected Vector3 m_Velocity;

    [Header ("Particles")]
    [SerializeField] private bool m_HasPlayedMovementParticles = false;
    [SerializeField] private ParticleSystem m_CollisionParticles;

    [Header ("Collision")]
    [SerializeField] private bool m_IsCollidingBelow;
    [SerializeField] private bool m_IsProcessingCollision;
    [SerializeField] private bool firstCollision = true;
    [SerializeField] protected bool m_CanProcessCollisions = true;

    [Header ("State")]
#pragma warning disable 0414
    [SerializeField] private MovementState m_State = MovementState.Idle;
#pragma warning restore 0414

    // Use this for initialization
    private void Awake ()
    {
        InitializePhysics ();
        m_State = MovementState.Idle;
        CanMove = true;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable ()
    {
        GameEvents.Instance.PlayerTriggerButtonEnter += OnPlayerTriggerButtonEnter;
        GameEvents.Instance.PlayerTriggerButtonExit += OnPlayerTriggerButtonExit;
        GameEvents.Instance.TriggerButtonAnimationFinished += TriggerButtonAnimationFinished;
        GameEvents.Instance.ToggleDragging += ToggleDragging;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable ()
    {
        GameEvents.Instance.PlayerTriggerButtonEnter -= OnPlayerTriggerButtonEnter;
        GameEvents.Instance.PlayerTriggerButtonExit -= OnPlayerTriggerButtonExit;
        GameEvents.Instance.TriggerButtonAnimationFinished -= TriggerButtonAnimationFinished;
        GameEvents.Instance.ToggleDragging -= ToggleDragging;
    }

    private void InitializePhysics ()
    {
        m_Gravity = -((2.0f * m_JumpUnitHeight) / (Mathf.Pow (m_JumpPeakTime, 2.0f)));
    }

    // Update is called once per frame
    private void Update ()
    {
        if (GameManager.Instance.State != GameState.Play)
        {
            return;
        }

        if (!CanMove)
        {
            return;
        }

        ProcessCollisions ();

        // If is colliding with something below or above
        // Vertical velocity will be 0
        if (m_CollisionInfo.above || m_CollisionInfo.below)
        {
            m_Velocity.y = 0;
        }

        if (m_Velocity.y != 0.0f)
        {
            IsMoving = true;
        }

        // Player just started moving
        if (m_Velocity.y < -2.0f && !m_HasPlayedMovementParticles)
        {
            m_State = MovementState.Moving;
            m_CanProcessCollisions = true;
            m_IsCollidingBelow = false;
            m_HasPlayedMovementParticles = true;
            m_AirTime += Time.deltaTime;
        }

        // Always apply gravity
        m_Velocity.y += m_Gravity * Time.deltaTime;

        Move (m_Velocity * Time.deltaTime);
    }

    public void ProcessCollisions ()
    {
        if (!m_CanProcessCollisions)
        {
            return;
        }

        if (m_CollisionInfo.below && !m_IsCollidingBelow)
        {
            m_IsCollidingBelow = true;
            m_IsProcessingCollision = true;
        }

        if (m_IsCollidingBelow && m_IsProcessingCollision)
        {
            if (firstCollision)
            {
                firstCollision = false;
                m_IsProcessingCollision = false;
                return;
            }

            AnimateCollision ();
        }
    }

    protected void AnimateCollision ()
    {
        Print.Log("COL");

        m_State = MovementState.Colliding;

        CanMove = true;

        StopCoroutine ("AnimateDrop");

        AudioManager.PlayEffect (ClipType.Collision);

        m_HasPlayedMovementParticles = false;

        m_CollisionParticles.Play ();

        StartCoroutine ("AnimateDrop");
    }

    private IEnumerator AnimateDrop ()
    {
        CanMove = false;

        m_IsProcessingCollision = false;

        var cpm = m_CollisionParticles.main;
        cpm.startSpeed = 3 * m_AirTime;
        m_CollisionParticles.Play ();

        GameEvents.Instance.Event_PlayerCollision ();

        float length = 0.2f;
        float randomX = Random.Range (0.2f, 0.3f);
        float randomY = Random.Range (0.6f, 0.7f);

        transform.DOScaleX (randomX, length);
        transform.DOScaleY (randomY, length);

        yield return new WaitForSeconds (length);

        transform.DOScaleX (1, 0.2f);
        transform.DOScaleY (1, 0.2f);

        yield return new WaitForSeconds (0.2f);

        if (m_HasTriggeredButton)
        {
            m_HasTriggeredButton = false;
        }
        else
        {
            CanMove = true;
            IsMoving = false;
        }

        m_AirTime = 0;

        m_State = MovementState.Idle;
    }

    protected virtual void OnPlayerTriggerButtonEnter (Vector3 _position) { }

    protected virtual void OnPlayerTriggerButtonExit () { }

    protected virtual void TriggerButtonAnimationFinished ()
    {
        CanMove = true;
    }

    protected virtual void ToggleDragging (bool _state)
    {
        // Dragging
        if (_state == true)
        {
            CanMove = false;
            IsMoving = false;
        }
        // Stopped Dragging
        // The player must wait for the map to even its angles before moving
        // Otherwise this will break the movement
        else
        {
            // CanMove = true;
        }
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Ray r = new Ray (transform.position + Vector3.back, DownDirection + Vector3.back);
        Gizmos.DrawRay (r);
    }

    protected Vector3 DownDirection
    {
        get
        {
            return (-transform.InverseTransformDirection (transform.up));
        }
    }

    private void OnValidate ()
    {
        InitializePhysics ();
    }
}

// -------------------------------------------------------------------------------------------
[System.Serializable]
public enum MovementState
{
    Idle = 0,
    Moving,
    Colliding
}