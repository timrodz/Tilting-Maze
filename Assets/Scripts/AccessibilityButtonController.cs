using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessibilityButtonController : MonoBehaviour
{
	[Header ("Buttons")]
	[SerializeField] private Button m_ButtonRotateRight;
	[SerializeField] private Button m_ButtonRotateLeft;

	void OnEnable ()
	{
		m_ButtonRotateRight.onClick.AddListener (() => { Rotate (true); });
		m_ButtonRotateLeft.onClick.AddListener (() => { Rotate (false); });
	}

	private void Rotate (bool _rotateRight)
	{
		GameEvents.Instance.Event_ToggleDragging(false);
		GameEvents.Instance.Event_RotateLevel (_rotateRight);
	}
}