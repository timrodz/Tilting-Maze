using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    [Header ("Controller2D Variables")]
    [SerializeField] protected LayerMask m_CollisionMask;

    [SerializeField] private const float SKIN_WIDTH = 0.015f;
    [Range (2, 8)]
    [SerializeField] private int m_HorizontalRayCount = 4;
    [Range (2, 8)]
    [SerializeField] private int m_VerticalRayCount = 4;

    [SerializeField] private float m_HorizontalRaySpacing;
    [SerializeField] private float m_VerticalRaySpacing;

    [SerializeField] private BoxCollider2D m_BoxCollider2D;
    [SerializeField] private RaycastOrigins m_RaycastOrigins;
    [SerializeField] public CollisionInfo m_CollisionInfo;

    [SerializeField] protected bool m_CanMove = true;
    [SerializeField] protected bool m_IsMoving = false;

    // Use this for initialization
    void Start ()
    {
        m_BoxCollider2D = GetComponent<BoxCollider2D> ();
        CalculateRaySpacing ();
    }

    public void Move (Vector3 velocity)
    {
        if (!CanMove) { return; }

        UpdateRaycastOrigins ();

        m_CollisionInfo.Reset ();

        if (velocity.x != 0)
        {
            HorizontalCollisions (ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions (ref velocity);
        }

        transform.Translate (velocity, Space.World);
    }

    /// <summary>
    /// Check for vertical collisions
    /// </summary>
    /// <param name="velocity"></param>
    private void VerticalCollisions (ref Vector3 velocity)
    {
        float directionY = Mathf.Sign (velocity.y);
        float rayLength = Mathf.Abs (velocity.y) + SKIN_WIDTH;

        for (int i = 0; i < m_VerticalRayCount; i++)
        {
            // Depending on the direction we're moving, check which direction to check
            Vector2 rayOrigin = (directionY == -1) ? m_RaycastOrigins.botLeft : m_RaycastOrigins.topLeft;

            rayOrigin += Vector2.right * (m_VerticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, m_CollisionMask);

            Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance;

                m_CollisionInfo.below = (directionY == -1);
                m_CollisionInfo.above = (directionY == 1);
            }

        }

    }

    /// <summary>
    /// Check for horizontal collisions
    /// </summary>
    /// <param name="velocity"></param>
    private void HorizontalCollisions (ref Vector3 velocity)
    {
        float directionX = Mathf.Sign (velocity.x);
        float rayLength = Mathf.Abs (velocity.x) + SKIN_WIDTH;

        for (int i = 0; i < m_HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? m_RaycastOrigins.botLeft : m_RaycastOrigins.botRight;

            rayOrigin += Vector2.up * (m_HorizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, m_CollisionMask);

            Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - SKIN_WIDTH) * directionX;
                rayLength = hit.distance;

                m_CollisionInfo.left = (directionX == -1);
                m_CollisionInfo.right = (directionX == 1);
            }
        }
    }

    /// <summary>
    /// Update the raycast origins (So they stick to the transform's position)
    /// </summary>
    void UpdateRaycastOrigins ()
    {
        // Get the bounds of the current collider
        Bounds bounds = m_BoxCollider2D.bounds;

        // Shrink the collider by 1 pixel on each side
        bounds.Expand (SKIN_WIDTH * -2);

        // Update the new raycast origins
        m_RaycastOrigins.botLeft = new Vector2 (bounds.min.x, bounds.min.y);
        m_RaycastOrigins.botRight = new Vector2 (bounds.max.x, bounds.min.y);
        m_RaycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
        m_RaycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
    }

    /// <summary>
    /// Calculates the spacing between the rays
    /// </summary>
    void CalculateRaySpacing ()
    {
        // Get the bounds of the current collider
        Bounds bounds = m_BoxCollider2D.bounds;

        // Shrink the collider by 1 pixel on each side
        bounds.Expand (SKIN_WIDTH * -2);

        // Make sure we have at least 2 rays
        m_HorizontalRayCount = Mathf.Clamp (m_HorizontalRayCount, 2, int.MaxValue);
        m_VerticalRayCount = Mathf.Clamp (m_VerticalRayCount, 2, int.MaxValue);

        m_HorizontalRaySpacing = bounds.size.y / (m_HorizontalRayCount - 1);
        m_VerticalRaySpacing = bounds.size.x / (m_VerticalRayCount - 1);
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate ()
    {
        m_BoxCollider2D = GetComponent<BoxCollider2D> ();
        CalculateRaySpacing ();
    }

    public bool CanMove
    {
        get { return m_CanMove; }
        set { m_CanMove = value; }
    }

    public bool IsMoving
    {
        get { return m_IsMoving; }
        set { m_IsMoving = value; }
    }

    [System.Serializable]
    private struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 botLeft, botRight;
    }

    [System.Serializable]
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public void Reset ()
        {
            above = below = left = right = false;
        }
    }

}