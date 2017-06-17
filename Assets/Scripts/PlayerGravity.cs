using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public PlayerGravity Instance;

    Vector3 velocity = new Vector3(5, 5, 0);
    float gravity = 9.8f;

    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }
}
