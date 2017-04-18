using UnityEngine;

[System.Serializable]
public class Barrier {

    public GameObject gameObject;
    public Direction movementDirection;
    [RangeAttribute(1, 7)]
    public float movementDistance = 1;

    public Barrier(GameObject gameObject, Direction movementDirection, float movementDistance) {

        this.gameObject = gameObject;
        this.movementDirection = movementDirection;
        this.movementDistance = movementDistance;

    }

    public Barrier() { }

}