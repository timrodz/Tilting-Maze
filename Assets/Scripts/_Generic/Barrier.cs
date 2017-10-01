using UnityEngine;

[System.Serializable]
public class MovableBarrier
{
    [SerializeField] private GameObject gameObject;
    [SerializeField] private Direction m_MovementDirection;
    [RangeAttribute(1, 12)]
    [SerializeField] private int m_MovementDistance = 1;

    [SerializeField] private Vector3 m_OriginalPosition;
    [SerializeField] private Vector3 m_FinalPosition;

    [HideInInspector] [SerializeField] private bool m_HasMoved;
    [HideInInspector] [SerializeField] private bool m_ShouldDeleteFromList = false;

    public MovableBarrier() { }

    public MovableBarrier(GameObject gameObject, Direction movementDirection, int movementDistance)
    {
        this.gameObject = gameObject;
        this.m_MovementDirection = movementDirection;
        this.m_MovementDistance = movementDistance;
        this.m_OriginalPosition = gameObject.transform.position;
    }

    /// <summary>
    /// BASIC METHODS
    /// </summary>

    public GameObject GameObject()
    {
        return (gameObject);
    }

    public Transform Transform()
    {
        return (gameObject.transform);
    }

    public void SetOriginalPosition(Vector3 _position)
    {
        m_OriginalPosition = _position;
    }

    public Vector3 GetOriginalPosition()
    {
        return (m_OriginalPosition);
    }

    public void SetFinalPosition(Vector3 _position)
    {
        m_FinalPosition = _position;
    }

    public Vector3 GetFinalPosition()
    {
        return (m_FinalPosition);
    }

    public Vector3 GetPosition()
    {
        return (gameObject.transform.position);
    }

    public Vector3 GetScale()
    {
        return (gameObject.transform.localScale);
    }

    public void SetHasMoved(bool _HasMoved)
    {
        m_HasMoved = _HasMoved;
    }

    public bool HasMoved()
    {
        return (m_HasMoved);
    }

    public bool ShouldDeleteFromList()
    {
        return (m_ShouldDeleteFromList);
    }

    /// <summary>
    /// FINAL POSITIONS
    /// </summary>

    public Vector3 GetWorldFinalPosition()
    {
        return (Transform().TransformDirection(m_FinalPosition));
    }

    /// <summary>
    /// FINAL DIRECTIONS
    /// </summary>

    public Vector3 GetFinalDirection()
    {
        return (gameObject.transform.TransformDirection(GetMovementDirection() * m_MovementDistance));
    }

    public Vector3 GetFinalOppositeDirection()
    {
        return (gameObject.transform.TransformDirection(GetOppositeMovementDirection() * m_MovementDistance));
    }

    public Vector3 GetFinalPerpendicularDirection()
    {
        return (gameObject.transform.TransformDirection(GetPerpendicularMovementDirection() * m_MovementDistance));
    }

    /// <summary>
    /// MOVEMENT DIRECTION
    /// </summary>

    private Vector3 GetMovementDirection()
    {
        return (VectorDirection.DetermineDirection(m_MovementDirection));
    }

    private Vector3 GetOppositeMovementDirection()
    {
        return (VectorDirection.DetermineOppositeDirection(m_MovementDirection));
    }

    private Vector3 GetPerpendicularMovementDirection()
    {
        return (VectorDirection.DeterminePerpendicularDirection(m_MovementDirection));
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