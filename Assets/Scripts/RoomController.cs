﻿using System.Collections;
using DG.Tweening;
using UnityEngine;
using XboxCtrlrInput;

public class RoomController : MonoBehaviour {

    private GameManager gameManager;
    private ParticleController particleController;

    [HideInInspector] public static bool canRotateCamera = true;

    [HideInInspector] public static bool canReceiveInput = true;

    [HeaderAttribute("Rotation")]
    public Ease rotationEaseType = Ease.OutQuad;
    [RangeAttribute(0.3f, 1.0f)]
    public float rotationLength = 0.5f;

    [HeaderAttribute("Player")]
    public GameObject playerObject;
    private PlayerController playerController;


    // -------------------------------------------------------------------------------------------

    void Start() {

        particleController = GetComponent<ParticleController>();
        playerController = playerObject.GetComponent<PlayerController>();

        gameManager = FindObjectOfType<GameManager>();

    }

    void Update() {

        // Don't do anything if the game's curently paused
        if (gameManager.currentState != GameState.Playing || !playerObject || !canReceiveInput) {
            return;
        }

        // Allow for camera rotation ONLY if the player meets the following criteria
        if ((canRotateCamera) && (!playerController.isMoving) && (!gameManager.isLevelComplete)) {

            if (XCI.GetAxisRaw(XboxAxis.LeftStickX) > 0 || Input.GetKey(KeyCode.D)) {

                StartCoroutine(RotateCamera(true));

            } else if (XCI.GetAxisRaw(XboxAxis.LeftStickX) < 0 || Input.GetKey(KeyCode.A)) {

                StartCoroutine(RotateCamera(false));

            }

        }

    }

    /// <summary>
    /// Rotates the camera.
    /// </summary>
    public IEnumerator RotateCamera(bool shouldRotateRight) {
        
        gameManager.IncrementMoveCount();
        
        playerController.movementParticles.transform.DOScale(0, 0);

        canRotateCamera = false;

        Vector3 eulerRotation = transform.eulerAngles;

        if (shouldRotateRight) {
            eulerRotation.z -= 90;
        } else {
            eulerRotation.z += 90;
        }

        transform.DORotate(eulerRotation, rotationLength).SetEase(rotationEaseType);

        float wait = rotationLength * 0.2f;

        yield return new WaitForSeconds(wait);

        particleController.Play();

        yield return new WaitForSeconds(rotationLength - wait);

        canRotateCamera = true;

        particleController.Stop();

    }

}