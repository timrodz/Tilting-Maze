using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	// Make sure the player object can move when the camera has stopped rotating
	bool bCanMove;

	Rigidbody body;

	// Use this for initialization
	void Awake () {

		body = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
		
		bCanMove = MapController.bCanRotateCamera;

		if (!bCanMove) {

			body.constraints = RigidbodyConstraints.FreezeAll | RigidbodyConstraints.FreezePosition;

		}
		else {

			body.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX;

		}
	
	}
}
