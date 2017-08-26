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

public class SnowParticles : MonoBehaviour
{
    public float speed = .3f;
    public float[] startScales = { .1f, .2f, .15f };
    ParticleSystem particle;


    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();

        if(!SaveManager.Instance.data.snowOn)
        {
            gameObject.SetActive(false);
        }
        else if(SaveManager.Instance.data.snowOn)
        {
            gameObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        int i = Random.Range(0, startScales.Length);
        var part = particle.main;
        part.simulationSpeed = speed;
        part.startSize = startScales[i];
    }
}
