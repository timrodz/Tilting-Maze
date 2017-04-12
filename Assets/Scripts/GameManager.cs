#define MORELEVELS
using System.Collections;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private Transform mainCameraTransform;
    private RoomController roomController;

    [HeaderAttribute("UI Elements")]
    public TextMeshProUGUI totalMovesText;
    public Button nextLevelButton;
    public Image pausePanel;

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
    [HideInInspector] public bool canShowWinScreen = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        mainCameraTransform = Camera.main.transform;
        roomController = FindObjectOfType<RoomController> ();

    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {

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

        totalMovesText.text += moveCount.ToString();

        totalMovesText.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(true);

        StartCoroutine(AnimateLevelCompletion());

    }

    private IEnumerator AnimateLevelCompletion() {

        mainCameraTransform.DOMove(winningAnimationCameraPosition, winningAnimationDuration).SetEase(winningAnimationEaseType);

        roomController.playerObject.transform.DOMove(winningAnimationPlayerPosition, winningAnimationDuration).SetEase(winningAnimationEaseType);

        yield return new WaitForSeconds(winningAnimationDuration);

    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    public void LoadNextLevel() {

        // Remove the appended movecount string from the winning text
        totalMovesText.text = totalMovesText.text.Remove(totalMovesText.text.IndexOf(' ') + 1, totalMovesText.text.Length - totalMovesText.text.IndexOf(' ') - 1);

        // Disable the score text and the next level buttons
        totalMovesText.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);

        // Reset the move count back to 0
        moveCount = 0;

        // Load the next level //

        Transform level = GameObject.Find("PLAYER").GetComponent<Transform> ().parent;

        // Get the current level's string and load the next level based on hierarchy
        string str = TrimString(level.ToString(), true);

        // Destroy the current level
        Destroy(level.gameObject);

        // Find the level prefab by loading the resources directly
        Object nextLevel = Resources.Load(str);

        // Reset the camera's position
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera> ();
        camera.transform.position = new Vector3(0, 0, -10);

        // If there's a next level, create it
        if (nextLevel) {

            // Instantiate the next level
            // The quaternion uses 1 on the y-axis because it needs to be rotated 90 degrees around it
            GameObject levelToInstantiate = Instantiate(nextLevel, Vector3.zero, new Quaternion(0, 1, 0, 1)) as GameObject;

            // Get rid of the "(CLONE)" text
            levelToInstantiate.name = TrimString(nextLevel.ToString(), false);

        }
        // Otherwise, go back to the menu
        else {

            SceneManager.LoadScene("Level Selection");

        }

    }

    /// <summary>
    /// Trims the string.
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="_stringToTrim">String to trim.</param>
    /// <param name="_containsNumber">If set to <c>true</c> contains number.</param>
    private string TrimString(string _stringToTrim, bool _containsNumber) {

        int length = _stringToTrim.Length - 1;

        // Find the first parentheses and delete everything
        // that follows after it
        int index = _stringToTrim.IndexOf('(');
        _stringToTrim = _stringToTrim.Remove(index - 1, length - index + 2);

#if (MORELEVELS)

        // TODO: Modify this section so it takes numbers higher than 9

        if (_containsNumber) {

            // Recalculate the new length
            length = _stringToTrim.Length - 1;

            // store the number it has and remove it
            string number = Regex.Match(_stringToTrim, @"\d+").Value;

            // parse the number from the string and add 1 to it
            int num = int.Parse(number) + 1;

            // TODO: Count the amount of digits that the number has

            // Remove the number at the top
            _stringToTrim = _stringToTrim.Remove(length);

            _stringToTrim += (num);

        }

#endif

        return _stringToTrim;

    }

}