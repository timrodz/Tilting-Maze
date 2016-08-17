using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private float pullDuration = 3.0f;

	private Rigidbody body;

	bool bHasFinishedLevel = false;

	// Use this for initialization
	void Awake() {

		body = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update() {

		if (!bHasFinishedLevel) {

			// Freeze the object's rotation and position whenever the camera is moving
			if (!MapController.canRotateCamera) {
				body.isKinematic = true;

			}
			// Remove every constrain except for the X position and rotation
			else {

				body.isKinematic = false;
				// Round the position vector
				transform.position = new Vector3((float)System.Math.Round(transform.position.x, 1), (float)System.Math.Round(transform.position.y, 1), (float)System.Math.Round(transform.position.z, 1));

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

			bHasFinishedLevel = true;
			StartCoroutine(AnimationWin());

		}

	}

	/// Function AnimationWin
	// Center the camera on the player
	// Translate the player's x-axis positively
	// Rotate 180 around itself

	IEnumerator AnimationWin() {

		Transform camera = GameObject.Find("Main Camera").GetComponent<Transform>();

		for (float t = 0.1f; t < 1.0f; t += Time.deltaTime / pullDuration) {

			float cameraX = transform.position.x - camera.position.x;
			float cameraY = transform.position.y - camera.position.y;

			Vector3 finalCameraPosition = new Vector3(cameraX * Time.deltaTime, cameraY * Time.deltaTime, 0);

			// Center the camera on the player
			camera.transform.Translate(finalCameraPosition);

			// Translate the player's x-axis positively
			transform.Translate(Vector3.right * (t / 50));

			// Rotate 180 around itself
			transform.Rotate(new Vector3(2 + t, 0, 0));
//			transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);

			yield return null;

		}
		
		// Destroy the object 2 seconds after the animation finishes
		yield return new WaitForSeconds(2.0f);
		Destroy(gameObject);

	}

}