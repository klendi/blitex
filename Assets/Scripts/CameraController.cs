using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static float cameraMovingSpeed = 0.2f;
    public GameObject player;
    public static bool isReady = false;

    private void Update()
    {
        if (isReady)
            transform.position += new Vector3(0, -cameraMovingSpeed / 16);
    }
}