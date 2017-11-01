using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToRotate : MonoBehaviour
{
	[SerializeField] private Camera m_Camera;
    private float m_BaseAngle = 0.0f;
	private bool m_IsRotating;

	public bool IsRotating
	{
		get { return (m_IsRotating); }
		set {}
	}

    void OnMouseDown()
    {
        Vector3 pos = m_Camera.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        m_BaseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        m_BaseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
    }

    void OnMouseDrag()
    {
        Vector3 pos = m_Camera.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;
        transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
    }

}