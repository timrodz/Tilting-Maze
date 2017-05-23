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

    // Collision registry
    private Vector3 lastPosition = Vector3.zero;
    private GameObject collidedObject = null;
    [HideInInspector] public bool hasCollided;

    // Animations
    private float airTime = 0;
    float stretchMultiplier = 0.35f;

    // -------------------------------------------------------------------------------------------

    void Start () {

        gameManager = FindObjectOfType<GameManager> ();
        controller = GetComponent<CharacterController> ();

    }

    void FixedUpdate () {

        // Only process the player's movement if the goal hasn't been reached
        if (gameManager.currentState != GameState.Playing) {
            return;
        }

        isMoving = (int)controller.velocity.magnitude > 0;

        // Apply movement when either the camera isn't rotating or the game's not paused
        if (!(!RoomController.canRotateCamera || gameManager.currentState == GameState.Paused)) {

            if (isMoving) {

                airTime += Time.deltaTime;
                AnimateDrop ();
                AnimateParticles ();

            }
            else {

                StopAnimatingParticles ();

            }

            // move the body whenever it's grounded
            if (controller.isGrounded) {

                // Reset the moveDirection vector everytime the player is grounded
                // Otherwise the gravity will accumulate too much force
                moveDirection = Vector3.zero;
                moveDirection = transform.TransformDirection (moveDirection);
                moveDirection *= speed;

            }

            // Apply gravity to the body
            moveDirection.y -= gravityMultiplier * Time.deltaTime;
            controller.Move (moveDirection * Time.deltaTime);

            // Round the position vector's positions to 1 decimal
            // Aims to reduce many wall-sticking glitches
            transform.position = new Vector3 (
                (float)System.Math.Round (transform.position.x, 1),
                (float)System.Math.Round (transform.position.y, 1),
                (float)System.Math.Round (transform.position.z, 1)
            );

        }
        else {

            StopAnimatingParticles ();

        }

    }

    private void OnTriggerEnter (Collider other) {

        if (gameManager.currentState != GameState.Playing) {
            return;
        }

        // Has reached the goal of the level
        if ((other.CompareTag ("Finish"))) {

            gameManager.CompleteLevel ();

        }
        else if (other.CompareTag ("Trigger")) {

            StopAnimatingParticles ();

        }

    }

    /// <summary>
    /// OnControllerColliderHit is called when the controller hits a
    /// collider while performing a Move.
    /// </summary>
    /// <param name="hit">The ControllerColliderHit data associated with this collision.</param>
    private void OnControllerColliderHit (ControllerColliderHit hit) {

        if (gameManager.currentState != GameState.Playing) {
            return;
        }

        if ((isMoving) && (!hasCollided) && (collidedObject != hit.gameObject) && (transform.position != lastPosition)) {

            lastPosition = transform.position;

            collidedObject = hit.gameObject;

            StopAllCoroutines ();

            StartCoroutine (AnimateCollision ());

        }

    }

    public void AnimateParticles () {

        if (!movementParticles.isPlaying) {
            movementParticles.transform.DOScale (1, 0);
            movementParticles.Play ();
        }

    }

    public void StopAnimatingParticles () {

        if (movementParticles.isPlaying) {
            movementParticles.Stop ();
        }

    }

    private IEnumerator ResetHasHit () {

        hasCollided = true;
        yield return new WaitForSeconds (0.1f);
        hasCollided = false;

    }

    private IEnumerator AnimateCollision () {

        hasCollided = true;

        gameManager.soundManager.Play (Clip.hit);

        collisionParticles.Play ();

        gameManager.cameraController.Shake ();

        float length = 0.3f;
        float randomX = Random.Range (0.2f, 0.3f);
        float randomY = Random.Range (0.6f, 0.7f);

        transform.DOScaleX (randomX, length);
        transform.DOScaleY (randomY, length);

        yield return new WaitForSeconds (length);

        transform.DOScaleX (1, 0.2f);
        transform.DOScaleY (1, 0.2f);

        yield return new WaitForSeconds (0.2f);

        hasCollided = false;
        airTime = 0;

    }

    private void AnimateDrop () {

        int rotationZ = (int)transform.eulerAngles.z;

        switch (rotationZ) {
            case 270:
                // x axis down, y axis right
                transform.DOScaleY (1 - (airTime * stretchMultiplier), 0).SetEase (Ease.OutCubic);
                break;
            case 0:
            case 360:
                // x axis right, y axis up
                transform.DOScaleX (1 - (airTime * stretchMultiplier), 0).SetEase (Ease.OutCubic);
                break;
            case 90:
            case -180:
                // x axis up, y axis left
                transform.DOScaleY (1 - (airTime * stretchMultiplier), 0).SetEase (Ease.OutCubic);
                break;
            case 180:
                // x axis left, y axis down
                transform.DOScaleX (1 - (airTime * stretchMultiplier), 0).SetEase (Ease.OutCubic);
                break;
        }

    }

}