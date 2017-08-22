using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [Header("The Splash Image to fade in/out/")]
    public Image splashPane;
    public float time = 1f;

    [Header("The scene to load after this scene.")]
    public string sceneName = "Main Menu";
    private bool fadeIn = true;
    private Color col;

    IEnumerator Start()
    {
        col = splashPane.color;
        yield return new WaitForSeconds(time);
        fadeIn = false;
    }

    void Update()
    {

        // Fade the UI Image to alpha = 1.
        if (fadeIn == true)
        {
            col.a += 0.05f;
            splashPane.color = col;
        }
        // Fade the UI image to alpha = 0.
        else
        {
            col.a -= 0.05f;
            splashPane.color = col;

            // Completely transparent. Load scene.
            if (splashPane.color.a <= 0f)
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
