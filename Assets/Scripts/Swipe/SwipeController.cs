using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    public Swipe swipeControls;
    public Transform player;
    public float speed = 3f;   //default is 3
    public Transform maxPoint;
    private Vector3 desiredPos;
    private Vector3 startPos;
    public bool reverse;   //changes btwn swipe ups and swipe downs

    private void Start()
    {
        startPos = player.position;
        desiredPos += startPos;
    }

    private void Update()
    {
        //we use this at level selector, this is when the other part to swipe is up
        if (!reverse)
        {
            if (swipeControls.SwipeUp && player.position.y > startPos.y)
                desiredPos += Vector3.down;
            else if (swipeControls.SwipeDown && player.position.y < maxPoint.position.y)
                desiredPos += Vector3.up;
        }
        //we use this at shop, this is when the other part to swipe is down
        else if (reverse)
        {
            if (swipeControls.SwipeUp && player.position.y > maxPoint.position.y)
                desiredPos += Vector3.down;
            else if (swipeControls.SwipeDown && player.position.y < startPos.y)
                desiredPos += Vector3.up;
        }

        player.position = Vector3.MoveTowards(player.transform.position, desiredPos, speed * Time.deltaTime);
    }
}
