/// <summary>
/// Draws arrows for gizmos
/// </summary>

using System.Collections;
using UnityEngine;

public static class DrawArrow
{
    public static void ForGizmo (Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawRay (pos, direction);

        Vector3 right = Quaternion.LookRotation (direction) * Quaternion.Euler (0, 180 + arrowHeadAngle, 0) * new Vector3 (0, 0, 1);
        Vector3 left = Quaternion.LookRotation (direction) * Quaternion.Euler (0, 180 - arrowHeadAngle, 0) * new Vector3 (0, 0, 1);
        Gizmos.DrawRay (pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay (pos + direction, left * arrowHeadLength);
    }

    public static void ForGizmo (Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color;
        Gizmos.DrawRay (pos, direction);

        Vector3 right = Quaternion.LookRotation (direction) * Quaternion.Euler (0, 180 + arrowHeadAngle, 0) * new Vector3 (0, 0, 1);
        Vector3 left = Quaternion.LookRotation (direction) * Quaternion.Euler (0, 180 - arrowHeadAngle, 0) * new Vector3 (0, 0, 1);
        Gizmos.DrawRay (pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay (pos + direction, left * arrowHeadLength);
    }

    public static void ForDebug (Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay (pos, direction);

        Vector3 right = Quaternion.LookRotation (direction) * Quaternion.Euler (0, 180 + arrowHeadAngle, 0) * new Vector3 (0, 0, 1);
        Vector3 left = Quaternion.LookRotation (direction) * Quaternion.Euler (0, 180 - arrowHeadAngle, 0) * new Vector3 (0, 0, 1);
        Debug.DrawRay (pos + direction, right * arrowHeadLength);
        Debug.DrawRay (pos + direction, left * arrowHeadLength);
    }
    public static void ForDebug (Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay (pos, direction, color);

        Vector3 right = Quaternion.LookRotation (direction) * Quaternion.Euler (0, 180 + arrowHeadAngle, 0) * new Vector3 (0, 0, 1);
        Vector3 left = Quaternion.LookRotation (direction) * Quaternion.Euler (0, 180 - arrowHeadAngle, 0) * new Vector3 (0, 0, 1);
        Debug.DrawRay (pos + direction, right * arrowHeadLength, color);
        Debug.DrawRay (pos + direction, left * arrowHeadLength, color);
    }

    public static void ForGizmo2D (Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawRay (pos, direction);
        pos.z -= 1;
        direction.z -= 1;
        DrawArrowEnd (true, pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
    }

    public static void ForGizmo2D (Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color;
        pos.z -= 1;
        direction.z -= 1;
        Gizmos.DrawRay (pos, direction);
        DrawArrowEnd (true, pos, direction, color, arrowHeadLength, arrowHeadAngle);
    }

    public static void ForDebug2D (Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay (pos, direction);
        pos.z -= 1;
        direction.z -= 1;
        DrawArrowEnd (false, pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
    }

    public static void ForDebug2D (Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color;
        pos.z -= 1;
        direction.z -= 1;
        Debug.DrawRay (pos, direction, color);
        DrawArrowEnd (false, pos, direction, color, arrowHeadLength, arrowHeadAngle);
    }

    private static void DrawArrowEnd (bool gizmos, Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Vector3 right = Quaternion.LookRotation (direction) * Quaternion.Euler (arrowHeadAngle, 0, 0) * Vector3.back;
        Vector3 left = Quaternion.LookRotation (direction) * Quaternion.Euler (-arrowHeadAngle, 0, 0) * Vector3.back;
        Vector3 up = Quaternion.LookRotation (direction) * Quaternion.Euler (0, arrowHeadAngle, 0) * Vector3.back;
        Vector3 down = Quaternion.LookRotation (direction) * Quaternion.Euler (0, -arrowHeadAngle, 0) * Vector3.back;
        if (gizmos)
        {
            Gizmos.color = color;
            Gizmos.DrawRay (pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay (pos + direction, left * arrowHeadLength);
            Gizmos.DrawRay (pos + direction, up * arrowHeadLength);
            Gizmos.DrawRay (pos + direction, down * arrowHeadLength);
        }
        else
        {
            Debug.DrawRay (pos + direction, right * arrowHeadLength, color);
            Debug.DrawRay (pos + direction, left * arrowHeadLength, color);
            Debug.DrawRay (pos + direction, up * arrowHeadLength, color);
            Debug.DrawRay (pos + direction, down * arrowHeadLength, color);
        }
    }
}