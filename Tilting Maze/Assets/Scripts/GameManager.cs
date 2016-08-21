#define MORELEVELS

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public Text scoreText;
	public Button nextLevelButton;

	[HideInInspector]
	public static int moveCount;

	private bool bIsPaused = false;

	public Transform pauseImage;

	// Update is called once per frame
	void Update() {

		if (bIsPaused) {

			if (!pauseImage.gameObject.activeInHierarchy) {
				pauseImage.gameObject.SetActive(true);
			}

		}
		else {

			if (pauseImage.gameObject.activeInHierarchy) {
				pauseImage.gameObject.SetActive(false);
			}

		}

	}
	
	/// Function PauseGame
	// This will enable the paused state for the game

	public void PauseGame() {
		bIsPaused = !bIsPaused;
	}

	/// Function IsPaused
	// Return whether or not the game's currently paused

	public bool IsPaused() {
		return (bIsPaused == true);
	}

	/// Function CompleteLevel
	// Finish the current level
	// Show the moves it took to complete
	// And the prompt for loading the next level

	public void CompleteLevel() {

		scoreText.text += moveCount;
		scoreText.gameObject.SetActive(true);
		nextLevelButton.gameObject.SetActive(true);

	}

	/// Function LoadNextLevel
	// Happens whenever the player presses the winning button

	public void LoadNextLevel() {

		// Remove the appended movecount string from the winning text
		scoreText.text = scoreText.text.Remove(scoreText.text.IndexOf(' ') + 1, scoreText.text.Length - scoreText.text.IndexOf(' ') - 1);

		// Disable the score text and the next level buttons
		scoreText.gameObject.SetActive(false);
		nextLevelButton.gameObject.SetActive(false);

		// Reset the move count back to 0
		moveCount = 0;

		// Load the next level //

		Transform level = GameObject.Find("PLAYER").GetComponent<Transform>().parent;

		// Get the current level's string and load the next level based on hierarchy
		string str = TrimString(level.ToString(), true);

		// Destroy the current level
		Destroy(level.gameObject);

		// Find the level prefab by loading the resources directly
		Object nextLevel = Resources.Load(str);

		// Reset the camera's position
		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
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

	/// Function TrimString
	// param _stringToTrim : the string that's going to be trimmed
	// param _containsNumber : whether or not it has a number that must be changed

	string TrimString(string _stringToTrim, bool _containsNumber) {

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

			// Remove the number at the top
			_stringToTrim = _stringToTrim.Remove(length);

			_stringToTrim += (num);

		}

#endif

		return _stringToTrim;

	}

}