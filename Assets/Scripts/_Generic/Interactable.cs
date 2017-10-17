using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BoxCollider2D m_Collider;
    [SerializeField] private Rigidbody2D m_Rigidbody;

    [Header("Variables")]
    [SerializeField] private bool m_IsTrigger = true;
    [SerializeField] private bool m_IsKinematic = false;

    private Vector3 pos;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        pos = transform.position;
        if (null == m_Collider)
        {
            m_Collider = GetComponent<BoxCollider2D>();
        }

        if (null == m_Rigidbody)
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }
        
        InitializePhysics();
    }

    private void InitializePhysics()
    {
        if ((null == m_Collider) || (null == m_Rigidbody))
        {
            return;
        }
        
        // Force the collider to be trigger
        m_Collider.isTrigger = m_IsTrigger;

        m_Rigidbody.isKinematic = m_IsKinematic;
        
        if (m_IsKinematic)
        {
            m_Rigidbody.gravityScale = 0.0f;
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            m_Rigidbody.gravityScale = 1.0f;
            m_Rigidbody.constraints = RigidbodyConstraints2D.None;
        }

        transform.position = pos;
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
    
    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        InitializePhysics();
    }
}