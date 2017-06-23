using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelShopManager : MonoBehaviour
{
    public GameObject infoLabel;
    public GameObject infoLabelExit;

    public void Start()
    {
        infoLabel.SetActive(false);
        infoLabelExit.SetActive(false);
    }

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
        infoLabel.SetActive(true);
    }
    public void OnInfoExitClicked()
    {
        StartCoroutine(ExitThenWait(1f));
    }

    private IEnumerator ExitThenWait(float seconds)
    {
        infoLabel.SetActive(false);
        infoLabelExit.SetActive(true);
        yield return new WaitForSeconds(seconds);
        infoLabelExit.SetActive(false);
    }
}