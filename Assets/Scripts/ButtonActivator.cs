using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonActivator : MonoBehaviour {

    private RoomController room;

    public int numberOfUsesBeforeDestroying = 1;
    public Ease easeType = Ease.OutSine;

    public float duration = 1;

    public List<Barrier> barrierList = new List<Barrier>(1);

    // Some barriers can be toggled on and off, this controls it
    private bool canRegisterCollisions = true;
    private int collisionCount = 0;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {

        room = FindObjectOfType<RoomController>();

    }

    // -------------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other) {

        if (!canRegisterCollisions || !room.canRotateCamera || GameManager.Instance.currentState != GameState.Play)
            return;

        // Stop registering any other collisions
        canRegisterCollisions = false;

        AudioManager.Instance.Play("Trigger Button");

        // Make the player's position be the trigger's position
        other.transform.position = new Vector3(
            (float) System.Math.Round(transform.position.x, 1),
            (float) System.Math.Round(transform.position.y, 1),
            (float) System.Math.Round(other.transform.position.z, 1)
        );
        
        StartCoroutine(CollisionHelper());

        foreach(Barrier barrier in barrierList) {

            if (barrier.gameObject != null) {

                StartCoroutine(MoveBarrier(barrier));

            }

        }

        // If the number of uses before destroying is 0, never destroy it
        if (numberOfUsesBeforeDestroying > 0)
            collisionCount++;

        Debug.Log("Interacting with " + this.name + " - Position: " + other.transform.position);

        AnalyticsManager.Instance.RegisterCustomEventTriggerEnter(transform.name);

    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other) {
        
        StopAllCoroutines();
        
        if (collisionCount >= numberOfUsesBeforeDestroying && numberOfUsesBeforeDestroying > 0)
            return;

        StartCoroutine(EnableCollisionRegistry());

        Debug.Log("Exiting " + this.name);

        AnalyticsManager.Instance.RegisterCustomEventTriggerExit(transform.name);

    }

    private IEnumerator EnableCollisionRegistry() {

        yield return new WaitForSeconds(1f);
        canRegisterCollisions = true;

    }
    
    private IEnumerator CollisionHelper() {
        
        room.canReceiveInput = false;
        room.canRotateCamera = false;
        
        yield return new WaitForSeconds(duration);
        
        room.canReceiveInput = true;

        room.canRotateCamera = true;
        
        // yield return new WaitForSeconds(0.1f);
        // Debug.Log("pasd");

        // player.canCheckForCollisions = true;
        // Debug.Log("Check for player collisions");
        // if (!player.CheckForCollisions()) {

        //     Debug.Log("FAIL");
        //     player.canCheckForCollisions = true;

        // } else {

        //     Debug.Log("PASS");

        // }
        
    }

    private IEnumerator MoveBarrier(Barrier barrier) {

        GameObject obj = barrier.gameObject;

        Vector3 movementDirection;

        if (!barrier.hasMoved) {

            movementDirection = VectorDirection.DetermineDirection(barrier.movementDirection);

        } else {

            movementDirection = VectorDirection.DetermineOppositeDirection(barrier.movementDirection);

        }

        float scale = barrier.movementDistance;

        Vector3 finalPosition = obj.transform.localPosition + (movementDirection * scale);

        obj.transform.DOLocalMove(finalPosition, duration).SetEase(easeType);

        // Give a bit of delay in case of any glitches
        yield return new WaitForSeconds(duration);

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