using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
    public static CameraController Instance { get; private set; }
    
    [Header("Camera Easing")]
    public Ease cameraEase;

    [Header("Screen Shake")]
    [RangeAttribute(0.3f, 1f)]
    public float shakeDuration = 0.5f;

    [HideInInspector] public Camera mainCamera;

    private Vector3 originalPosition;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        
        // Check if there is another instance of the same type and destroy it
        if (Instance != null & Instance != this) {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    
	    mainCamera = GetComponent<Camera>();
    
	}

    // Use this for initialization
    void Start() {

        originalPosition = transform.position;
        
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 7.5f);
        
        ResetPosition();

    }
	
	public void ResetPosition() {
		
		StartCoroutine(ResetPositionController());
		
	}

    public void Shake() {

        StartCoroutine(ShakeController());

    }

    private IEnumerator ShakeController() {

        transform.DOShakePosition(shakeDuration);

        yield return new WaitForSeconds(shakeDuration);

        transform.DOMove(originalPosition, 0.35f);

    }
	
	private IEnumerator ResetPositionController() {

		transform.DOMove(originalPosition, 2).SetEase(cameraEase);
		
		yield return new WaitForSeconds(2);
        
        GameManager.Instance.SetState(GameState.Play);
		
	}

}