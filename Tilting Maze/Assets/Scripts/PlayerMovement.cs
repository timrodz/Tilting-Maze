using UnityEngine;
using System.Collections;

// TODO: Use CharacterController for movement instead of direct RigidBody gravity

public class PlayerMovement : MonoBehaviour {

	private float fWinPullDuration = 2.0f;

	private Rigidbody body;

	[HideInInspector]
	public bool bHasFinishedLevel = false;

	private bool canShowWinScreen = true;

	// For accessing the paused state of the game
	GameManager gm;

	// The material to access
	MeshRenderer thisMeshRenderer;
	MeshRenderer goalMeshRenderer;

	// Use this for initialization
	void Awake() {

		gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
		body = GetComponent<Rigidbody>();
		thisMeshRenderer = GetComponent<MeshRenderer>();
		goalMeshRenderer = GameObject.Find("GOAL").GetComponent<MeshRenderer>();

	}

	// Change the color of the material

	void Start() {

		Color color = RandomColor();
		thisMeshRenderer.material.color = color;
		goalMeshRenderer.material.color = color;

	}

	// Update is called once per frame
	void Update() {

		if (!bHasFinishedLevel) {

			// Freeze the object's rotation and position whenever the camera is moving
			if (!MapController.canRotateCamera || gm.IsPaused()) {

				body.isKinematic = true;

			}
			// Remove every constrain except for the X position and rotation
			else {

				body.isKinematic = false;

				// Round the position vector for more accurate physics
				transform.position = new Vector3(
					(float) System.Math.Round(transform.position.x, 1),
					(float) System.Math.Round(transform.position.y, 1),
					(float) System.Math.Round(transform.position.z, 1)
				);

			}

		}
		else {

			body.isKinematic = true;

		}

	}

	/// Function OnTriggerEnter
	// Check if the player has found the finish

	void OnTriggerEnter(Collider other) {

		// Has reached the goal of the level
		if (other.tag == "Finish" && !bHasFinishedLevel) {

			bHasFinishedLevel = true;
			gm.bCanPause = false;
			StartCoroutine(AnimationWin());

		}

	}

	/// Function AnimationWin
	// Center the camera on the player
	// Translate the player's x-axis positively
	// Rotate around itself
	// Scale it in the y and z planes

	IEnumerator AnimationWin() {

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

		GameObject.Find("Background").GetComponent<MeshRenderer>().material.color = transform.GetComponent<MeshRenderer>().material.color;

	}

	Color RandomColor() {

		// Start with a basic random color

		float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
		float randomValue = Random.Range(0, 1);
		randomValue += goldenRatio;
		randomValue %= 1;

		return Random.ColorHSV(1/6f, 5/6f, 1f, 1f, 1f, 1f);

	}

}