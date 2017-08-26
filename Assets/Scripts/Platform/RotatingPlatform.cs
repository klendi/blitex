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

public class RotatingPlatform : MonoBehaviour
{
    public float speed;
    public float angle;
    public bool loopingMode;
    public bool reverse;
    Quaternion startRotation;
    float timeCounter = 0;

    private void Start()
    {
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        if (!loopingMode)
        {
            timeCounter += Time.deltaTime;
            transform.localRotation = Quaternion.Euler(startRotation.x, startRotation.y, startRotation.z + (angle * Mathf.Sin(speed * timeCounter)));
        }
        else if (loopingMode && !reverse)
        {
            transform.eulerAngles += new Vector3(0, 0, speed);
        }
        else if (loopingMode && reverse)
        {
            transform.eulerAngles += new Vector3(0, 0, -speed);
        }
    }
}