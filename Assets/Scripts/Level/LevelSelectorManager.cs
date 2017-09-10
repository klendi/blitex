/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       28/8/2017. 10:50
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

/// <summary>
/// The class that handle level selectors
/// </summary>
public class LevelSelectorManager : MonoBehaviour
{
    public Transform levelPanel;
    public Text youNeedMoreDiamonds;
    public GameObject endlessUnlockTab;
    public GameObject snowONOF;
    private bool hasUnlockedNormalEndless = false;
    private bool hasUnlockedSnowEndless = false;
    private bool isSnowOn = true;
    private bool shownInterstital;
    private bool thisTimeShowInterstital = false;
    public bool isSnowSelector = false;

    UIVerticalScroller scroll;
    public SnowParticles snow;

    private void Start()
    {
        //TODO: take a close look at this val
        if (Random.Range(0, 100) < 35)
        {
            //this time we gonna show ads
            AdsManager.Instance.ShowInterstitalAd();
            thisTimeShowInterstital = true;
        }

        scroll = FindObjectOfType<UIVerticalScroller>();
        endlessUnlockTab.SetActive(false);
        hasUnlockedNormalEndless = SaveManager.Instance.data.hasUnlockedNormalEndless;
        hasUnlockedSnowEndless = SaveManager.Instance.data.hasUnlockedSnowEndless;
        isSnowOn = SaveManager.Instance.data.snowOn;

        if (FindObjectOfType<AudioManager>().IsPlaying("LevelTheme"))
        {
            StartCoroutine(FindObjectOfType<AudioManager>().FadeOut("LevelTheme", .25f));
        }

        if (!FindObjectOfType<AudioManager>().IsPlaying("MenuTheme"))
        {
            StartCoroutine(FindObjectOfType<AudioManager>().FadeIn("MenuTheme", .5f));
        }

        if (isSnowOn && isSnowSelector)
        {
            snowONOF.GetComponentInChildren<Text>().text = "SNOW: ON";
            snow.gameObject.SetActive(true);
        }
        else if (!isSnowOn && isSnowSelector)
        {
            snowONOF.GetComponentInChildren<Text>().text = "SNOW: OFF";
            snow.gameObject.SetActive(false);
        }

        if (!isSnowSelector)
        {
            InitLevel();
            scroll.StartingIndex = SaveManager.Instance.data.completedLevels;
        }
        else if (isSnowSelector)
        {
            InitSnowLevel();
            scroll.StartingIndex = SaveManager.Instance.data.completedSnowLevels - 27;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Main Menu");

        if (!AdsManager.Instance.interstitalLoaded && !shownInterstital && thisTimeShowInterstital)
        {
            print("Loaded and showed interstital AD on level selector");
            AdsManager.Instance.ShowInterstitalAd();
        }
        else if (AdsManager.Instance.interstitalLoaded && thisTimeShowInterstital)
        {
            shownInterstital = true;
            thisTimeShowInterstital = false;
        }
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnFrozenPlay()
    {
        SceneManager.LoadScene("LevelSelectorSnow");
    }

    //wtf u were thinking klendi when u published game with testing buttons on -_-
    public void ToNextLevelNormal()
    {
        //SaveManager.Instance.data.completedLevels++;
        //SaveManager.Instance.Save();
        //InitLevel();

        //this button is gonna serve as the next level to be played
        LoadLevel(SaveManager.Instance.data.completedLevels.ToString());
    }

    //wtf u were thinking klendi when u published game with testing buttons on -_-
    public void ToNextLevelSnow()
    {
        //SaveManager.Instance.data.completedSnowLevels++;
        //SaveManager.Instance.Save();
        //InitSnowLevel();

        //this button is gonna serve as the next level to be played
        LoadLevel(SaveManager.Instance.data.completedSnowLevels.ToString());
    }

    private void InitLevel()
    {
        if (levelPanel == null)
            Debug.LogError("You didn't assigned the values to the levelPanel Component");

        //For every children transform under our level panel, find the button and add onclick()

        int i = 0;

        foreach (Transform t in levelPanel)
        {
            Button b = t.GetComponent<Button>();

            if (b.tag == "SpecialLevels")
                b.onClick.AddListener(() => LoadLevelNonIndex(t.GetComponent<Button>().gameObject.name));
            else
                b.onClick.AddListener(() => LoadLevel(t.GetComponent<Button>().gameObject.name));

            if (b != null && b.tag != "SpecialLevels")
            {
                Image img = t.GetComponent<Image>();

                //Is it unlocked?
                if (i <= SaveManager.Instance.data.completedLevels)
                {
                    //It is unlocked!
                    if (i == SaveManager.Instance.data.completedLevels)
                    {
                        //Its not completed
                        img.color = Color.cyan;
                    }
                    else
                    {
                        //Level is already completed
                        img.color = new Color(0f, 1f, 202f / 255f);
                    }
                }
                else
                {
                    //Level isn't unlocked disable the button
                    b.interactable = false;

                    //Set to dark color
                    img.color = Color.grey;
                }

                i++;
            }
        }
    }
    private void InitSnowLevel()
    {
        if (levelPanel == null)
            Debug.LogError("You didn't assigned the values to the levelPanel Component");

        //For every children transform under our level panel, find the button and add onclick()

        int i = 27;

        foreach (Transform t in levelPanel)
        {
            Button b = t.GetComponent<Button>();

            if (b.tag == "SpecialLevels")
                b.onClick.AddListener(() => LoadLevelNonIndex(t.GetComponent<Button>().gameObject.name));
            else
                b.onClick.AddListener(() => LoadLevel(t.GetComponent<Button>().gameObject.name));

            if (b != null && b.tag != "SpecialLevels")
            {
                Image img = t.GetComponent<Image>();

                //Is it unlocked?
                if (i <= SaveManager.Instance.data.completedSnowLevels)
                {
                    //It is unlocked!
                    if (i == SaveManager.Instance.data.completedSnowLevels)
                    {
                        //Its not completed
                        img.color = Color.cyan;
                    }
                    else
                    {
                        //Level is already completed
                        img.color = new Color(0f, 1f, 202f / 255f);
                    }
                }
                else
                {
                    //Level isn't unlocked disable the button
                    b.interactable = false;

                    //Set to dark color
                    img.color = Color.grey;
                }

                i++;
            }
        }
    }

    private void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        Manager.Instance.sceneIndex = int.Parse(name);
    }
    private void LoadLevelNonIndex(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void OnEndlessClick()
    {
        //player can afford it and hasent unlocked snow endless
        if (SaveManager.Instance.data.specialDiamond >= 10 && !hasUnlockedNormalEndless)
        {
            print("Bough normal Endless");
            PlayServices.UnlockAchievement(GPGSIds.achievement_unlock_endless);
            SaveManager.Instance.data.specialDiamond -= 10;
            SaveManager.Instance.data.hasUnlockedNormalEndless = true;
            SaveManager.Instance.Save();
            SceneManager.LoadScene("Endless_normal");
        }
        //he has bough it before
        else if (hasUnlockedNormalEndless)
        {
            SceneManager.LoadScene("Endless_normal");
        }
        else if (SaveManager.Instance.data.specialDiamond < 10 && !hasUnlockedNormalEndless)
        {
            //not enough diamonds
            endlessUnlockTab.SetActive(true);
            youNeedMoreDiamonds.text = "You have " + SaveManager.Instance.data.specialDiamond.ToString();
        }
    }

    public void OnSnowVFXClicked()
    {
        if (isSnowOn)
        {
            isSnowOn = false;
            snowONOF.GetComponentInChildren<Text>().text = "SNOW: OFF";
            SaveManager.Instance.data.snowOn = false;
            SaveManager.Instance.Save();
            snow.gameObject.SetActive(false);
        }
        else if (!isSnowOn)
        {
            isSnowOn = true;
            snowONOF.GetComponentInChildren<Text>().text = "SNOW: ON";
            SaveManager.Instance.data.snowOn = true;
            SaveManager.Instance.Save();
            snow.gameObject.SetActive(true);
        }
    }

    public void OnSnowEndlessClick()
    {
        //player can afford it and hasent unlocked snow endless
        if (SaveManager.Instance.data.specialDiamond >= 10 && !hasUnlockedSnowEndless)
        {
            print("Bough Snow Endless");
            PlayServices.UnlockAchievement(GPGSIds.achievement_unlock_snow_endless);
            SaveManager.Instance.data.specialDiamond -= 10;
            SaveManager.Instance.data.hasUnlockedSnowEndless = true;
            SaveManager.Instance.Save();
            SceneManager.LoadScene("Endless_snow");
        }
        //he has bough it before
        else if (hasUnlockedNormalEndless)
        {
            SceneManager.LoadScene("Endless_snow");
        }
        else if (SaveManager.Instance.data.specialDiamond < 10 && !hasUnlockedSnowEndless)
        {
            //not enough diamonds
            endlessUnlockTab.SetActive(true);
            youNeedMoreDiamonds.text = "You have " + SaveManager.Instance.data.specialDiamond.ToString();
        }
    }

    public void OnEndlessTabClick()
    {
        endlessUnlockTab.SetActive(false);
    }
}
