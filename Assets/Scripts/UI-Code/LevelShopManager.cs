﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelShopManager : MonoBehaviour
{
    public GameObject infoLabel;
    public GameObject infoLabelExit;
    public Button soundButton;
    public CanvasGroup cg;

    private void Start()
    {
        cg.alpha = 0;
        infoLabel.SetActive(false);
        infoLabelExit.SetActive(false);

        if (!Manager.Instance.soundOn)
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[0];
        else
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[1];
    }

    public void OnShopClicked()
    {
        SceneManager.LoadScene("Shop-New");
    }
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("LevelSelector-New");
    }

    public void OnSoundClicked()
    {
        Manager.Instance.OnSoundClick(soundButton);
    }

    public void OnInfoClicked()
    {
        cg.alpha = 0;
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