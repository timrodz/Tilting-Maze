using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Text winText;
	public Button winButton;

	[HideInInspector]
	public static int turns;
	
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

	// Function PauseGame
	// This will enable the paused state for the game

	public void PauseGame() {

		bIsPaused = !bIsPaused;

	}

	public bool IsPaused() {
		return (bIsPaused == true);
	}

	public void CompleteLevel() {

		winText.text += turns;
		winText.gameObject.SetActive(!winText.gameObject.activeInHierarchy);
		winButton.gameObject.SetActive(!winButton.gameObject.activeInHierarchy);

	}

	// Happens whenever the player presses the winning button
	public void BackToMenu() {

		// Reset the turn count
		turns = 0;

		// Find and destroy the level instance
		Transform level = GameObject.Find("PLAYER").GetComponent<Transform>().parent;
		Destroy(level.gameObject);

	}

}