using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    // Game state
    [HideInInspector] public GameState currentState = GameState.LoadingLevel;
    [HideInInspector] public GameState previousState = GameState.LoadingLevel;
    [HideInInspector] public static int moveCount;

    // Pause states
    [HideInInspector] public bool canPause = true;
    [HideInInspector] public bool isPaused = false;

    // Level ending
    [HideInInspector] public bool isLevelComplete = false;

    public GameObject levelCompletePanel;

    public UnityEvent OnLevelComplete;

    // -------------------------------------------------------------------------------------------

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        if (Instance != null & Instance != this) {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        levelCompletePanel.SetActive(true);

    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {

        SetState(GameState.LoadingLevel);

        StartLevel();

    }

    void Update() {

        // if (isLevelComplete && currentState == GameState.LevelComplete) {

        //     if (Input.GetKeyDown(KeyCode.Return) || XCI.GetButtonDown(XboxButton.A)) {

        //         SetState(GameState.LoadingLevel);
        //         LoadNextLevel(false);

        //     }

        //     if (Input.GetKeyDown(KeyCode.Escape) || XCI.GetButtonDown(XboxButton.B)) {

        //         LoadNextLevel(true);

        //     }

        // }

    }

    /// <summary>
    /// Sets the game state
    /// </summary>
    /// <param name="state"></param>
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
            //pausePanel.gameObject.SetActive(true);

        }
        // Unpause the game
        else {

            currentState = previousState;
            //pausePanel.gameObject.SetActive(false);

        }

    }

    public void StartLevel() {

        // ATTENTION: Sets the state to "Play" in the camera controller script

        moveCount = 0;
        isLevelComplete = false;
        canPause = true;
        isPaused = false;

        CanvasManager.Instance.ResetTotalMovesPosition();

    }

    /// <summary>
    /// Finish the current level
    /// Show the moves it took to complete
    /// And the prompt for loading the next level
    /// </summary>
    public void CompleteLevel() {

        DOTween.KillAll();

        StopAllCoroutines();

        SetState(GameState.LevelComplete);

        OnLevelComplete.Invoke();

        // FindObjectOfType<LevelCompleteAnimation>().PlayAnimation();

    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    public void LoadNextLevel() {

        GameObject currentLevel = FindObjectOfType<RoomController>().gameObject;

        // Get the current level's string and load the next level based on hierarchy
        string nextLevelName = Utils.FindStringAndIncrementNumber(currentLevel.name);

        string levelNumber = Utils.FindAndIncrementNumberInString(currentLevel.name);

        // Find the level prefab by loading the resources directly
        Object nextLevelPrefab = Resources.Load(nextLevelName);

        // If there's a next level, create it
        if (nextLevelPrefab) {

            StartCoroutine(DestroyCurrentLevelAndInstantiateNextLevelPrefab(currentLevel, nextLevelPrefab, nextLevelName));

            FindObjectOfType<NextLevelAnimator>().ChangeLevelText(levelNumber);

        }
        // Otherwise, go back to the menu
        else {

            SceneManager.LoadScene("Level Selection");

        }

    }

    private IEnumerator DestroyCurrentLevelAndInstantiateNextLevelPrefab(GameObject currentLevel, Object nextLevelPrefab, string nextLevelName) {

        currentLevel.transform.DOScale(1.25f, 1.25f).SetEase(Ease.InOutBack);
        yield return new WaitForSeconds(1.5f);
        
        // currentLevel.transform.DOScale(1, 1.5f).SetEase(Ease.InOutBack);
        // yield return new WaitForSeconds(0.5f);

        currentLevel.transform.DORotate(transform.eulerAngles * 180, 1.5f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(1.35f);

        // Fade out and destroy current room
        currentLevel.transform.DOScale(0, 1).SetEase(Ease.OutExpo);
        yield return new WaitForSeconds(1);
        Destroy(currentLevel);

        GameObject nextLevel = (GameObject) Instantiate(nextLevelPrefab, Vector3.zero, Quaternion.identity);
        nextLevel.name = nextLevelName;

        nextLevel.transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(3.5f);

        nextLevel.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack);

        // yield return new WaitForSeconds(1);
        // nextLevel.transform.DORotate(nextLevel.transform.eulerAngles * -180, 1f).SetEase(Ease.OutBack);
        
        yield return new WaitForSeconds(1);

        // Utils.Fade(FindObjectOfType<LevelCompleteAnimation>().transparency, false, 0);
        CameraController.Instance.ResetPosition();
        CanvasManager.Instance.ResetTotalMovesPosition();

        StartLevel();

        SetState(GameState.Play);

    }

    /// <summary>
    /// 
    /// </summary>
    public void IncrementMoveCount() {

        moveCount++;

        CanvasManager.Instance.totalMovesText.text = "Moves: " + moveCount.ToString();

        if (moveCount == 1) {

            Utils.Fade(CanvasManager.Instance.TotalMovesPanelTransparency, true, 1);

        }

    }

}