using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	[HideInInspector]
	public bool bHasFinishedLevel = false;

	private float fWinPullDuration = 2.0f;

	private bool canShowWinScreen = true;

	// For accessing the paused state of the game
	private GameManager gm;

	// The material to access
	private MeshRenderer playerMR, goalMR;

	// CHARACTER CONTROLLER //
	private CharacterController controller;
	private float speed = 6.0f;
	private float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;


	private void Awake() {

		gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
		controller = GetComponent<CharacterController>();

	}

	private void Start() {

		StartCoroutine(UpdateColors(1 / 6f, 1 / 2f));

	}

	private void Update() {

		// Only process the player's movement if the goal hasn't been reached
		if (!bHasFinishedLevel) {
			
			// Apply movement when the camera isn't rotating or the game's not paused
			if (!(!MapController.canRotateCamera || gm.IsPaused())) {

				if (controller.isGrounded) {

					// Reset the moveDirection vector everytime the player is grounded
					// Otherwise the gravity'll add too much force
					moveDirection = new Vector3(0, 0, 0);
					moveDirection = transform.TransformDirection(moveDirection);
					moveDirection *= speed;

				}

				moveDirection.y -= gravity * Time.deltaTime;
				controller.Move(moveDirection * Time.deltaTime);

				// Round the position vector's positions to 1 decimal
				// Aims to reduce many wall-sticking glitches
				transform.position = new Vector3(
					(float)System.Math.Round(transform.position.x, 1),
					(float)System.Math.Round(transform.position.y, 1),
					(float)System.Math.Round(transform.position.z, 1)
				);

			}

		}

	}

	private void OnTriggerEnter(Collider other) {

		// Has reached the goal of the level
		if ((other.tag == "Finish") && (!bHasFinishedLevel)) {

			print(">>>> Reached GOAL");
			bHasFinishedLevel = true;
			gm.bCanPause = false;
			StartCoroutine(AnimationWin());

		}

	}

	/// <summary>
	/// Executes the winning animation whenever the player reaches the goal.
	/// </summary>
	/// <returns>nil.</returns>
	private IEnumerator AnimationWin() {

		GameManager gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
		Transform camera = GameObject.Find("Main Camera").GetComponent<Transform>();

		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / fWinPullDuration)) {

			float scaleFactor = 0.0225f;

			// Make the camera fade out and move the player toward's the center
			camera.Translate(new Vector3(0.0f, 0.0f, -scaleFactor * 4.0f));
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(0.5f, 0.0f, 0.0f), t / 1.5f);

			// Rotate around itself
			transform.Rotate(new Vector3(3.25f + t, 0.0f, 0.0f));

			// Scale it in the y and z planes
			scaleFactor *= (22.0f + t);
			transform.localScale += new Vector3(0.01f, scaleFactor, scaleFactor);

			// Show the winning screen before the rotation finishes
			// Reason: it makes the UX feel smoother
			if ((t > 0.6f) && (canShowWinScreen)) {

				canShowWinScreen = false;
				gm.CompleteLevel();

			}

			yield return null;

		}

		// Change the color of the background to the player's material color
		GameObject.Find("Background").GetComponent<MeshRenderer>().material.color = transform.GetComponent<MeshRenderer>().material.color;

	}

	/// Change the colors of the player and the goal
	/// param _lOffset : The minimum value for the hue
	/// param _rOffset : The maximum value for the hue
	//

	/// <summary>
	/// Updates the colors.
	/// </summary>
	/// <returns>The colors.</returns>
	/// <param name="_lOffset">L offset.</param>
	/// <param name="_rOffset">R offset.</param>
	private IEnumerator UpdateColors(float _lOffset, float _rOffset) {

		yield return new WaitForSeconds(0.0f);

		Color color = RandomColor(_lOffset, _rOffset);

		playerMR = GetComponent<MeshRenderer>();

		goalMR = GameObject.Find("GOAL").GetComponent<MeshRenderer>();

		playerMR.material.color = color;
		goalMR.material.color = color;

	}

	/// Return a random bright color between two hue values
	/// param _lOffset : The minimum value for the hue
	/// param _rOffset : The maximum value for the hue
	/// 
	//

	/// <summary>
	/// Randoms the color.
	/// </summary>
	private Color RandomColor(float _lOffset, float _rOffset) {

		return Random.ColorHSV(_lOffset, _rOffset, 1f, 1f, 1f, 1f);

	}

}