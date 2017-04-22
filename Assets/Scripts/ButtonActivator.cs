using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonActivator : MonoBehaviour {

    private GameManager gameManager;

    public int numberOfUsesBeforeDestroying = 1;
    public Ease easeType = Ease.OutSine;

    public float duration = 1;

    public List<Barrier> barrierList = new List<Barrier> (1);

    // Some barriers can be toggled on and off, this controls it
    private bool canRegisterCollisions = true;
    private int collisionCount = 0;

    // -------------------------------------------------------------------------------------------

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        gameManager = FindObjectOfType<GameManager> ();

    }

    private void OnTriggerEnter(Collider other) {

        if (!canRegisterCollisions || !RoomController.canRotateCamera)
            return;

        Debug.Log("Entering " + this.name);

        Debug.Log("Room rotation: " + (int) gameManager.roomController.transform.eulerAngles.z);

        canRegisterCollisions = false;

        gameManager.soundManager.Play(Clip.triggerButton);

        foreach(Barrier barrier in barrierList) {

            if (barrier.gameObject != null) {
                Debug.Log("Barrier: " + barrier.gameObject.name);
                StartCoroutine(MoveBarrier(barrier));
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
    void OnTriggerExit(Collider other) {

        if (collisionCount >= numberOfUsesBeforeDestroying && numberOfUsesBeforeDestroying > 0)
            return;

        Debug.Log("Exiting " + this.name);

        canRegisterCollisions = true;

    }

    private IEnumerator MoveBarrier(Barrier barrier) {

        // Disable input
        RoomController.canReceiveInput = false;

        GameObject obj = barrier.gameObject;

        Vector3 movementDirection;

        if (!barrier.hasMoved) {

            // if ((int) gameManager.roomController.transform.eulerAngles.z == 180 || (int) gameManager.roomController.transform.eulerAngles.z == -180) {

            //     Debug.Log("Opposite direction");
            //     movementDirection = VectorDirection.DetermineOppositeDirection(barrier.movementDirection);

            // } else {

            Debug.Log("Normal direction");
            movementDirection = VectorDirection.DetermineDirection(barrier.movementDirection);

            // }

        } else {

            //     if ((int) gameManager.roomController.transform.eulerAngles.z == 180 || (int) gameManager.roomController.transform.eulerAngles.z == -180) {

            //         Debug.Log("Normal direction");
            //         movementDirection = VectorDirection.DetermineDirection(barrier.movementDirection);

            //     } else {

            Debug.Log("Opposite direction");
            movementDirection = VectorDirection.DetermineOppositeDirection(barrier.movementDirection);

            //     }

        }

        float scale = barrier.movementDistance;

        Vector3 finalPosition = obj.transform.localPosition + (movementDirection * scale);

        obj.transform.DOLocalMove(finalPosition, duration).SetEase(easeType);

        // Wait for a small amount of time and disable camera movement
        // Prevents the player from moving
        yield return new WaitForSeconds(0.1f);

        RoomController.canRotateCamera = false;

        // Give a bit of delay in case of any glitches
        yield return new WaitForSeconds(duration);

        RoomController.canReceiveInput = true;

        RoomController.canRotateCamera = true;

        if (collisionCount >= numberOfUsesBeforeDestroying && numberOfUsesBeforeDestroying > 0) {

            // Destroy the trigger button once the animations have finished
            Destroy(this.gameObject);

        } else {

            barrier.hasMoved = !barrier.hasMoved;

            if (barrier.shouldDeleteFromList) {
                barrierList.Remove(barrier);
            }

        }

    }

}