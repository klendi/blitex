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

public class UpDownMovement : MonoBehaviour
{
    public float speed;
    public float range;
    float timeCounter = 0;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        timeCounter += Time.deltaTime * speed;

        transform.position = startPos + new Vector3(0f, Mathf.Cos(timeCounter) * range);
    }
}
