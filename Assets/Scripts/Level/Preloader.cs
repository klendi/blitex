using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    private CanvasGroup fadeGroup = new CanvasGroup();
    private float loadTime;
    public float minimunLoadTime = 0; // minimum time of that scene

    private void Start()
    {
        //Grab the only canvas group

        fadeGroup = FindObjectOfType<CanvasGroup>();

        //start with a white screen

        fadeGroup.alpha = 1;

        if (Time.time < minimunLoadTime)
            loadTime = minimunLoadTime;
        else
            loadTime = Time.time;
    }

    private void Update()
    {
        // Fade-in
        if (Time.time < minimunLoadTime)
        {
            fadeGroup.alpha = 1.5f - Time.time;
        }

        //Fade-out

        if (Time.time > minimunLoadTime && loadTime != 0)
        {
            fadeGroup.alpha = Time.time - minimunLoadTime;

            if (fadeGroup.alpha >= 1)
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }
}
