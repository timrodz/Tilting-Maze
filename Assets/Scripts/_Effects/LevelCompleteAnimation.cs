using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class LevelCompleteAnimation : MonoBehaviour {
    
    [HideInInspector]
    public CanvasGroup transparency;

    public Ease easeTye;
    public Vector3 cameraPosition;
    public Vector3 playerPosition;
    public float duration;
    public Vector3 playerScale;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        
        transparency = GetComponent<CanvasGroup>();
        Utils.Fade(transparency, false, 0);

    }

    public void PlayAnimation() {

        StartCoroutine(AnimationController());

    }

    private IEnumerator AnimationController() {
        
        CameraController.Instance.Shake();

        yield return new WaitForSeconds(CameraController.Instance.shakeDuration);

        CameraController.Instance.transform.DOMove(cameraPosition, duration).SetEase(easeTye);

        Transform player = FindObjectOfType<PlayerController>().transform;

        player.DOMove(playerPosition, duration).SetEase(easeTye);

        player.DOScale(playerScale, duration).SetEase(easeTye);
        
        // Rotate the player 360 degrees
        Vector3 euler = player.eulerAngles;
        euler.z -= 360;

        player.DORotate(euler, duration, RotateMode.FastBeyond360).SetEase(easeTye);

        yield return new WaitForSeconds(duration + 0.5f);

        // Move the "moves" text to the center
        Utils.Fade(CanvasManager.Instance.TotalMovesPanelTransparency, true, 1);
        
        CanvasManager.Instance.totalMovesText.rectTransform.DOLocalMove(Vector3.zero, 1);

        // Show the next level button by accessing its canvas group
        Utils.Fade(transparency, true, 1);

        yield return new WaitForSeconds(1);

        GameManager.Instance.isLevelComplete = true;

    }

}