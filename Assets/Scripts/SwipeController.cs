using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    public Swipe swipeControls;
    public Transform player;
    private Vector3 desiredPos;
    private Vector3 startPos;

    private void Start()
    {
        startPos = player.position;
        desiredPos += startPos;
    }

    private void Update()
    {
        //if (swipeControls.SwipeLeft)
        //    desiredPos += Vector3.left;
        //else if (swipeControls.SwipeRight)
        //    desiredPos += Vector3.right;
        if (swipeControls.SwipeUp)
            desiredPos += Vector3.down;
        else if (swipeControls.SwipeDown)
            desiredPos += Vector3.up;

        player.position = Vector3.MoveTowards(player.transform.position, desiredPos, 3f * Time.deltaTime);
    }
}
