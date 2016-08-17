using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody body;

    // Use this for initialization
    void Awake()
    {

        body = GetComponent<Rigidbody>();

    }
	
    // Update is called once per frame
    void Update()
    {

        // Freeze the object's rotation and position whenever the camera is moving
        if (!MapController.canRotateCamera)
        {
			body.isKinematic = true;

        }
		// Remove every constrain except for the X position and rotation
		else
        {
			
			body.isKinematic = false;
			// Round the position vector
            transform.position = new Vector3((float)System.Math.Round(transform.position.x, 1), (float)System.Math.Round(transform.position.y, 1), (float)System.Math.Round(transform.position.z, 1));


        }

        // TODO: add a constrain that allows the camera to rotate ONLY if the player isn't moving
        
	
    }
}