/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject platformDestroyerPoint;
    private void Start()
    {
        platformDestroyerPoint = GameObject.Find("PlatformDestroyerPoint");
    }
    private void Update()
    {
        //for every object that is behind that gameobject destroy it
        if (transform.position.y > platformDestroyerPoint.transform.position.y)
        {
            gameObject.SetActive(false);
        }
    }
}
