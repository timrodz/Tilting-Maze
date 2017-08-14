using UnityEngine;

[System.Serializable]
public class Barrier
{
    public GameObject gameObject;
    public Direction movementDirection;

    [RangeAttribute(1, 7)]
    public int movementDistance = 1;

    [HideInInspector] public bool hasMoved;
    public bool shouldDeleteFromList = false;

    public Barrier() { }

    public Barrier(GameObject gameObject, Direction movementDirection, int movementDistance)
    {

        this.gameObject = gameObject;
        this.movementDirection = movementDirection;
        this.movementDistance = movementDistance;

    }

}