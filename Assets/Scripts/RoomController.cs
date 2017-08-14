using System.Collections;
using DG.Tweening;
using UnityEngine;
using XboxCtrlrInput;

public class RoomController : MonoBehaviour
{
    [SerializeField] private ParticleController particleController;

    public bool canRotate = true;

    public bool registerInput = true;

    [SerializeField] private Ease rotationEaseType = Ease.OutQuad;
    [SerializeField] private float rotationLength = 0.35f;

    private PlayerController playerController;

    void Start()
    {
        particleController = GetComponent<ParticleController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        // Don't do anything if the game's curently paused
        if (GameManager.Instance.currentState != GameState.Play || !playerController || !registerInput)
        {
            return;
        }

        // Allow for camera rotation ONLY if the player meets the following criteria
        if ((canRotate) && (!playerController.isMoving) && (!GameManager.Instance.isLevelComplete))
        {

#if UNITY_STANDALONE || UNITY_EDITOR

            if (XCI.GetAxisRaw(XboxAxis.LeftStickX) > 0 || Input.GetKey(KeyCode.D) || MobileInputController.Instance.SwipeRight)
            {
                StartCoroutine(Rotate(true));
            }
            else if (XCI.GetAxisRaw(XboxAxis.LeftStickX) < 0 || Input.GetKey(KeyCode.A) || MobileInputController.Instance.SwipeLeft)
            {
                StartCoroutine(Rotate(false));
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
    public IEnumerator Rotate(bool shouldRotateRight)
    {
        AudioManager.Instance.PlayWithRandomPitch("Move", 0.98f, 1.02f);

        GameManager.Instance.IncrementMoveCount();

        playerController.movementParticles.transform.DOScale(0, 0);

        canRotate = false;

        Vector3 eulerRotation = transform.eulerAngles;

        if (shouldRotateRight)
        {
            eulerRotation.z -= 90;
            AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeRight);
        }
        else
        {
            eulerRotation.z += 90;
            AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeLeft);
        }

        transform.DORotate(eulerRotation, rotationLength).SetEase(rotationEaseType);

        float wait = rotationLength * 0.2f;

        yield return new WaitForSeconds(wait);

        particleController.Play();

        yield return new WaitForSeconds(rotationLength - wait);

        canRotate = true;

        particleController.Stop();

        yield return new WaitForSeconds(0.05f);

        playerController.CalculateMovementDirection();

    }

}