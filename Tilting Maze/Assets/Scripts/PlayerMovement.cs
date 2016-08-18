using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private float pullDuration = 3.0f;

	private Rigidbody body;

	bool HasFinishedLevel = false;

	// Use this for initialization
	void Awake() {

		body = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update() {

		if (!HasFinishedLevel) {

			// Freeze the object's rotation and position whenever the camera is moving
			if (!MapController.canRotateCamera) {
				
				body.isKinematic = true;

			}
			// Remove every constrain except for the X position and rotation
			else {

				body.isKinematic = false;
				// Round the position vector
				transform.position = new Vector3(
					(float)System.Math.Round(transform.position.x, 1), 
					(float)System.Math.Round(transform.position.y, 1), 
					(float)System.Math.Round(transform.position.z, 1)
				);

			}

			// TODO: add a constrain that allows the camera to rotate ONLY if the player isn't moving

		}
		else {

			body.isKinematic = true;

		}
	
	}

	void OnTriggerEnter(Collider other) {

		// Has reached the goal of the level
		if (other.tag == "Finish") {

			HasFinishedLevel = true;
			StartCoroutine(AnimationWin());

		}

	}

	/// Function AnimationWin
	// Center the camera on the player
	// Translate the player's x-axis positively
	// Rotate around itself
	// Scale it in the y and z planes

	IEnumerator AnimationWin() {

		Transform camera = GameObject.Find("Main Camera").GetComponent<Transform>();

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / pullDuration) {

			float scaleFactor = 0.0225f;

			// Make the camera fade out and move the player toward's the center
			camera.Translate(new Vector3(0, 0, -scaleFactor * 4));
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(0.5f, 0, 0), t / 1.5f);

			// Rotate around itself
			transform.Rotate(new Vector3(3.25f + t, 0.0f, 0.0f));

			// Scale it in the y and z planes
			scaleFactor *= (16 + t);
			transform.localScale += new Vector3(0.1f, scaleFactor, scaleFactor);

			yield return null;

		}
		
		// Change the background's color to the player's material color
		GameObject.Find("Background").GetComponent<MeshRenderer>().material.color = transform.GetComponent<MeshRenderer>().material.color;

		yield return new WaitForSeconds(0.0f);
		LevelLoader ll = GameObject.Find("Game Manager").GetComponent<LevelLoader>();
		ll.CompleteLevel(transform);

	}

}