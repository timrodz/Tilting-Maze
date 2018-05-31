using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsMenu : MonoBehaviour
{
	[SerializeField] private Button m_PauseButton;
	[SerializeField] private bool m_IsPaused = false;

	[Header ("Options Menu")]
	[SerializeField] private CanvasGroup m_CanvasGroup;
	[SerializeField] private Button m_ButtonBackground;
	[SerializeField] private Image m_ImageOptions;
	[SerializeField] private Button m_ButtonOptionYes;
	[SerializeField] private Button m_ButtonOptionNo;

	[Header("Accesibility menu")]
	[SerializeField] private Button m_ButtonOptionAccessibility;
	[SerializeField] private CanvasGroup m_AccessibilityCanvasGroup;

	private Sequence m_Sequence;

	void Start ()
	{
		Utils.Fade (m_CanvasGroup, false, 0);

		Utils.Fade (m_AccessibilityCanvasGroup, Utils.ACCESSIBILITY_ON ? true : false, 0);
	}

	void OnEnable ()
	{
		m_PauseButton.onClick.AddListener (TogglePause);
		m_ButtonBackground.onClick.AddListener (TogglePause);
		m_ButtonOptionNo.onClick.AddListener (TogglePause);
		m_ButtonOptionYes.onClick.AddListener (HeadBackToMenu);
		m_ButtonOptionAccessibility.onClick.AddListener(ToggleAccessibility);
	}

	public void TogglePause ()
	{
		m_IsPaused = !m_IsPaused;
		Print.LogFormat ("Pause state: {0}", m_IsPaused);

		if (m_IsPaused)
		{
			Pause ();
		}
		else
		{
			GameManager.SetState (GameManager.Instance.LastState);
			Utils.Fade (m_CanvasGroup, false, 0.35f);
		}

	}

	private void Pause ()
	{
		GameManager.SetState (GameState.Paused);
		Utils.Fade (m_CanvasGroup, true, 0.35f);
	}

	private void HeadBackToMenu ()
	{
		GameManager.LoadScene (SceneName.MAIN_MENU);
	}

	private void ToggleAccessibility()
	{
		// TODO: Change to PlayerPrefs
		Utils.ACCESSIBILITY_ON = !Utils.ACCESSIBILITY_ON;

		if (Utils.ACCESSIBILITY_ON)
		{
			Utils.Fade (m_AccessibilityCanvasGroup, true, 0.35f);
		}
		else
		{
			Utils.Fade (m_AccessibilityCanvasGroup, false, 0.35f);
		}

		TogglePause();
	}
}