using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private RectTransform m_MainPanel;

	[SerializeField] private Button m_ButtonPlay;
	[SerializeField] private Button m_ButtonSettings;
	[SerializeField] private Button m_ButtonAbout;

	// Use this for initialization
	void OnEnable ()
	{
		m_ButtonPlay.onClick.AddListener (LoadLevelSelect);
	}

	private void LoadLevelSelect ()
	{
		// TODO: Remove this when development is done
		GameManager.LoadScene (Debug.isDebugBuild ? SceneName.GAME : SceneName.LEVEL_SELECT);
	}
}