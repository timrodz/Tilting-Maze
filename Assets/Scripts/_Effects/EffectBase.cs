using System.Collections;
using DG.Tweening;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EffectBase : MonoBehaviour
{
	[Header("Animation Settings")]
	[SerializeField]
	protected AnimationSettings m_Settings;

	protected RectTransform m_Rect;

	protected Vector3 m_OriginalScale;
	protected Vector2 m_OriginalPosition;

	protected Sequence m_Sequence;

	protected virtual void Awake()
	{
		m_Rect = GetComponent<RectTransform>();
		m_OriginalPosition = (Vector2) m_Rect.anchoredPosition;
		m_OriginalScale = m_Rect.localScale;
	}

	protected virtual void Start()
	{
		if (m_Settings.animateOnStart)
		{
			ResetSequence();
			PlaySequence();
		}
	}

	public void ResetSequence()
	{
		if (null != m_Sequence)
		{
			if (m_Settings.loop)
			{
				m_Sequence.SetLoops(0);
			}
			m_Sequence.Complete();
			m_Sequence.Kill();
		}
	}

	public virtual void PlaySequence() { }

	private void Update() { }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EffectBase), true)]
public class EffectBaseEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EffectBase effect = (EffectBase) target;

		if (GUILayout.Button("Play"))
		{
			effect.ResetSequence();
			effect.PlaySequence();
		}
		if (GUILayout.Button("Stop"))
		{
			effect.ResetSequence();
		}
	}
}
#endif