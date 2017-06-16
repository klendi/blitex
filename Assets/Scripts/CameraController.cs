using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; set; }
    public float cameraMovingSpeed = 0.2f;
    public GameObject player;
    public bool downWards = true;
    public bool isReady;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (isReady && downWards)
            transform.position += new Vector3(0, -cameraMovingSpeed / 16);
        else if (isReady && !downWards)
            transform.position += new Vector3(0f, cameraMovingSpeed / 16);
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