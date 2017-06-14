using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelShopManager : MonoBehaviour
{
    public GameObject infoLabel;

    public void OnShopClicked()
    {
        //we load the shop level
        print("Shop button clicked");
        SceneManager.LoadScene("Shop");
    }
    public void OnPlayClicked()
    {
        print("Play button clicked");
        SceneManager.LoadScene("LevelSelector");
    }

    public void OnInfoClicked()
    {
        print("Info button clicked");
    }
}