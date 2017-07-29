using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelShopManager : MonoBehaviour
{
    public GameObject infoLabel;
    public GameObject infoLabelExit;
    public GameObject gameModeTab;
    public GameObject playButtonOriginal, playButtonOther;
    public Animator playButtonAnimator;
    public Button soundButton;
    public CanvasGroup cg;
    bool hasPlayed = false;

    private void Start()
    {
        cg.alpha = 0;
        infoLabel.SetActive(false);
        infoLabelExit.SetActive(false);
        gameModeTab.SetActive(false);
        playButtonOriginal.GetComponent<Button>().onClick.AddListener(() => OnAnimStart());

        if (!Manager.Instance.soundOn)
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[0];
        else
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[1];
    }

    public void OnShopClicked()
    {
        SceneManager.LoadScene("Shop");
    }
    public void OnAnimStart()
    {
        if (!hasPlayed)
        {
            playButtonAnimator.SetBool("hasPressed", true);
            StartCoroutine(WaitForAnimToEndThenPlay());
            hasPlayed = true;
        }
        else if (hasPlayed)
        {
            SceneManager.LoadScene("LevelSelector");
        }
    }
    public void OnPlayClick()
    {
        SceneManager.LoadScene("LevelSelector");
    }
    private IEnumerator WaitForAnimToEndThenPlay()
    {
        yield return new WaitForSeconds(.1f);
        gameModeTab.SetActive(true);
    }
    public void OnFrozenPlay()
    {
        SceneManager.LoadScene("LevelSelectorSnow");
    }
    public void OnTwitterClick()
    {
        float startTime;
        startTime = Time.timeSinceLevelLoad;

        //open the twitter app
        Application.OpenURL("twitter:///user?screen_name=klendigocci");

        if (Time.timeSinceLevelLoad - startTime <= 3f)
        {
            //fail. Open chrome.
            Application.OpenURL("http://www.twitter.com/klendigocci");
        }
    }
    public void OnGameReset()
    {
        PlayerPrefs.DeleteKey("save");
    }
    public void OnFacebookClick()
    {
        float startTime;
        startTime = Time.timeSinceLevelLoad;

        //open the fb app
        Application.OpenURL("fb:///page/1984682608469987");

        if (Time.timeSinceLevelLoad - startTime <= 3f)
        {
            //fail. Open chrome.
            Application.OpenURL("http://www.facebook.com/sublexgames");
        }
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