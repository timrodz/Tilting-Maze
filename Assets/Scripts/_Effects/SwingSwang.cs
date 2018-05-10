using DG.Tweening;
using UnityEngine;

public class SwingSwang : EffectBase
{
	[SerializeField] private float m_RotationOffset = 5.0f;

	[SerializeField] private RotateMode m_RotationMode = RotateMode.FastBeyond360;

	public override void PlaySequence ()
	{

		m_Sequence.Append (m_Rect.DORotate (transform.eulerAngles + (Vector3.forward * m_RotationOffset), m_Settings.duration, m_RotationMode).SetEase (m_Settings.ease).SetLoops(m_Settings.loop ? -1 : 0, m_Settings.loopType));
	}
}