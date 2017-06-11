using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelIcon : MonoBehaviour
{
    private void OnMouseDown()
    {
        string levelName = gameObject.name;

        try
        {
            print("Loading scene " + levelName);
            SceneManager.LoadScene(levelName);
        }
        catch(UnityException)
        {
            Debug.Log("Scene don't exist yet");
        }
    }
}
