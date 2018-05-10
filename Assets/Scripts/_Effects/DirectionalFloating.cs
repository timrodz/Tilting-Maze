using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DirectionalFloating : EffectBase
{
	[SerializeField] public Direction m_Direction = Direction.Both;

	[SerializeField] private Vector2 m_MoveOffset = Vector2.zero;

	public override void PlaySequence ()
	{
		m_Sequence.Append(m_Rect.DOAnchorPos (m_OriginalPosition + m_MoveOffset, m_Settings.duration).SetEase (m_Settings.ease).SetLoops (m_Settings.loop ? -1 : 0, m_Settings.loopType));
	}

	void OnValidate ()
	{
		switch (m_Direction)
		{
			case Direction.Horizontal:
				m_MoveOffset.y = 0;
				break;
			case Direction.Vertical:
				m_MoveOffset.x = 0;
				break;
		}
	}
}