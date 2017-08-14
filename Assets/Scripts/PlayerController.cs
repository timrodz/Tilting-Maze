using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RoomController room;

    public LayerMask barrierMask;

    // Particles
    public ParticleSystem movementParticles;
    public ParticleSystem collisionParticles;

    // Character Controller
    private CharacterController controller;
    [HideInInspector] public bool isMoving = true;
    private Vector3 moveDirection = Vector3.zero;
    public float gravityMultiplier = 20.0f;

    // Collision registry
    public bool canCheckForCollisions = false;
    private Vector3 lastPosition = Vector3.zero;
    private GameObject collidedObject = null;
    private bool hasCollided = false;
    private int angleXPointsTowards = 0;
    private Vector3 downwardsDirection = Vector3.zero;

    // Animations
    private float airTime = 0;
    private float stretchMultiplier = 0.35f;

    // -------------------------------------------------------------------------------------------

    void Start()
    {

        controller = GetComponent<CharacterController>();

        room = FindObjectOfType<RoomController>();

        CalculateMovementDirection();

        canCheckForCollisions = false;

    }

    void Update()
    {

        if (GameManager.Instance == null)
        {
            return;
        }

        // Only process the player's movement if the game state is on playing
        if (GameManager.Instance.currentState != GameState.Play)
        {
            return;
        }

        Debug.DrawRay(transform.position, downwardsDirection, Color.magenta);

        // Apply movement when either the camera isn't rotating or the game's not paused
        if (!(!room.canRotate || GameManager.Instance.currentState == GameState.Paused))
        {

            // Check for collisions when the map can't be moved
            if (canCheckForCollisions)
            {
                CheckForCollisions();
            }

            // move the body whenever it's grounded
            if (controller.isGrounded)
            {

                if (movementParticles.isPlaying)
                {

                    movementParticles.Stop();

                }

                // Reset the moveDirection vector everytime the player is grounded
                // Otherwise the gravity will accumulate too much force
                moveDirection = Vector3.zero;
                moveDirection = transform.TransformDirection(moveDirection);

            }
            // Otherwise animate its fall
            else
            {

                isMoving = (int) controller.velocity.magnitude > 0;

                if (isMoving)
                {

                    airTime += Time.deltaTime;

                    if (!movementParticles.isPlaying)
                    {
                        movementParticles.transform.DOScale(1, 0);
                        movementParticles.Play();
                    }

                    AnimateDrop();

                }

            }

            // Apply gravity to the body
            moveDirection.y -= gravityMultiplier * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);

            // Round the position vector's positions to 1 decimal
            // Aims to reduce many wall-sticking glitches
            transform.position = new Vector3(
                (float) System.Math.Round(transform.position.x, 1),
                (float) System.Math.Round(transform.position.y, 1),
                (float) System.Math.Round(transform.position.z, 1)
            );

        }
        else
        {

            movementParticles.transform.DOScale(0, 1);
            movementParticles.Stop();

        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (GameManager.Instance.currentState != GameState.Play)
        {
            return;
        }

        // if (!room.canRotateCamera) {
        //     return;
        // }

        // Has reached the goal of the level
        if ((other.CompareTag("Finish")))
        {

            movementParticles.Stop();
            GameManager.Instance.CompleteLevel();

        }
        else if (other.CompareTag("Trigger"))
        {

            movementParticles.Stop();
            canCheckForCollisions = true;
            // Debug.Log("Entering trigger ID: " + other.name);

        }

    }

    public bool CheckForCollisions()
    {

        // Since the player is at an offset Z-axis position
        // store its position and set that Z back to 0
        Vector3 position = transform.position;
        position.z = 0;

        RaycastHit hit;

        if (Physics.Raycast(position, downwardsDirection, out hit, 1, barrierMask))
        {

            // Debug.Log("Raycast enter with " + hit.collider.name);

            canCheckForCollisions = false;

            if ((isMoving) && (!hasCollided) && (collidedObject != hit.collider.gameObject))
            {

                // Debug.Log("Current position: " + position + " last position: " + lastPosition);

                if (position != lastPosition)
                {

                    // canCheckForCollisions = false;

                    lastPosition = position;

                    collidedObject = hit.collider.gameObject;

                    StopAllCoroutines();

                    StartCoroutine(AnimateCollision());

                    Debug.Log("Collision with " + collidedObject.name + ": PASS");

                    return true;

                }
                else
                {

                    Debug.LogError("Collision check: FAIL - Positions didn't match");

                    return false;

                }

            }
            else
            {

                Debug.Log("Collision check: PASS - Has not collided or moved");

                return false;

            }

        }

        return false;

    }

    private IEnumerator AnimateCollision()
    {

        // Debug.Log("Air time: " + airTime);

        hasCollided = true;

        AudioManager.Instance.Play("Collision");

        var cpm = collisionParticles.main;
        cpm.startSpeed = 3 * airTime;
        collisionParticles.Play();

        CameraController.Instance.Shake();

        float length = 0.3f;
        float randomX = Random.Range(0.2f, 0.3f);
        float randomY = Random.Range(0.6f, 0.7f);

        transform.DOScaleX(randomX, length);
        transform.DOScaleY(randomY, length);

        yield return new WaitForSeconds(length);

        transform.DOScaleX(1, 0.2f);
        transform.DOScaleY(1, 0.2f);

        yield return new WaitForSeconds(0.2f);

        hasCollided = false;

        airTime = 0;

    }

    private void AnimateDrop()
    {

        switch (angleXPointsTowards)
        {
            // x axis down, y axis right
            case 270:
                transform.DOScaleY(1 - (airTime * stretchMultiplier), 0).SetEase(Ease.OutCubic);
                break;
            case 0:
            case 360:
                // x axis right, y axis up
                transform.DOScaleX(1 - (airTime * stretchMultiplier), 0).SetEase(Ease.OutCubic);
                break;
            case 90:
            case -180:
                // x axis up, y axis left
                transform.DOScaleY(1 - (airTime * stretchMultiplier), 0).SetEase(Ease.OutCubic);
                break;
            case 180:
                // x axis left, y axis down
                transform.DOScaleX(1 - (airTime * stretchMultiplier), 0).SetEase(Ease.OutCubic);
                break;
        }

    }

    public void CalculateMovementDirection()
    {

        canCheckForCollisions = true;

        angleXPointsTowards = (int) transform.eulerAngles.z;

        // We get a vector from the angles given by direction
        // It's negated because the X behaves like a Y-axis in a normal cartesian plane
        // downwardsDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angleXPointsTowards), Mathf.Sin(Mathf.Deg2Rad * -angleXPointsTowards), 0);

        downwardsDirection = transform.InverseTransformDirection(transform.up) * -1;

        // Debug.Log("Movement direction: " + angleXPointsTowards + " angles");

    }

}