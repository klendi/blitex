/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [Header("The Splash Image to fade in/out/")]
    public Image imageToFade;
    public float time = 1f;

    [Header("The scene to load after this scene.")]
    public string sceneName = "Main Menu";
    private bool fadeIn = true;
    private Color color;

    IEnumerator Start()
    {
        print("Started Preloader");
        color = imageToFade.color;
        yield return new WaitForSeconds(time);
        fadeIn = false;
    }

    void Update()
    {

        // Fade the UI Image to alpha = 1.
        if (fadeIn == true)
        {
            color.a += 0.05f;
            imageToFade.color = color;
        }
        // Fade the UI image to alpha = 0.
        else
        {
            color.a -= 0.05f;
            imageToFade.color = color;

            // Completely transparent. Load scene.
            if (imageToFade.color.a <= 0f)
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
