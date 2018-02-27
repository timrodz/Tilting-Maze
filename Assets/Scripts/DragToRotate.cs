using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToRotate : MonoBehaviour
{
	[SerializeField] private Camera m_Camera;
    private float m_BaseAngle = 0.0f;
	private bool m_IsRotating;

    void OnMouseDown()
    {
        Vector3 pos = m_Camera.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        m_BaseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        Debug.LogFormat("Initial Base Angle: {0} - Position: {1}", m_BaseAngle, pos);
        m_BaseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
        Debug.LogFormat("Initial Base Angle: {0}", m_BaseAngle);
        
        Game_Events.Instance.Event_ToggleDragging(true);
    }
    
    void OnMouseUp()
    {
        Game_Events.Instance.Event_ToggleDragging(false);
        
        Vector3 pos = m_Camera.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;
        transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
        
        Debug.LogFormat("Angle: {0} - Difference: {1}", ang, (float)(m_BaseAngle - ang));
    }

    void OnMouseDrag()
    {
        Vector3 pos = m_Camera.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;
        transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
        
        // Debug.LogFormat("Angle: {0} - Difference: {1}", ang, (float)(m_BaseAngle - ang));
    }

}