using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    public float speed;
    public float range;
    float timeCounter = 0;

    private void Update()
    {
        timeCounter += Time.deltaTime * speed;

        transform.position = new Vector3(transform.position.x, Mathf.Cos(timeCounter) * range);
    }
}
