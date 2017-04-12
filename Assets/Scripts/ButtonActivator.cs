using UnityEngine;
using System.Collections;

public class ButtonActivator : MonoBehaviour {
	
	public Transform[] barriers;
	
	public iTween.EaseType easeType;

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

		RoomController.canRotateCamera = false;
		
		Vector3 target = objectTransform.position + (distanceScale * translationVector);
		
		iTween.MoveTo(objectTransform.gameObject, iTween.Hash("position", target, "easetype", easeType, "time", fDuration));

		yield return new WaitForSeconds(fDuration);
		RoomController.canRotateCamera = true;
		Destroy(gameObject);

	}

}