using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExplosionEffect : EffectBase
{
	[Header("Visual")]
	[SerializeField] private Image m_SquareImage;
	[SerializeField] private Image m_SquareLinesImage;

	public void CreateEffect(Vector2 _position)
	{
		m_Rect.anchoredPosition = _position;
		ResetSequence();
		PlaySequence();
	}

	public override void PlaySequence()
	{
		Print.LogFormat("Position: {0}", m_Rect.anchoredPosition);
		float dur = m_Settings.duration / 2.0f;

		// -- Animate the square
		// Reset the values
		m_SquareImage.rectTransform.localScale = m_OriginalScale;
		m_SquareImage.rectTransform.localEulerAngles = Vector3.zero;
		m_SquareImage.color = Color.white;

		// Scale, rotate, fade-out
		m_Sequence.Append(m_SquareImage.rectTransform.DOScale(m_OriginalScale * 2, dur));
		m_Sequence.Join(m_SquareImage.rectTransform.DORotate(Vector3.forward * 180, dur, RotateMode.FastBeyond360));
		m_Sequence.Join(m_SquareImage.DOFade(0, dur));

		// Animate the lines
	}
}