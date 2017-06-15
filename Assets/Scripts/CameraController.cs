using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static float cameraMovingSpeed = 0.2f;
    public GameObject player;
    public bool DownWards = true;
    public static bool isReady;

    private void Update()
    {
        if (isReady && DownWards)
            transform.position += new Vector3(0, -cameraMovingSpeed / 16);
        else if (isReady && !DownWards)
            transform.position += new Vector3(-cameraMovingSpeed / 16, 0);
    }

    public static void Stop()
    {
        isReady = false;
    }
    public static void Play()
    {
        isReady = true;
    }
    public static void Play(float cameraSpeed)
    {
        cameraMovingSpeed = cameraSpeed;
        isReady = true;
    }
}