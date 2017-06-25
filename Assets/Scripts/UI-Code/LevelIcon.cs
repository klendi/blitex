using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelIcon : MonoBehaviour
{
    private void OnMouseDown()
    {
        string levelName = gameObject.name;
        Manager.Instance.sceneIndex = int.Parse(levelName);
        SceneManager.LoadScene(levelName);
        Debug.Log("Scene don't exist yet");
    }
}
