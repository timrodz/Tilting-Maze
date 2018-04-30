using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOptionsMenu : MonoBehaviour
{
	[SerializeField] private Button m_PauseButton;
	[SerializeField] private bool m_IsPaused = false;

	[Header("Options Menu")]
	[SerializeField] private CanvasGroup m_CanvasGroup;
	[SerializeField] private Button m_ButtonBackground;
	[SerializeField] private Image m_ImageOptions;
	[SerializeField] private Button m_ButtonOptionYes;
	[SerializeField] private Button m_ButtonOptionNo;
	
	private Sequence m_Sequence;
	
	void Start()
	{
		Utils.Fade(m_CanvasGroup, false, 0);
	}

	void OnEnable()
	{
		m_PauseButton.onClick.AddListener(TogglePause);
		m_ButtonBackground.onClick.AddListener(TogglePause);
		m_ButtonOptionNo.onClick.AddListener(TogglePause);
		m_ButtonOptionYes.onClick.AddListener(HeadBackToMenu);
	}

	public void TogglePause()
	{
		m_IsPaused = !m_IsPaused;
		Debug.LogFormat("Pause state: {0}", m_IsPaused);
		if (m_IsPaused)
		{
			Pause();
		}
		else
		{
			GameManager.SetState(GameManager.GetLastState());
			Utils.Fade(m_CanvasGroup, false, 0.35f);
		}
		
	}

	private void Pause()
	{
		GameManager.SetState(GameState.Paused);
		Utils.Fade(m_CanvasGroup, true, 0.35f);
	}
	
	private void HeadBackToMenu()
	{
		GameManager.LoadScene(SceneNames.MAIN_MENU);
	}
}