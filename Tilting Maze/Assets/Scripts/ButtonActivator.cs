using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonActivator : MonoBehaviour {

	public VectorDirection.directions vectorDirection;

	public Transform barrier;

	private float fDuration = 0.99f;

	Vector3 vectorToTranslate, target;

	// Use this for initialization
	void Start() {

		vectorToTranslate = VectorDirection.DetermineDirection(vectorDirection);

	}

	void OnTriggerEnter(Collider other) {

		target = barrier.position + vectorToTranslate;
		//MapController.canRotateCamera = false;
		StartCoroutine(TranslateTo(barrier, vectorToTranslate));

	}

	// Function TranslateTo
	// param _transform : the selected object's transform
	// param _position : the position to translate to

	IEnumerator TranslateTo(Transform _transform, Vector3 _position) {

		yield return new WaitForSeconds(0.0f);
		MapController.canRotateCamera = false;

		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / fDuration)) {

			_transform.Translate(_position.normalized * Time.deltaTime, Space.World);
			yield return null;

		}

		// Destroy the button after the translation has finished
		_transform.position = target;
		MapController.canRotateCamera = true;
		Destroy(gameObject);

	}

}