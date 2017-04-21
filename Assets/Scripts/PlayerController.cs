using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Script References
    private GameManager gameManager;

    // Particles
    public ParticleSystem movementParticles;
    public ParticleSystem collisionParticles;

    // Character Controller
    private CharacterController controller;
    [HideInInspector] public bool isMoving = true;
    private Vector3 moveDirection = Vector3.zero;
    public float gravityMultiplier = 20.0f;
    public float speed = 6.0f;

    private bool hasHit;

    // -------------------------------------------------------------------------------------------

    private void Awake() {

        gameManager = FindObjectOfType<GameManager> ();
        controller = GetComponent<CharacterController> ();

    }

    private void FixedUpdate() {

        // Only process the player's movement if the goal hasn't been reached
        if (!gameManager.isLevelComplete) {

            isMoving = (int) controller.velocity.magnitude > 0;

            // Apply movement when either the camera isn't rotating or the game's not paused
            if (!(!RoomController.canRotateCamera || gameManager.currentState == GameState.Paused)) {

                if (isMoving) {
                    AnimateParticles();
                } else {
                    StopAnimatingParticles();
                }

                // move the body whenever it's grounded
                if (controller.isGrounded) {

                    // Reset the moveDirection vector everytime the player is grounded
                    // Otherwise the gravity will accumulate too much force
                    moveDirection = Vector3.zero;
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= speed;

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

            } else {

                StopAnimatingParticles();

            }

        }

    }

    private void OnTriggerEnter(Collider other) {

        // Has reached the goal of the level
        if ((other.CompareTag("Finish")) && (!gameManager.isLevelComplete)) {

            gameManager.CompleteLevel();

        } else if (other.CompareTag("Trigger")) {

            StopAnimatingParticles();

        }

    }

    /// <summary>
    /// OnControllerColliderHit is called when the controller hits a
    /// collider while performing a Move.
    /// </summary>
    /// <param name="hit">The ControllerColliderHit data associated with this collision.</param>
    private void OnControllerColliderHit(ControllerColliderHit hit) {

        if (isMoving && !hasHit) {

            StartCoroutine(ResetHasHit());

            gameManager.soundManager.Play(Clip.hit);

            collisionParticles.Play();

            gameManager.cameraController.Shake();

        }

    }

    public void AnimateParticles() {

        if (!movementParticles.isPlaying) {
            movementParticles.transform.DOScale(1, 0);
            movementParticles.Play();
        }

    }

    public void StopAnimatingParticles() {

        if (movementParticles.isPlaying) {
            movementParticles.Stop();
        }

    }

    private IEnumerator ResetHasHit() {

        hasHit = true;
        yield return new WaitForSeconds(0.1f);
        hasHit = false;

    }

}