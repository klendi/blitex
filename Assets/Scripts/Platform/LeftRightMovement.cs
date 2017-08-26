/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using UnityEngine;

public enum MovementType
{
    LeftToRight,
    RightToLeft
}

public class LeftRightMovement : MonoBehaviour
{
    public float speed;
    public float range;
    [Tooltip("This is for delaying the platforms")]
    public MovementType movementType = MovementType.LeftToRight;
    float timeCounter = 0;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        timeCounter += Time.deltaTime * speed;

        if (movementType == MovementType.RightToLeft)
        {
            transform.position = startPos + new Vector3(Mathf.Sin(timeCounter) * range, 0f);
        }
        else if (movementType == MovementType.LeftToRight)
        {
            transform.position = startPos + new Vector3(-Mathf.Sin(timeCounter) * range, 0f);
        }
    }
}