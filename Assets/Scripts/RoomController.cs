using System.Collections;
using DG.Tweening;
using UnityEngine;
using XboxCtrlrInput;

public class RoomController : MonoBehaviour {

    private ParticleController particleController;

    public bool canRotateCamera = true;

    public bool canReceiveInput = true;

    [HeaderAttribute("Rotation")]
    public Ease rotationEaseType = Ease.OutQuad;
    [RangeAttribute(0.3f, 1.0f)]
    public float rotationLength = 0.5f;


    private PlayerController playerController;

    void Start() {

        particleController = GetComponent<ParticleController>();
        playerController = FindObjectOfType<PlayerController>();

    }

    void Update() {

        // Don't do anything if the game's curently paused
        if (GameManager.Instance.currentState != GameState.Play || !playerController || !canReceiveInput) {
            return;
        }

        // Allow for camera rotation ONLY if the player meets the following criteria
        if ((canRotateCamera) && (!playerController.isMoving) && (!GameManager.Instance.isLevelComplete)) {

#if UNITY_STANDALONE || UNITY_EDITOR

            if (XCI.GetAxisRaw(XboxAxis.LeftStickX) > 0 || Input.GetKey(KeyCode.D) || MobileInputController.Instance.SwipeRight) {

                StartCoroutine(RotateCamera(true));

            }
            else if (XCI.GetAxisRaw(XboxAxis.LeftStickX) < 0 || Input.GetKey(KeyCode.A) || MobileInputController.Instance.SwipeLeft) {

                StartCoroutine(RotateCamera(false));

            }

#elif UNITY_IOS || UNITY_ANDROID

            if (MobileInputController.Instance.SwipeRight)
            {

                StartCoroutine(RotateCamera(true));

            }
            else if (MobileInputController.Instance.SwipeLeft)
            {

                StartCoroutine(RotateCamera(false));

            }

#endif

        }

    }

    /// <summary>
    /// Rotates the camera.
    /// </summary>
    public IEnumerator RotateCamera(bool shouldRotateRight) {

        // AudioManager.Instance.PlayWithRandomPitch("Move", 0.95f, 1.05f);
        AudioManager.Instance.Play("Move");

        GameManager.Instance.IncrementMoveCount();

        playerController.movementParticles.transform.DOScale(0, 0);

        canRotateCamera = false;

        Vector3 eulerRotation = transform.eulerAngles;

        if (shouldRotateRight) {
            eulerRotation.z -= 90;
            AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeRight);
        }
        else {
            eulerRotation.z += 90;
            AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeLeft);
        }

        transform.DORotate(eulerRotation, rotationLength).SetEase(rotationEaseType);

        float wait = rotationLength * 0.2f;

        yield return new WaitForSeconds(wait);

        particleController.Play();

        yield return new WaitForSeconds(rotationLength - wait);

        canRotateCamera = true;

        particleController.Stop();

        yield return new WaitForSeconds(0.05f);

        playerController.CalculateMovementDirection();

    }

}