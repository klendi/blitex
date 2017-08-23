using UnityEngine;

public class SnowParticles : MonoBehaviour
{
    float speed = .3f;
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
        part.startSizeMultiplier = startScales[i];
    }
}
