using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelIcon : MonoBehaviour
{
    private void OnMouseDown()
    {
        string levelName = gameObject.name;
        Manager.Instance.sceneIndex = int.Parse(levelName);

        try
        {
            print("Loading scene " + levelName);
            SceneManager.LoadScene(levelName);
        }
        catch(Exception)
        {
            Debug.Log("Scene don't exist yet");
        }
    }
}
