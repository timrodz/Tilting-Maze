using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonActivator : MonoBehaviour {

    public int numberOfUsesBeforeDestroying = 1;
    public Ease easeType = Ease.OutSine;

    public float duration = 1;

    public List<Barrier> barrierList = new List<Barrier> (1);

    // Some barriers can be toggled on and off, this controls it
    private bool canRegisterCollisions = true;
    private int collisionCount = 0;

    // -------------------------------------------------------------------------------------------

    private void OnTriggerEnter (Collider other) {

        if (!canRegisterCollisions || !RoomController.canRotateCamera || GameManager.Instance.currentState != GameState.Play)
            return;

        Debug.Log ("Interacting with " + this.name + " - Position: " + other.transform.position);

        // Debug.Log("Room rotation: " + (int) gameManager.roomController.transform.eulerAngles.z);

        // RoomController.canRotateCamera = false;
        
        // Make the player's position be the trigger's position
        other.transform.position = new Vector3 (
            (float) System.Math.Round (transform.position.x, 1),
            (float) System.Math.Round (transform.position.y, 1),
            (float) System.Math.Round (other.transform.position.z, 1)
        );
        
        // Stop registering any other collisions
        canRegisterCollisions = false;

        AudioManager.Instance.Play("Trigger Button");

        foreach (Barrier barrier in barrierList) {

            if (barrier.gameObject != null) {

                StartCoroutine (MoveBarrier (barrier));

            }

        }

        // If the number of uses before destroying is 0, never destroy it
        if (numberOfUsesBeforeDestroying > 0)
            collisionCount++;

    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit (Collider other) {

        if (collisionCount >= numberOfUsesBeforeDestroying && numberOfUsesBeforeDestroying > 0)
            return;

        Debug.Log ("Exiting " + this.name);

        canRegisterCollisions = true;

    }

    private IEnumerator MoveBarrier (Barrier barrier) {

        // Disable input
        RoomController.canReceiveInput = false;

        GameObject obj = barrier.gameObject;

        Vector3 movementDirection;

        if (!barrier.hasMoved) {

            movementDirection = VectorDirection.DetermineDirection (barrier.movementDirection);

        } else {

            movementDirection = VectorDirection.DetermineOppositeDirection (barrier.movementDirection);

        }

        float scale = barrier.movementDistance;

        Vector3 finalPosition = obj.transform.localPosition + (movementDirection * scale);

        obj.transform.DOLocalMove (finalPosition, duration).SetEase (easeType);

        // Wait for a small amount of time and disable camera movement
        // Prevents the player from moving
        // yield return new WaitForSeconds(0.1f);

        RoomController.canRotateCamera = false;

        // Give a bit of delay in case of any glitches
        yield return new WaitForSeconds (duration);

        RoomController.canReceiveInput = true;

        RoomController.canRotateCamera = true;

        if (collisionCount >= numberOfUsesBeforeDestroying && numberOfUsesBeforeDestroying > 0) {

            // Destroy the trigger button once the animations have finished
            Destroy (this.gameObject);

        } else {

            barrier.hasMoved = !barrier.hasMoved;

            if (barrier.shouldDeleteFromList) {
                barrierList.Remove (barrier);
            }

        }

    }

}