using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowParticles : MonoBehaviour
{
    float speed = .3f;
    float[] startScales = { };
    ParticleSystem particle;


    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        int i = Random.Range(0, startScales.Length);
        var part = particle.main;
        part.startSizeMultiplier = startScales[i];
    }
}
