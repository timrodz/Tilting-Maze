using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {

	[HideInInspector]
	public static bool canRotateCamera = true;

	[SerializeField]
	private float rotationDuration = 0.05f;

	// For referencing the player's current speed and finished level state
	private GameObject player;

	// For accessing the paused state of the game
	GameManager gm;

	void Awake() {

		gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
		player = GameObject.Find("PLAYER");

	}


	void Update() {

		// Don't do anything if the game's curently paused
		if (gm.IsPaused() || player == null) {
			player = GameObject.Find("PLAYER");
			return;
		}

		// Check whether or not the player is moving by tracking its magnitude velocity vector
		bool bIsPlayerMoving = (int)Mathf.Abs(player.GetComponent<CharacterController>().velocity.magnitude) > 0;

		// Check whether or not the player has reached the goal
		bool bHasPlayerFinishedTheLevel = player.GetComponent<PlayerMovement>().bHasFinishedLevel;

		// Allow for camera rotation ONLY if the player meets the following criteria
		if ((canRotateCamera) && (!bIsPlayerMoving) && (!bHasPlayerFinishedTheLevel)) { 

			if (Input.GetAxisRaw("Horizontal") < 0) {
				
				var valToAdd = 90.0f;
				if (transform.rotation.x > 0.7f)
					valToAdd *= -1;
				StartCoroutine(RotateCamera(Vector3.left * valToAdd));

			}
			else if (Input.GetAxisRaw("Horizontal") > 0) {
				
				var valToAdd = 90.0f;
				if (transform.rotation.x > 0.7f)
					valToAdd *= -1;
				StartCoroutine(RotateCamera(Vector3.right * valToAdd));

			}

		}

	}

	/// <summary>
	/// Rotates the camera.
	/// </summary>
	public IEnumerator RotateCamera(Vector3 anglesInDegrees) {

		canRotateCamera = false;

		Quaternion fromAngle = transform.rotation; // Get the transform's current rotation coordinates
		Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + anglesInDegrees); // Convert byAngles to radians

		// Process a loop that lasts for the prompted time
		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / rotationDuration)) {

			// Make a slerp from the current rotation's coordinates to the desired rotation
			transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
			yield return null;

		}

		// Round the rotation at the end
		transform.rotation = toAngle; 
		canRotateCamera = true;

		// Update the current move count
		GameManager.moveCount++;

	}

}