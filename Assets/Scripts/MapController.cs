using System.Collections;
using DG.Tweening;
using UnityEngine;
using XboxCtrlrInput;

public class MapController : MonoBehaviour {

    [HideInInspector] public static bool canRotateCamera = true;

    [HeaderAttribute("Rotation")]
    public Ease rotationEaseType = Ease.OutQuad;
    [RangeAttribute(0.3f, 1.0f)]
    public float rotationLength = 0.5f;

    // For referencing the player's current speed and finished level state
    public Transform player;

    // For accessing the paused state of the game
    GameManager gm;

    void Awake() {

        gm = FindObjectOfType<GameManager> ();

    }

    void Update() {

        // Don't do anything if the game's curently paused
        if (gm.isPaused || !player) {
            return;
        }

        // Check whether or not the player is moving by tracking its magnitude velocity vector
        bool bIsPlayerMoving = (int) Mathf.Abs(player.GetComponent<CharacterController> ().velocity.magnitude) > 0;

        // Check whether or not the player has reached the goal
        bool bHasPlayerFinishedTheLevel = player.GetComponent<PlayerMovement> ().hasFinishedLevel;

        // Allow for camera rotation ONLY if the player meets the following criteria
        if ((canRotateCamera) && (!bIsPlayerMoving) && (!bHasPlayerFinishedTheLevel)) {

			if (XCI.GetAxisRaw(XboxAxis.LeftStickX) > 0 || Input.GetKey(KeyCode.D)) {
                
                StartCoroutine(RotateCamera(true));

			} else if (XCI.GetAxisRaw(XboxAxis.LeftStickX) < 0 || Input.GetKey(KeyCode.A)) {
				
                StartCoroutine(RotateCamera(false));

            }

        }

    }

    /// <summary>
    /// Rotates the camera.
    /// </summary>
    public IEnumerator RotateCamera(bool shouldRotateRight) {

        
        canRotateCamera = false;

        // Quaternion fromAngle = transform.rotation; // Get the transform's current rotation coordinates
        // Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + anglesInDegrees); // Convert byAngles to radians

        // // Process a loop that lasts for the prompted time
        // for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / rotationDuration)) {

        //     // Make a slerp from the current rotation's coordinates to the desired rotation
        //     transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
        //     yield return null;

        // }

        // // Round the rotation at the end
        // transform.rotation = toAngle;

        Vector3 eulerRotation = transform.eulerAngles;

        if (shouldRotateRight) {
            eulerRotation.z -= 90;
        } else {
            eulerRotation.z += 90;
        }

        transform.DORotate(eulerRotation, rotationLength);

        yield return new WaitForSeconds(rotationLength);
        
        canRotateCamera = true;

        // Update the current move count
        GameManager.moveCount++;

    }

}