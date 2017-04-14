using UnityEngine;
using System.Collections;

public class VectorDirection {

	public enum Direction {
		Up,
		Down,
		Right,
		Left,
		Forward,
		Back,
		Equally,
		None
	};

	public static Vector3 DetermineDirection(Direction dir) {

		Vector3 resultingVector = Vector3.zero;

		switch (dir) {
			case VectorDirection.Direction.Up:
				resultingVector = Vector3.up;
				break;
			case VectorDirection.Direction.Down:
				resultingVector = Vector3.down;
				break;
			case VectorDirection.Direction.Right:
				resultingVector = Vector3.right;
				break;
			case VectorDirection.Direction.Left:
				resultingVector = Vector3.left;
				break;
			case VectorDirection.Direction.Forward:
				resultingVector = Vector3.back;
				break;
			case VectorDirection.Direction.Back:
				resultingVector = Vector3.forward;
				break;
			case VectorDirection.Direction.Equally:
				resultingVector = Vector3.one;
				break;
		}

		return resultingVector;

	}

}