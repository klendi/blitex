using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelShopManager : MonoBehaviour
{
    public GameObject infoLabel;
    public GameObject infoLabelExit;
    public Button soundButton;
    public CanvasGroup cg;
    public int savemanagerindex;

    private void Start()
    {
        cg.alpha = 0;
        infoLabel.SetActive(false);
        infoLabelExit.SetActive(false);

        if (Manager.Instance.soundOn)
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[0];
        else
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[1];
    }

    private void Update()
    {
        savemanagerindex = SaveManager.Instance.data.activeBall;
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