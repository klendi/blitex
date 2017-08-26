/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum UpOrDown
{
    UP,
    DOWN
}

public class OutOfCameraDetector : MonoBehaviour
{
    float cameraSize = 0f;
    UpOrDown pos = UpOrDown.DOWN;
    public float cameraOffset = 0f;
    public float upCameraOffset = 0f;
    LevelManager level;

    private void Awake()
    {
        cameraSize = Screen.height;
        level = FindObjectOfType<LevelManager>();

        if (pos == UpOrDown.DOWN)
        {
            gameObject.transform.position = new Vector3(0f, -cameraSize - cameraOffset);
        }
        else if (pos == UpOrDown.UP)
        {
            gameObject.transform.position = new Vector3(0f, upCameraOffset);
        }
    }

    private void OnCollision2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
            level.OnGameOver();
    }
}
