using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonActivator : MonoBehaviour {

	public enum vectorDirections {
		Forward = 0,
		Back = 1,
		Up = 2,
		Down = 3
	};

	public vectorDirections vectorDirection;

	public float duration = 0.99f;

	public Transform barrier;

	Vector3 vectorToTranslate, target;

	bool hasCollided = false;

	// Use this for initialization
	void Start() {

		switch (vectorDirection) {
			case vectorDirections.Forward:
				vectorToTranslate = Vector3.forward;
				break;
			case vectorDirections.Back:
				vectorToTranslate = Vector3.back;
				break;
			case vectorDirections.Up:
				vectorToTranslate = Vector3.up;
				break;
			case vectorDirections.Down:
			default:
				vectorToTranslate = Vector3.down;
				break;
		}

		target = barrier.position + vectorToTranslate;

	}

	void OnTriggerEnter(Collider other) {

		if (!hasCollided) {

			StartCoroutine(TranslateTo(barrier, vectorToTranslate));
			hasCollided = !hasCollided;

		}

	}

	IEnumerator TranslateTo(Transform _transform, Vector3 _position) {

		for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / duration)) {

			_transform.Translate(_position.normalized * Time.deltaTime, Space.World);
			yield return null;

		}

	}

}
