/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The class that handles the main menu: play, shop, info and rate-us panels and the sound toggle
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    public GameObject infoLabel;            //the info panel
    public CanvasGroup infoGroup;           //its canvas group, we fade it in and out
    public GameObject gameModeTab;
    public GameObject playButtonOriginal;
    public GameObject rateUsTab;
    public Animator playButtonAnimator;
    public Button soundButton;              //the sound button on the info panel

    const float InfoFadeSeconds = .2f;

    bool hasPlayed = false;
    bool isAtInfoTab = false;
    Coroutine infoFade;

    private void Start()
    {
        infoGroup.alpha = 0;
        infoLabel.SetActive(false);
        gameModeTab.SetActive(false);
        rateUsTab.SetActive(false);
        playButtonOriginal.GetComponent<Button>().onClick.AddListener(OnAnimStart);

        AudioManager.instance?.PlayMenuMusic();

        if ((SaveManager.Instance.data.completedLevels == 20 || SaveManager.Instance.data.completedSnowLevels == 14) && SaveManager.Instance.data.hasRatedGame)
        {
            rateUsTab.SetActive(true);
        }

        UpdateSoundIcons();
    }

    private void Update()
    {
        if (isAtInfoTab && Input.GetKeyDown(KeyCode.Escape))
        {
            OnInfoExitClicked();
        }
    }

    public void OnShopClicked()
    {
        SceneManager.LoadScene(SceneNames.Shop);
    }

    /// <summary>
    /// The first press of play shows the game modes (if the snow theme is unlocked), the next one goes to the levels
    /// </summary>
    public void OnAnimStart()
    {
        if (!hasPlayed && SaveManager.Instance.data.hasUnlockedSnowTheme)
        {
            playButtonAnimator.SetBool("hasPressed", true);
            StartCoroutine(ShowGameModesAfter(.1f));
            hasPlayed = true;
        }
        else
        {
            SceneManager.LoadScene(SceneNames.LevelSelector);
        }
    }

    public void OnPlayClick()
    {
        SceneManager.LoadScene(SceneNames.LevelSelector);
    }

    public void OnFrozenPlay()
    {
        SceneManager.LoadScene(SceneNames.LevelSelectorSnow);
    }

    public void OnGameReset()
    {
        SaveManager.Instance.ResetSave();
        SaveManager.Instance.Load();
        Debug.Log("Reset succesfully");
    }

    public void OnSoundClicked()
    {
        Manager.Instance.OnSoundClick(soundButton);
    }

    public void OnRatingClicked()
    {
        SaveManager.Instance.Save();
        RequestReview();
    }

    public void OnRatingTabClicked()
    {
        SaveManager.Instance.data.hasRatedGame = true;
        SaveManager.Instance.Save();
        RequestReview();
    }

    public void OnRatingTabExit()
    {
        rateUsTab.SetActive(false);
    }

    public void OnInfoClicked()
    {
        infoLabel.SetActive(true);
        FadeInfo(1f);
        isAtInfoTab = true;
    }

    public void OnInfoExitClicked()
    {
        FadeInfo(0f);
        isAtInfoTab = false;
    }

    void FadeInfo(float target)
    {
        if (infoFade != null)
            StopCoroutine(infoFade);

        infoFade = StartCoroutine(FadeInfoTo(target));
    }

    void UpdateSoundIcons()
    {
        soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[Manager.Instance.soundOn ? 1 : 0];
    }

    static void RequestReview()
    {
#if UNITY_IOS
        UnityEngine.iOS.Device.RequestStoreReview();
#endif
    }

    private IEnumerator ShowGameModesAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameModeTab.SetActive(true);
    }

    //fades the info panel with unscaled time and hides it once it is fully transparent
    private IEnumerator FadeInfoTo(float target)
    {
        float start = infoGroup.alpha;

        for (float t = 0f; t < InfoFadeSeconds; t += Time.unscaledDeltaTime)
        {
            infoGroup.alpha = Mathf.Lerp(start, target, t / InfoFadeSeconds);
            yield return null;
        }

        infoGroup.alpha = target;

        if (target <= 0f)
            infoLabel.SetActive(false);
    }
}
