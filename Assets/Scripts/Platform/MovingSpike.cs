using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RaycastDirection
{
    Up,
    Down,
    Left,
    Right
}

public class MovingSpike : MonoBehaviour
{
    public float speed;
    public float rotatingSpeed;
    public float range;
    public bool reverseRotatingDirection = false;
    private Vector3 rayOrigin;
    public Transform spikeCenter;
    public RotatingPlatform rotatingPlatform;
    public RaycastDirection raycastDirection = RaycastDirection.Left;

    private void Start()
    {
        rotatingPlatform.loopingMode = true;
    }

    private void Update()
    {
        rayOrigin = spikeCenter.position;
        rotatingPlatform.speed = rotatingSpeed;

        if (reverseRotatingDirection)
        {
            rotatingPlatform.reverse = true;
        }
        else if (!reverseRotatingDirection)
        {
            rotatingPlatform.reverse = false;
        }

        Debug.DrawRay(rayOrigin, Vector2.right * range, Color.red);
        //@klendigocci this is a todo lol

        if (raycastDirection == RaycastDirection.Right)
        {
            if (Physics2D.Raycast(rayOrigin, Vector2.right * range))
            {
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * range);
                Debug.DrawRay(rayOrigin, Vector2.right * range, Color.red);

                if (hit.transform.tag == "Player")
                {
                    rotatingPlatform.speed = rotatingSpeed;
                    print("Im seeing the player");
                    transform.Translate(Vector2.left * speed * Time.deltaTime);
                }
                else
                {
                    print("Im not seeing the player");
                    transform.Translate(Vector2.zero);
                    rotatingPlatform.speed = 0;
                }
            }
        }
    }
}
