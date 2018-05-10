using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    // -- Public variables
    public int LevelID
    {
        get { return m_LevelID; }
    }

    public GameState State
    {
        get { return m_State; }
    }

    public GameState LastState
    {
        get { return m_LastState; }
    }

    public bool IsLevelComplete
    {
        get { return m_IsLevelComplete; }
    }

    // -- Private variables
    // Game state
    [SerializeField] private int m_LevelID = 0;
    private float m_ElapsedLevelTime = 0.0f;

    [SerializeField] private GameState m_State = GameState.LoadingLevel;
    [SerializeField] private GameState m_LastState = GameState.LoadingLevel;
    [SerializeField] private static int m_MoveCount;

    [SerializeField] private bool m_IsLevelComplete = false;

    // -------------------------------------------------------------------------------------------

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start ()
    {
        InitializeLevelVariables ();
    }

    public void InitializeLevelVariables ()
    {
        m_MoveCount = 0;
        m_IsLevelComplete = false;
    }

    void LateUpdate ()
    {
        if (State == GameState.Play)
        {
            m_ElapsedLevelTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void TogglePause ()
    {
        Print.Log ("Toggling Pause");
        // Pause the game if it's not
        if (State != GameState.Paused)
        {
            SetState (GameState.Paused);
        }
        // Unpause the game
        else
        {
            SetState (m_LastState);
        }
    }

    /// <summary>
    /// Finish the current level
    /// Show the moves it took to complete
    /// And the prompt for loading the next level
    /// </summary>
    public static void CompleteLevel ()
    {
        // AnalyticsManager.Instance.RegisterCustomEventLevelComplete(m_MoveCount, m_ElapsedLevelTime);

        DOTween.KillAll ();

        GameManager.Instance.StopAllCoroutines ();

        GameManager.Instance.m_ElapsedLevelTime = 0.0f;

        SetState (GameState.LevelComplete);

        AudioManager.PlayEffect (ClipType.Trigger_Button);

        GameManager.Instance.LoadNextLevel ();
    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    public void LoadNextLevel ()
    {
        GameObject currentLevel = FindObjectOfType<LevelController> ().gameObject;

        // Get the current level's string and load the next level based on hierarchy
        string nextLevelName = Utils.FindStringAndIncrementNumber (currentLevel.name);

        string stringLevelNumber = Utils.FindStringAndReturnIncrementedNumber (currentLevel.name);

        // Find the level prefab by loading the resources directly
        Object nextLevelPrefab = Resources.Load (nextLevelName);

        // If there's a next level, create it
        if (nextLevelPrefab)
        {
            int nextLevelID = -1;
            if (System.Int32.TryParse (stringLevelNumber, out nextLevelID))
            {
                m_LevelID = nextLevelID;
                StartCoroutine (DestroyCurrentLevelAndInstantiateNextLevelPrefab (currentLevel, nextLevelPrefab, nextLevelName));
            }
            else
            {
                Print.LogError (">>>> ERROR - Could not set level ID");
                GameManager.LoadScene(SceneName.MAIN_MENU);
            }
        }
        // Otherwise, go back to the menu
        else
        {
            FindObjectOfType<NextLevelAnimator> ().ChangeLevelText ("<size=100>More levels to come!");
        }

        GameEvents.Instance.Event_LevelComplete (LevelID);
    }

    private IEnumerator DestroyCurrentLevelAndInstantiateNextLevelPrefab (GameObject currentLevel, Object nextLevelPrefab, string nextLevelName)
    {
        // Zoom out
        currentLevel.transform.DOScale (1.25f, 1.25f).SetEase (Ease.InOutBack);
        yield return new WaitForSeconds (1.5f);

        // Rotate by 360 degrees
        currentLevel.transform.DOLocalRotate (Vector3.forward * 360, 1.5f, RotateMode.LocalAxisAdd).SetEase (Ease.InOutBack);

        yield return new WaitForSeconds (1.45f);

        // Fade out and destroy current level
        currentLevel.transform.DOScale (0, 1).SetEase (Ease.OutExpo);
        
        yield return new WaitForSeconds (1);

        Destroy (currentLevel);

        if (nextLevelPrefab != null)
        {
            // Create the new room and fade it to 0
            GameObject nextLevel = (GameObject) Instantiate (nextLevelPrefab, new Vector3 (0, 0, 1000), Quaternion.identity);
            nextLevel.name = nextLevelName;
            // nextLevel.transform.localScale = Vector3.zero;

            yield return new WaitForSeconds (3f);

            // Scale it up and rotate it 360 degrees clockwise
            nextLevel.transform.DOLocalMoveZ (0, 3f).SetEase (Ease.InOutSine);
            // nextLevel.transform.DOScale(Vector3.one, 3f).SetEase(Ease.InOutSine);
            nextLevel.transform.DOLocalRotate (Vector3.forward * -360, 3f, RotateMode.LocalAxisAdd).SetEase (Ease.InOutSine);

            yield return new WaitForSeconds (3f);

            GameEvents.Instance.Event_LevelLoaded();

            InitializeLevelVariables ();

            SetState(GameState.Play);
        }
        else
        {
            Print.LogError(">>>> nextLevelPrefab is NULL");
            yield return new WaitForSeconds (4.5f);
            LoadScene (SceneName.LEVEL_SELECT);
        }

    }

    public void IncrementMoveCount ()
    {
        m_MoveCount++;

        if (m_MoveCount % 2 == 0)
        {
            AudioManager.PlayEffect (ClipType.Move_L);
        }
        else
        {
            AudioManager.PlayEffect (ClipType.Move_R);
        }
    }

    // -- Static metohds

    /// <summary>
    /// Loads a new scene
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_mode"></param>
    public static void LoadScene (SceneName _name = SceneName.MAIN_MENU, LoadSceneMode _mode = LoadSceneMode.Single)
    {
        SetState (GameState.LoadingLevel);
        SceneManager.LoadScene ((int) _name, LoadSceneMode.Single);
    }

    /// <summary>
    /// Sets the current game state to a variable, and stores the last state as the current
    /// </summary>
    /// <param name="state"></param>
    public static void SetState (GameState state)
    {
        Print.LogFormat(">>>> Set State to {0}", state);
        GameManager.Instance.m_LastState = GameManager.Instance.m_State;
        GameManager.Instance.m_State = state;
    }
}

#if UNITY_EDITOR
[CustomEditor (typeof (GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        if (DrawDefaultInspector ())
        {

        }

        if (GUILayout.Button ("Complete Level"))
        {
            GameManager.CompleteLevel ();
        }

        if (GUILayout.Button ("Load level-1"))
        {
            Object nextLevelPrefab = Resources.Load ("level-1");
            GameObject nextLevel = (GameObject) Instantiate (nextLevelPrefab, Vector3.zero, Quaternion.identity);
            nextLevel.name = nextLevelPrefab.name;
        }
    }
}
#endif