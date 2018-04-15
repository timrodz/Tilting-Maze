using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DirectionalFloating : EffectBase
{
	[SerializeField]
	public Direction direction = Direction.Both;

	[SerializeField]
	private Vector2 MoveOffset = Vector2.zero;

	public override void PlaySequence ()
	{
		m_Sequence.Append(m_Rect.DOAnchorPos (m_OriginalPosition + MoveOffset, m_Settings.duration).SetEase (m_Settings.ease).SetLoops (m_Settings.loop ? -1 : 0, m_Settings.loopType));
	}

	void OnValidate ()
	{
		switch (direction)
		{
			case Direction.Horizontal:
				MoveOffset.y = 0;
				break;
			case Direction.Vertical:
				MoveOffset.x = 0;
				break;
		}
	}
}