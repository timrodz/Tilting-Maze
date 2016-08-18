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

    }

    void OnTriggerEnter(Collider other) {

        target = barrier.position + vectorToTranslate;
        MapController.canRotateCamera = false;
        StartCoroutine(TranslateTo(barrier, vectorToTranslate));

    }

    // Function TranslateTo
	// @param _transform : the selected object's transform
	// @param _position : the position to translate to
	// 
	// FIX:
	// Whenever the player touches the button.
	
    IEnumerator TranslateTo(Transform _transform, Vector3 _position) {

        for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / duration)) {

            _transform.Translate(_position.normalized * Time.deltaTime, Space.World);
            yield return null;

        }

        // Destroy the button after the translation has finished
        _transform.position = target;
        MapController.canRotateCamera = true;
        Destroy(gameObject);

    }

}