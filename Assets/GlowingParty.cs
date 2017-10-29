using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingParty : MonoBehaviour
{
    public Material glowMaterial;
    Shader shader;
    public Color a, b;
    float duration = 5; // This will be your time in seconds.
    float smoothness = 0.02f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.

    void Start()
    {
        //glowMaterial.get = new Color(255 / 255, 0, 5 / 255);
        StartCoroutine("LerpColor");
    }


    IEnumerator LerpColor()
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness / duration; //The amount of change to apply.
        while (progress < 50)
        {
            glowMaterial.color = Color.Lerp(a, b, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
        //return true;
    }
}
