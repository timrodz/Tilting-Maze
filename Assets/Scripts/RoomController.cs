using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RoomController : MonoBehaviour
{

    public bool canRotate = true;

    public bool registerInput = true;

    [SerializeField] private Ease rotationEaseType = Ease.OutQuad;
    [SerializeField] private float rotationLength = 0.35f;

    private PlayerController playerController;
    private ParticleController particleController;

    void Awake()
    {
        particleController = GetComponent<ParticleController>(); 
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        // Don't do anything if the game's curently paused
        if (GameManager.Instance.GetState() != GameState.Play || !playerController || !registerInput)
        {
            return;
        }

        // Allow for camera rotation ONLY if the player meets the following criteria
        if ((canRotate) && (!playerController.isMoving) && (!GameManager.Instance.IsLevelComplete()))
        {
#if UNITY_STANDALONE || UNITY_EDITOR

            if (Input.GetKey(KeyCode.D) || (MobileInputController.Instance.SwipeRight))
            {
                StartCoroutine(Rotate(true));
            }
            else if (Input.GetKey(KeyCode.A) || (MobileInputController.Instance.SwipeLeft))
            {
                StartCoroutine(Rotate(false));
            }
#elif UNITY_IOS || UNITY_ANDROID

            if (MobileInputController.Instance.SwipeRight)
            {
                StartCoroutine(Rotate(true));
            }
            else if (MobileInputController.Instance.SwipeLeft)
            {
                StartCoroutine(Rotate(false));
            }
#endif

        }

    }

    /// <summary>
    /// Rotates the camera.
    /// </summary>
    public IEnumerator Rotate(bool shouldRotateRight)
    {
        GameManager.Instance.IncrementMoveCount();

        playerController.movementParticles.transform.DOScale(0, 0);

        canRotate = false;
        
        // particleController.Play();

        Vector3 eulerRotation = transform.eulerAngles;

        if (shouldRotateRight)
        {
            eulerRotation.z -= 90;
            // AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeRight);
        }
        else
        {
            eulerRotation.z += 90;
            // AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeLeft);
        }

        transform.DORotate(eulerRotation, rotationLength).SetEase(rotationEaseType);

        float wait = rotationLength * 0.2f;

        yield return new WaitForSeconds(wait);

        yield return new WaitForSeconds(rotationLength - wait);

        canRotate = true;

        yield return new WaitForSeconds(0.05f);

        playerController.CalculateMovementDirection();

    }

}