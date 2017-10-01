using UnityEngine;

[RequireComponent(typeof (BoxCollider2D))]
[RequireComponent(typeof (Rigidbody2D))]
public class Interactable : MonoBehaviour
{
	[Header("Components")]
    [SerializeField] private BoxCollider2D m_Collider;
	[SerializeField] private Rigidbody2D m_Rigidbody;
	
	[Header("Variables")]
	[SerializeField] private bool m_IsTrigger = true;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (null == m_Collider)
        {
            m_Collider = GetComponent<BoxCollider2D>();
        }
		
		if (null == m_Rigidbody)
		{
			m_Rigidbody = GetComponent<Rigidbody2D>();
		}
		
		// Force the collider to be trigger
		m_Collider.isTrigger = m_IsTrigger;
		
		m_Rigidbody.isKinematic = true;
		m_Rigidbody.gravityScale = 0;
		m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
		Debug.LogFormat("-> OnTriggerEnter2D: ({0})::({1})", this.gameObject.name, other.gameObject.name);
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
		Debug.LogFormat("-> OnTriggerExit2D: ({0})::({1})", this.gameObject.name, other.gameObject.name);
    }
}