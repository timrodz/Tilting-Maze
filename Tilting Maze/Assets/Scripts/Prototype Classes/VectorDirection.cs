using UnityEngine;
using System.Collections;

public class VectorDirection {

	public enum directions {
		Up,
		Down,
		Right,
		Left,
		Forward,
		Back,
		Equally,
		None
	};

	public static Vector3 DetermineDirection(directions dir) {

		Vector3 resultingVector = Vector3.zero;

		switch (dir) {
			case VectorDirection.directions.Up:
				resultingVector = Vector3.up;
				break;
			case VectorDirection.directions.Down:
				resultingVector = Vector3.down;
				break;
			case VectorDirection.directions.Right:
				resultingVector = Vector3.right;
				break;
			case VectorDirection.directions.Left:
				resultingVector = Vector3.left;
				break;
			case VectorDirection.directions.Forward:
				resultingVector = Vector3.back;
				break;
			case VectorDirection.directions.Back:
				resultingVector = Vector3.forward;
				break;
			case VectorDirection.directions.Equally:
				resultingVector = Vector3.one;
				break;
		}

		return resultingVector;

	}

}