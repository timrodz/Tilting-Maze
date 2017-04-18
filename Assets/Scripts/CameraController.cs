using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
	private GameManager gameManager;

    [HeaderAttribute("Screen Shake")]
    [RangeAttribute(0.3f, 1f)]
    public float shakeDuration = 0.5f;

    [HideInInspector] public Camera mainCamera;

    private Vector3 originalPosition;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
    
	    mainCamera = GetComponent<Camera>();
		gameManager = FindObjectOfType<GameManager>();
    
	}

    // Use this for initialization
    void Start() {

        originalPosition = transform.position;

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
		
		transform.DOMove(originalPosition, 2);
		
		yield return new WaitForSeconds(2);
		
		gameManager.SetState(GameState.Playing);
		
	}

}