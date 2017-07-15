using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElevatorDirection
{
    Up, Down
}

public class Elevator : MonoBehaviour
{
    public float speed = 1f;
    Vector3 startPos;
    bool detected = false;
    public float distance = 5f;
    public ElevatorDirection direction = ElevatorDirection.Down;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
        {
            detected = true;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, startPos) <= distance && detected)
        {
            if (direction == ElevatorDirection.Down)
                transform.Translate((startPos + Vector3.down) * speed * Time.deltaTime);

            else if (direction == ElevatorDirection.Up)
                transform.Translate((startPos + Vector3.up) * speed * Time.deltaTime);
        }
        else
            detected = false;
    }
}
