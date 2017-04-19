#define MORELEVELS
using System.Collections;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // References
    public CameraController cameraController;
    public SoundManager soundManager;
    [HideInInspector] public RoomController roomController;
    [HideInInspector] public PlayerController playerController;

    // UI

    [HeaderAttribute("UI Elements")]
    public TextMeshProUGUI totalMovesText;
    public Button nextLevelButton;
    public Image pausePanel;
    private CanvasGroup nextLevelCG;
    private CanvasGroup movesCG;

    // Winning state

    [HeaderAttribute("Winning animation")]
    public Ease winningAnimationEaseType;
    public Vector3 winningAnimationCameraPosition;
    public Vector3 winningAnimationPlayerPosition;
    public float winningAnimationDuration;

    // Game state
    [HideInInspector] public static int moveCount;
    [HideInInspector] public GameState currentState = GameState.Playing;
    [HideInInspector] public GameState previousState = GameState.Playing;

    // Pause states
    [HideInInspector] public bool canPause = true;
    [HideInInspector] public bool isPaused = false;

    // Level ending
    [HideInInspector] public bool isLevelComplete = false;

    // -------------------------------------------------------------------------------------------

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {

        StartLevel();

    }

    public void StartLevel() {

        moveCount = 0;
        isLevelComplete = false;
        canPause = true;
        isPaused = false;

        roomController = FindObjectOfType<RoomController>();
        playerController = FindObjectOfType<PlayerController>();
        
        nextLevelCG = nextLevelButton.GetComponent<CanvasGroup>();
        movesCG = totalMovesText.GetComponentInParent<CanvasGroup>();

        Fade(nextLevelCG, false, 0);
        Fade(movesCG, false, 0);

        totalMovesText.text = "Moves: " + moveCount.ToString();

    }

    public void SetState(GameState state) {

        previousState = currentState;
        currentState = state;

    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void TogglePause() {

        // Pause the game if it's not
        if (currentState != GameState.Paused) {

            previousState = currentState;
            currentState = GameState.Paused;
            pausePanel.gameObject.SetActive(true);

        }
        // Unpause the game
        else {

            currentState = previousState;
            pausePanel.gameObject.SetActive(false);

        }

    }

    /// <summary>
    /// Finish the current level
    /// Show the moves it took to complete
    /// And the prompt for loading the next level
    /// </summary>
    public void CompleteLevel() {

        StartCoroutine(AnimateLevelCompletion());

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimateLevelCompletion() {

        SetState(GameState.LoadingLevel);

        canPause = false;
        isLevelComplete = true;

        playerController.collisionParticles.Play();
        cameraController.Shake();

        yield return new WaitForSeconds(cameraController.shakeDuration);

        cameraController.transform.DOMove(winningAnimationCameraPosition, winningAnimationDuration).SetEase(winningAnimationEaseType);

        Transform player = playerController.transform;

        player.DOMove(winningAnimationPlayerPosition, winningAnimationDuration).SetEase(winningAnimationEaseType);
        player.DOScale(new Vector3(25, 25, 1), winningAnimationDuration).SetEase(winningAnimationEaseType);

        Vector3 euler = player.eulerAngles;
        euler.z -= 360;

        player.DORotate(euler, winningAnimationDuration, RotateMode.FastBeyond360).SetEase(winningAnimationEaseType);

        yield return new WaitForSeconds(winningAnimationDuration);

        totalMovesText.rectTransform.DOLocalMove(Vector3.zero, 1);
        Fade(nextLevelCG, true, 1);

    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    public void LoadNextLevel() {

        GameObject currentLevel = roomController.gameObject;

        // Get the current level's string and load the next level based on hierarchy
        string nextLevelName = TrimString(currentLevel.name, true);

        // Destroy the current level
        Destroy(currentLevel);

        // Find the level prefab by loading the resources directly
        Object nextLevelPrefab = Resources.Load(nextLevelName);

        // If there's a next level, create it
        if (nextLevelPrefab) {

            cameraController.ResetPosition();
            totalMovesText.rectTransform.localPosition = new Vector3(-306, 200, 0);
            GameObject levelToInstantiate = (GameObject) Instantiate(nextLevelPrefab, Vector3.zero, Quaternion.identity);
            levelToInstantiate.name = nextLevelName;
            StartLevel();

        }
        // Otherwise, go back to the menu
        else {

            SceneManager.LoadScene("Level Selection");

        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="fadeIn"></param>
    /// <param name="duration"></param>
    private void Fade(CanvasGroup canvasGroup, bool fadeIn, float duration) {

        if (fadeIn) {

            canvasGroup.DOFade(1, duration);
            canvasGroup.blocksRaycasts = true;

        } else {

            canvasGroup.DOFade(0, duration);
            canvasGroup.blocksRaycasts = false;

        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void IncrementMoveCount() {

        moveCount++;
        totalMovesText.text = "Moves: " + moveCount.ToString();

        if (moveCount == 1) {
            
            Fade(movesCG, true, 1);
            
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="hasNumber"></param>
    /// <returns></returns>
    private string TrimString(string str, bool hasNumber) {

        int length = str.Length - 1;

        // Find the first parentheses and delete everything
        // that follows after it
        int index = str.IndexOf('-');

#if (MORELEVELS)

        if (hasNumber) {

            // Recalculate the new length
            length = str.Length - 1;

            // store the number it has and remove it
            string number = Regex.Match(str, @"\d+").Value;

            // parse the number from the string and add 1 to it
            int num = int.Parse(number) + 1;

            // Remove the number at the top
            str = str.Remove(index + 1);

            str += (num);

        }

#endif

        Debug.Log("Function TrimString returned: " + str);

        return str;

    }

}