﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; set; }
    public float cameraMovingSpeed = 2f;
    public float camerSpeedGameOver = .5f;
    public bool downWards = true;
    public bool isReady;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (isReady && downWards)
        {
            transform.Translate(Vector2.down * cameraMovingSpeed * Time.deltaTime);
        }
        else if (isReady && !downWards)
        {
            transform.Translate(Vector2.up * cameraMovingSpeed * Time.deltaTime);
        }
    }

    public void Stop()
    {
        isReady = false;
    }
    public void Play()
    {
        isReady = true;
    }
    public void Play(float cameraSpeed, bool down)
    {
        cameraMovingSpeed = cameraSpeed;
        isReady = true;
        if (down)
            downWards = true;
        else if (!down)
            downWards = false;
    }
    public void Play(bool down)
    {
        isReady = true;

        if (down)
            downWards = true;
        else if (!down)
            downWards = false;
    }
}