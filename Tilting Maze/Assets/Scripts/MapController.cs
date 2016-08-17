using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {

	public static bool canRotateCamera = true;

	[SerializeField]
	private float RotationTime = 0.05f;

	// Update is called once per frame
	void Update () {

		if (canRotateCamera) {

			if (Input.GetAxisRaw("Horizontal") < 0) {

				// canRotateCamera = false;
				var valToAdd = 90.0f;
				if (transform.rotation.x > 0.7f)
					valToAdd *= -1;
				StartCoroutine(RotateCamera(Vector3.left * valToAdd, RotationTime));


			}
			else if (Input.GetAxisRaw("Horizontal") > 0) {

				// canRotateCamera = false;
				var valToAdd = 90.0f;
				if (transform.rotation.x > 0.7f)
					valToAdd *= -1;
				StartCoroutine(RotateCamera(Vector3.right * valToAdd, RotationTime));

			}

		}

	}

	// Function RotateCamera
	// @param byAngles : the amount of angles to rotate (will be converted to radians)
	// @param inTime : the length of the rotation
	
	IEnumerator RotateCamera (Vector3 byAngles, float inTime) {

		canRotateCamera = false;

		var fromAngle = transform.rotation; // Get the transform's current rotation coordinates
		var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles); // Convert byAngles to radians

		// Process a loop that lasts for the prompted time
		for (float t = 0f; t < 1; t += Time.deltaTime / inTime) {

			// Make a slerp from the current rotation's coordinates to the desired rotation
			transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
			yield return null;

		}

		// Round the rotation at the end
		transform.rotation = toAngle; 
		canRotateCamera = true;

	}

}