using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class ButtonActivator : MonoBehaviour {

    public Ease easeType = Ease.OutSine;

    public float duration = 1;

    public List<Barrier> barrierList = new List<Barrier>(1);

    // -------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other) {

        Debug.Log("Activating " + this.name);

        foreach(Barrier barrier in barrierList) {

            if (barrier.gameObject != null) {
                StartCoroutine(MoveBarrier(barrier));
            }

        }

    }

    private IEnumerator MoveBarrier(Barrier barrier) {
		
		// Disable input
        RoomController.canReceiveInput = false;

        GameObject obj = barrier.gameObject;

        Vector3 movementDirection = VectorDirection.DetermineDirection(barrier.movementDirection);

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
		
		// Destroy the trigger button once the animations have finished
        Destroy(this.gameObject);

    }

}