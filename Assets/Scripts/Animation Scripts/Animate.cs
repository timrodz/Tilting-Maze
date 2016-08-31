using UnityEngine;
using System.Collections;

public class Animate : MonoBehaviour {

	public enum AnimationTypes {
		FadeIn,
		FadeOut,
		ZoomIn,
		ZoomOut,
		FadeZoomIn}

	;

	public Transform objectToAnimate;

	public AnimationTypes typeOfAnimation;

	public VectorDirection.directions directionToAnimateFrom;

	public float animationDuration = 1.0f;

	public float animationScale = 1.0f;

	private Vector3 targetPosition;

		
	void OnEnable() {

		if (objectToAnimate == null)
			objectToAnimate = transform;

		// The target position will be the original transform's position
		targetPosition = objectToAnimate.transform.position;

		// This will be the position where the object will appear from
		Vector3 fromPosition = targetPosition + (VectorDirection.DetermineDirection(directionToAnimateFrom) * animationScale);

		// Position the transform at the updated position
		objectToAnimate.transform.position = fromPosition;

		// Animate based on the chosen option
		switch (typeOfAnimation) {

			case AnimationTypes.FadeIn:
				StartCoroutine(FadeIn());
				break;
			case AnimationTypes.FadeOut:
				StartCoroutine(FadeOut());
				break;
			case AnimationTypes.ZoomIn:
				StartCoroutine(ZoomIn());
				break;
			case AnimationTypes.ZoomOut:
				StartCoroutine(ZoomOut());
				break;
			case AnimationTypes.FadeZoomIn:
				StartCoroutine(FadeZoomIn());
				break;
			default:
				break;

		}

	}

	void Update() {

		// Translate the object from its position to the target position
		objectToAnimate.transform.position = Vector3.MoveTowards(objectToAnimate.transform.position, targetPosition, (Time.deltaTime * 5f));

	}

	/// ////////// /// ////////// /// ////////// ///
	/// ANIMATIONS /// ANIMATIONS /// ANIMATIONS /// 
	/// ////////// /// ////////// /// ////////// ///

	/// <summary>
	/// Access the object's canvas renderer (alpha) and change them from 0 to 1 in the time desired
	/// </summary>
	public IEnumerator FadeIn() {

		for (float t = 0.0f; t <= 1.0f; t += (Time.deltaTime / animationDuration)) {
			
			objectToAnimate.transform.GetComponent<CanvasRenderer>().SetAlpha(t);
			yield return null;

		}

	}

	// Animation: Fade Out
	// Access the object's canvas renderer (for its alpha value)
	// Change the alpha values from 1 to 0 in the time desired

	public IEnumerator FadeOut() {

		for (float t = 1.0f; t >= 0.0f; t -= (Time.deltaTime / animationDuration)) {

			objectToAnimate.transform.GetComponent<CanvasRenderer>().SetAlpha(t);
			yield return null;

		}

	}

	// Animation: Zoom In
	// Access the object's scale
	// Change the scale values from zoomScale to 1 in the time desired

	public IEnumerator ZoomIn() {


		yield return null;

	}

	// Animation: Zoom Out
	// Access the object's scale
	// Change the scale values from 1 to ZoomScale in the time desired

	public IEnumerator ZoomOut() {


		yield return null;

	}

	// Animation: Fade In, Zoom In
	// Access the object's canvas renderer (for its alpha value) and its scale
	// Change the alpha values from 0 to 1 in the time desired
	// Change the scale values from zoomScale to 1 in the time desired

	public IEnumerator FadeZoomIn() {


		yield return null;

	}

}
