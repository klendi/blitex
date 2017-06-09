using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static float cameraMovingSpeed = 0.2f;
    public GameObject player;

    private void Update()
    {
        transform.position += new Vector3(0, -cameraMovingSpeed / 16);
    }
}