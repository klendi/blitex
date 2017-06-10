using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsEffects : MonoBehaviour
{
    /// <summary
    /// This class is all in one
    /// That mean that you can do all the kind of animations if you drag
    /// and drop the gameobject in the array
    /// but you have three kinds of scripts (int this folder) that do that seperately
    /// and you have to attach to a object
    /// </summary>


    public GameObject[] leftRightMovement;   //plaforms that will move left right
    public GameObject[] scaleRightLeft;      //platforms that will move right left
    public GameObject[] upDownMovement;     //platforms that will move up down


    [Header("Up-Down movement")]
    public float Upheight;
    public float Upspeed;
    float UptimeCounter = 0;

    [Header("Left-Right movement")]
    public float leftSpeed;
    public float leftRange;
    float leftTimeCounter = 0;

    [Header("Left-Right Scaling")]
    public float scaleSpeed;
    public float scaleRange;
    float scaleTimeCounter = 0;


    private void Update()
    {
        UptimeCounter += Time.deltaTime * Upspeed;
        leftTimeCounter += Time.deltaTime * leftSpeed;
        scaleTimeCounter += Time.deltaTime * scaleSpeed;

        if (leftRightMovement != null)
        {
            foreach (var platform in leftRightMovement)
            {
                float x = Mathf.Sin(leftTimeCounter) * leftRange;
                platform.transform.position = new Vector3(x, platform.transform.position.y);
            }
        }

        if (scaleRightLeft != null)
        {
            foreach (var platform in scaleRightLeft)
            {

            }
        }
        if (upDownMovement != null)
        {
            foreach (var platform in upDownMovement)
            {
                float y = Mathf.Sin(UptimeCounter) * Upheight;
                platform.transform.position = new Vector3(platform.transform.position.x, y);
            }
        }
    }

    //Once the platforms are invisible we turn them off to optimize
    private void OnBecameInvisible()
    {
        if (leftRightMovement != null)
        {
            foreach (var platform in leftRightMovement)
            {
            }
        }

        if (scaleRightLeft != null)
        {
            foreach (var platform in scaleRightLeft)
            {
            }
        }
        if (upDownMovement != null)
        {
            foreach (var platform in upDownMovement)
            {
            }
        }
    }
}
