using UnityEngine;

public static class VectorDirection
{
    public static Vector3 DetermineDirection(Direction dir)
    {
        Vector3 resultingVector;

        switch (dir)
        {
            case Direction.Up:
                resultingVector = Vector3.up;
                break;
            case Direction.Down:
                resultingVector = Vector3.down;
                break;
            case Direction.Right:
                resultingVector = Vector3.right;
                break;
            case Direction.Left:
                resultingVector = Vector3.left;
                break;
            case Direction.Forward:
                resultingVector = Vector3.forward;
                break;
            case Direction.Back:
                resultingVector = Vector3.back;
                break;
            case Direction.One:
                resultingVector = Vector3.one;
                break;
            case Direction.Zero:
            default:
                resultingVector = Vector3.zero;
                break;
        }

        return resultingVector;

    }

    public static Vector3 DetermineOppositeDirection(Direction dir)
    {

        Vector3 resultingVector;

        switch (dir)
        {
            case Direction.Up:
                resultingVector = Vector3.down;
                break;
            case Direction.Down:
                resultingVector = Vector3.up;
                break;
            case Direction.Right:
                resultingVector = Vector3.left;
                break;
            case Direction.Left:
                resultingVector = Vector3.right;
                break;
            case Direction.Forward:
                resultingVector = Vector3.back;
                break;
            case Direction.Back:
                resultingVector = Vector3.forward;
                break;
            case Direction.One:
                resultingVector = Vector3.one;
                break;
            case Direction.Zero:
            default:
                resultingVector = Vector3.zero;
                break;
        }

        return resultingVector;

    }

    public static Vector3 DeterminePerpendicularDirection(Direction dir)
    {

        Vector3 resultingVector;

        switch (dir)
        {
            case Direction.Up:
                resultingVector = Vector3.right;
                break;
            case Direction.Down:
                resultingVector = Vector3.left;
                break;
            case Direction.Right:
                resultingVector = Vector3.up;
                break;
            case Direction.Left:
                resultingVector = Vector3.down;
                break;
            case Direction.Forward:
                resultingVector = Vector3.back;
                break;
            case Direction.Back:
                resultingVector = Vector3.forward;
                break;
            case Direction.One:
                resultingVector = Vector3.one;
                break;
            case Direction.Zero:
            default:
                resultingVector = Vector3.zero;
                break;
        }

        return resultingVector;

    }

}

// -------------------------------------------------------------------------------------------
[System.Serializable]
public enum Direction
{
    Up,
    Down,
    Right,
    Left,
    Forward,
    Back,
    One,
    Zero
};