using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonActivator : MonoBehaviour {
	
	public Transform[] barriers;

	public VectorDirection.directions vectorDirection;

	public int distanceScale = 1;

	private float fDuration = 0.99f;

	Vector3 translationVector;


	private void Start() {

		translationVector = VectorDirection.DetermineDirection(vectorDirection);

	}

	private void OnTriggerEnter(Collider other) {

		foreach (Transform barrier in barriers) {

			StartCoroutine(TranslateTo(barrier));
			
		}

	}

	/// Translates to the desired position
	private IEnumerator TranslateTo(Transform objectTransform) {

		Vector3 target = objectTransform.position + (distanceScale * translationVector);

		yield return new WaitForSeconds(0.0f);
		MapController.canRotateCamera = false;

		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / fDuration)) {

			objectTransform.Translate(translationVector.normalized * Time.deltaTime, Space.World);
			yield return null;

		}

		// Destroy the button after the translation has finished
		objectTransform.position = target;
		MapController.canRotateCamera = true;
		Destroy(gameObject);

	}

}