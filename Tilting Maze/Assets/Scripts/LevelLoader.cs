using UnityEngine;
using System.Collections;


public class LevelLoader : MonoBehaviour {

	public static int turns;

	// Use this for initialization
	void Start() {
	


	}
		
	// Update is called once per frame
	void Update() {


	
	}

	public void CompleteLevel(Transform _player) {

		// Reset the turn count
		turns = 0;

		// Destroy the level instance
		Transform level = _player.parent;
		Destroy(level.gameObject);

	}

}