using UnityEngine;

[System.Serializable]
public class MovableBarrier
{
    [SerializeField] private GameObject m_GameObject;
    [SerializeField] private Direction m_MovementDirection;
    [RangeAttribute(1, 12)] [SerializeField] private int m_MovementDistance = 1;
    [SerializeField] private bool m_ShouldDeleteFromList = false;
    [SerializeField] private AnimationSettings m_AnimationSettings;
    
    [HideInInspector][SerializeField] private Vector3 m_OriginalPosition;
    [HideInInspector][SerializeField] private Vector3 m_FinalPosition;
    [HideInInspector][SerializeField] private Vector3 m_FinalDirection;
    [HideInInspector][SerializeField] private bool m_HasMoved;

    public MovableBarrier() { }

    public MovableBarrier(GameObject gameObject, Direction movementDirection, int movementDistance)
    {
        this.m_GameObject = gameObject;
        this.m_MovementDirection = movementDirection;
        this.m_MovementDistance = movementDistance;
        this.m_OriginalPosition = gameObject.transform.position;
    }

    public void Setup()
    {
        m_FinalDirection = Transform.TransformDirection(MovementDirection * MovementDistance);
        
        OriginalPosition = Transform.position;
        
        FinalPosition = OriginalPosition + FinalDirection;
    }

    /// <summary>
    /// BASIC METHODS
    /// </summary>

    public GameObject GameObject
    {
        get
        {
            return (m_GameObject);
        }
        set
        {
            m_GameObject = value;
        }
    }

    public Transform Transform
    {
        get
        {
            return (m_GameObject.transform);
        }
    }
    
    public Vector3 MovementDirection
    {
        get
        {
            return (VectorDirection.DetermineDirection(m_MovementDirection));
        }
    }
    
    public int MovementDistance
    {
        get
        {
            return (m_MovementDistance);
        }
    }

    public AnimationSettings AnimationSettings
    {
        get
        {
            return (m_AnimationSettings);
        }
        set
        {
            m_AnimationSettings = value;
        }
    }

    public Vector3 OriginalPosition
    {
        get
        {
            return (m_OriginalPosition);
        }
        set
        {
            m_OriginalPosition = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return (m_GameObject.transform.position);
        }
        set
        {
            m_GameObject.transform.position = value;
        }
    }

    public Vector3 Scale
    {
        get
        {
            return (m_GameObject.transform.localScale);
        }
        set
        {
            m_GameObject.transform.localScale = value;
        }
    }

    public bool HasMoved
    {
        get
        {
            return (m_HasMoved);
        }
        set
        {
            m_HasMoved = value;
        }
    }

    public bool ShouldDeleteFromList
    {
        get
        {
            return (m_ShouldDeleteFromList);
        }
        set
        {
            m_ShouldDeleteFromList = value;
        }
    }

    /// <summary>
    /// FINAL POSITIONS
    /// </summary>
    public Vector3 FinalPosition
    {
        get
        {
            return (m_FinalPosition);
        }
        set
        {
            m_FinalPosition = value;
        }
    }

    public Vector3 FinalDirection
    {
        get
        {
            return (m_FinalDirection);
        }
        set
        {
            m_FinalDirection = value;
        }
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        if (m_MovementDistance < 1)
        {
            m_MovementDistance = 1;
        }
    }

}