using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelShopManager : MonoBehaviour
{
    public void OnShopClicked()
    {
        //we load the shop level
        print("Shop button clicked , loading shop");
        SceneManager.LoadScene("Shop");
    }
    public void OnPlayClicked()
    {
        print("Play button clicked loading game");
        SceneManager.LoadScene("LevelSelector");
    }
}