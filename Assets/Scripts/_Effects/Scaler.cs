using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Scaler : EffectBase
{
	[SerializeField]
	private Vector2 ScaleOffset = Vector2.one;

	public override void PlaySequence ()
	{
		m_Sequence.Append(m_Rect.DOScale (m_OriginalScale + (Vector3)ScaleOffset, m_Settings.duration).SetEase (m_Settings.ease).SetLoops (m_Settings.loop ? -1 : 0, m_Settings.loopType));
	}
}