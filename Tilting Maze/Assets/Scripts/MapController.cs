using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {

	[SerializeField]
	private Transform T;

	public static bool bCanRotateCamera = true;

	[SerializeField]
	private float RotationTime = 0.05f;

	// Update is called once per frame
	void Update() {

		if (bCanRotateCamera) {

			if (Input.GetAxisRaw("Horizontal") < 0) {

				bCanRotateCamera = false;
				int valToAdd = 90;
				if ((int)transform.rotation.x == 1)
					valToAdd *= -1;
				StartCoroutine(RotateCamera(Vector3.left * valToAdd, RotationTime));


			}
			else if (Input.GetAxisRaw("Horizontal") > 0) {

				bCanRotateCamera = false;
				int valToAdd = 90;
				if ((int)transform.rotation.x == 1)
					valToAdd *= -1;
				StartCoroutine(RotateCamera(Vector3.right * valToAdd, RotationTime));

			}

		}

	}

	IEnumerator RotateCamera(Vector3 byAngles, float inTime) {

		var fromAngle = transform.rotation;
		var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);

		for (float t = 0f; t < 1; t += Time.deltaTime / inTime) {

			transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
			yield return null;

		}

		transform.rotation = toAngle;
		bCanRotateCamera = true;

	}

}