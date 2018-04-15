using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

public class SubtitleTextDisplay : MonoBehaviour
{
	[SerializeField]
	private TMPro.TextMeshProUGUI m_TextAbove;

	[SerializeField]
	private TMPro.TextMeshProUGUI m_TextBelow;

	[SerializeField]
	private SubtitleTextOptions m_Options;

	private float m_TimeElapsed = 0.0f;
	private bool m_CanUpdateTimeElapsed = false;
	private bool m_CanHideSubtitles = false;
	private bool m_HasClicked = false;

	void Awake()
	{
		m_TextAbove.text = m_TextBelow.text = "";
	}

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		Game_Events.Instance.DisplaySubtitles += DisplaySubtitles;
		LeanTouch.OnFingerDown += OnFingerDown;
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (m_CanUpdateTimeElapsed)
		{
			m_TimeElapsed += Time.deltaTime;

			if (m_TimeElapsed >= m_Options.Duration + m_Options.DelayAbove + m_Options.DelayBelow)
			{
				m_TimeElapsed = 0.0f;
				m_CanUpdateTimeElapsed = false;
				m_CanHideSubtitles = true;

				if (m_HasClicked)
				{
					Hide();
					m_HasClicked = false;
				}
			}
		}
	}

	public void DisplaySubtitles(SubtitleTextOptions _options)
	{
		m_Options = _options;
		Debug.LogFormat("SUBTITLES: {0}-{1}", m_Options.TextAbove, m_Options.TextBelow);

		// Displaying subtitles above
		if (m_Options.TextAbove.Length > 0)
		{
			ShowAbove();
		}
		// Display subtitles below
		if (m_Options.TextBelow.Length > 0)
		{
			ShowBelow();
		}

		m_CanUpdateTimeElapsed = true;
		m_TimeElapsed = 0.0f;
	}

	private void ShowAbove()
	{
		m_TextAbove.text = m_Options.TextAbove;
		m_TextAbove.DOFade(0, 0.5f).From().SetDelay(m_Options.DelayAbove);
	}

	private void ShowBelow()
	{
		m_TextBelow.text = m_Options.TextBelow;
		m_TextBelow.DOFade(0, 0.5f).From().SetDelay(m_Options.DelayBelow);
	}

	private void OnFingerDown(LeanFinger _finger)
	{
		m_HasClicked = true;
		
		// Hide if finger is put down after 'x' seconds.
		Hide();
	}

	private void Hide()
	{
		// Do not hide subtitles before 'x' amount of seconds have passed.
		if (!m_CanHideSubtitles)
		{
			return;
		}
		m_TextBelow.DOFade(0, 0.5f);
		m_TextAbove.DOFade(0, 0.5f);
		m_CanHideSubtitles = false;
	}
}

[System.Serializable]
public class SubtitleTextOptions
{
	public string TextAbove = "";
	public string TextBelow = "";
	public float Duration = 3.0f;
	public float DelayAbove = 0.0f;
	public float DelayBelow = 0.0f;
}