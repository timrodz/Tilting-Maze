using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private bool DEBUG;

    // Game state
    [HideInInspector] public int levelID = 0;
    [HideInInspector] public float levelTime = 0.0f;

    [HideInInspector] public GameState currentState = GameState.LoadingLevel;
    [HideInInspector] public GameState previousState = GameState.LoadingLevel;
    [HideInInspector] public static int moveCount;

    // Pause states
    [HideInInspector] public bool canPause = true;
    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool canUpdateTime = false;

    // Level ending
    [HideInInspector] public bool isLevelComplete = false;

    public UnityEvent OnLevelComplete;

    // -------------------------------------------------------------------------------------------

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        if (Instance != null & Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

        SetState(GameState.LoadingLevel);

        StartLevel();

        SetLevelID(1);

    }

    void LateUpdate()
    {
        if (currentState == GameState.Play)
        {
            levelTime += Time.deltaTime;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetState(GameState.LoadingLevel);
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("==== RELOADING SCENE ====");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif

    }

    /// <summary>
    /// Sets the game state
    /// </summary>
    /// <param name="state"></param>
    public void SetState(GameState state)
    {
        previousState = currentState;
        currentState = state;
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void TogglePause()
    {
        // Pause the game if it's not
        if (currentState != GameState.Paused)
        {

            previousState = currentState;
            currentState = GameState.Paused;
            //pausePanel.gameObject.SetActive(true);

        }
        // Unpause the game
        else
        {

            currentState = previousState;
            //pausePanel.gameObject.SetActive(false);

        }

    }

    public void StartLevel()
    {
        moveCount = 0;
        isLevelComplete = false;
        canPause = true;
        isPaused = false;

        // ATTENTION: Sets the state to "Play" in the camera controller script
        CanvasManager.Instance.ResetTotalMovesPanelPosition();
    }

    /// <summary>
    /// Finish the current level
    /// Show the moves it took to complete
    /// And the prompt for loading the next level
    /// </summary>
    public void CompleteLevel()
    {
        AnalyticsManager.Instance.RegisterCustomEventLevelComplete(moveCount, levelTime);

        DOTween.KillAll();

        StopAllCoroutines();

        SetState(GameState.LevelComplete);

        OnLevelComplete.Invoke();

        levelTime = 0.0f;
    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    public void LoadNextLevel()
    {
        GameObject currentLevel = FindObjectOfType<RoomController>().gameObject;

        // Get the current level's string and load the next level based on hierarchy
        string nextLevelName = Utils.FindStringAndIncrementNumber(currentLevel.name);

        string levelNumber = Utils.FindAndIncrementNumberInString(currentLevel.name);

        // Find the level prefab by loading the resources directly
        Object nextLevelPrefab = Resources.Load(nextLevelName);

        StartCoroutine(DestroyCurrentLevelAndInstantiateNextLevelPrefab(currentLevel, nextLevelPrefab, nextLevelName));

        // If there's a next level, create it
        if (nextLevelPrefab)
        {

            int levelID = 0;
            if (System.Int32.TryParse(levelNumber, out levelID))
            {
                SetLevelID(levelID);
            }
            else
            {
                Debug.Log(">>>> ERROR - Could not set level ID");
            }

            FindObjectOfType<NextLevelAnimator>().ChangeLevelText(levelNumber);

        }
        // Otherwise, go back to the menu
        else
        {
            FindObjectOfType<NextLevelAnimator>().ChangeLevelText("<size=100>More levels to come!");
        }
    }

    private IEnumerator DestroyCurrentLevelAndInstantiateNextLevelPrefab(GameObject currentLevel, Object nextLevelPrefab, string nextLevelName)
    {
        currentLevel.transform.DOScale(1.25f, 1.25f).SetEase(Ease.InOutBack);
        yield return new WaitForSeconds(1.5f);

        // Rotate by 360 degrees
        currentLevel.transform.DOLocalRotate(Vector3.forward * 360, 1.5f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(1.45f);

        // Fade out and destroy current room
        currentLevel.transform.DOScale(0, 1).SetEase(Ease.OutExpo);
        yield return new WaitForSeconds(1);
        Destroy(currentLevel);

        if (nextLevelPrefab != null)
        {

            // Create the new room and fade it to 0
            GameObject nextLevel = (GameObject) Instantiate(nextLevelPrefab, Vector3.zero, Quaternion.identity);
            nextLevel.name = nextLevelName;
            nextLevel.transform.localScale = Vector3.zero;

            yield return new WaitForSeconds(3f);

            // Scale it up and rotate it 360 degrees clockwise
            nextLevel.transform.DOScale(Vector3.one, 3f).SetEase(Ease.InOutSine);
            nextLevel.transform.DOLocalRotate(Vector3.forward * -360, 3f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine);

            yield return new WaitForSeconds(3f);

            // Reset the camera's position and reset the total movement position
            CameraController.Instance.ResetPosition();
            CanvasManager.Instance.ResetTotalMovesPanelPosition();

            StartLevel();

            SetState(GameState.Play);

        }
        else
        {
            yield return new WaitForSeconds(4.5f);
            SceneManager.LoadScene("Level Selection");
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void IncrementMoveCount()
    {
        moveCount++;

        CanvasManager.Instance.totalMovesText.text = "Moves: " + moveCount.ToString();

        if (moveCount == 1)
        {
            if (DEBUG)
            {
                Utils.Fade(CanvasManager.Instance.TotalMovesPanelTransparency, true, 1);
            }
        }

        if (moveCount % 2 == 0)
        {
            AudioManager.Instance.Play("Move_L");
        }
        else
        {
            AudioManager.Instance.Play("Move_R");
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void SetLevelID(int levelID)
    {
        Debug.Log("Set level ID: " + levelID);
        this.levelID = levelID;
    }

}