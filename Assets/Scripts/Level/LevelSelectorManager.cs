/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       28/8/2017. 10:50
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The class that handle level selectors
/// </summary>
public class LevelSelectorManager : MonoBehaviour
{
    public Transform levelPanel;
    public Text youNeedMoreDiamonds;
    public GameObject endlessUnlockTab;
    public GameObject snowONOF;
    public bool isSnowSelector = false;
    public SnowParticles snow;
    public Image snowIcon;                  //the snowflake inside the snow toggle, dimmed when the snow is off
    public Sprite lockIcon, completedIcon;  //small icons on the right of the locked and the finished level rows

    const int EndlessCost = 10;             //special diamonds needed to unlock an endless mode
    const int FirstSnowLevel = 27;          //the snow levels come after the 27 normal ones

    static readonly Color CompletedColor = new Color(0f, 1f, 202f / 255f);

    private bool hasUnlockedNormalEndless = false;
    private bool hasUnlockedSnowEndless = false;
    private bool isSnowOn = true;

    CenterSnapScroll scroll;

    private void Start()
    {
        scroll = FindFirstObjectByType<CenterSnapScroll>();
        endlessUnlockTab.SetActive(false);
        hasUnlockedNormalEndless = SaveManager.Instance.data.hasUnlockedNormalEndless;
        hasUnlockedSnowEndless = SaveManager.Instance.data.hasUnlockedSnowEndless;
        isSnowOn = SaveManager.Instance.data.snowOn;

        AudioManager.instance?.PlayMenuMusic();

        if (isSnowSelector)
        {
            SetSnow(isSnowOn);
            InitLevels(FirstSnowLevel, SaveManager.Instance.data.completedSnowLevels);
            scroll.SnapTo(SaveManager.Instance.data.completedSnowLevels - FirstSnowLevel);
        }
        else
        {
            InitLevels(0, SaveManager.Instance.data.completedLevels);
            scroll.SnapTo(SaveManager.Instance.data.completedLevels);
        }

        //a quiet tick whenever another level comes into focus (subscribed after the first snap so it stays silent on load)
        scroll.IndexChanged += _ => AudioManager.instance?.PlaySound("UiTick");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnHomeClick();
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene(SceneNames.MainMenu);
    }

    public void OnFrozenPlay()
    {
        SceneManager.LoadScene(SceneNames.LevelSelectorSnow);
    }

    //the skip button serves as "play the next level"
    public void ToNextLevelNormal()
    {
        LoadLevel(SaveManager.Instance.data.completedLevels.ToString());
    }

    public void ToNextLevelSnow()
    {
        LoadLevel(SaveManager.Instance.data.completedSnowLevels.ToString());
    }

    /// <summary>
    /// Adds the click listener to every level button under the level panel and colors it by its state:
    /// the next level to play is cyan, finished ones are green and the locked ones are grey and disabled
    /// </summary>
    /// <param name="firstIndex">The index of the first level of this panel</param>
    /// <param name="completed">The index of the next level the player has to play</param>
    private void InitLevels(int firstIndex, int completed)
    {
        if (levelPanel == null)
        {
            Debug.LogError("You didn't assigned the values to the levelPanel Component");
            return;
        }

        int index = firstIndex;

        foreach (Transform t in levelPanel)
        {
            Button b = t.GetComponent<Button>();
            if (b == null)
                continue;

            string sceneName = b.gameObject.name;

            //special levels are named by their scene and don't count towards the progress
            if (b.CompareTag("SpecialLevels"))
            {
                b.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
                continue;
            }

            b.onClick.AddListener(() => LoadLevel(sceneName));

            Image img = t.GetComponent<Image>();

            if (index > completed)
            {
                b.interactable = false;
                img.color = Color.grey;
                AddStateIcon(t, lockIcon);
            }
            else if (index == completed)
            {
                img.color = Color.cyan;
            }
            else
            {
                img.color = CompletedColor;
                AddStateIcon(t, completedIcon);
            }

            index++;
        }
    }

    /// <summary>Adds a small icon on the right of a level row (a lock for locked levels, a check for finished ones)</summary>
    private void AddStateIcon(Transform row, Sprite sprite)
    {
        if (sprite == null)
            return;

        GameObject go = new GameObject("StateIcon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(row, false);

        RectTransform rect = (RectTransform)go.transform;
        rect.anchorMin = rect.anchorMax = new Vector2(1f, .5f);
        rect.pivot = new Vector2(.5f, .5f);
        rect.sizeDelta = new Vector2(38f, 38f);
        rect.anchoredPosition = new Vector2(-46f, 0f);

        Image icon = go.GetComponent<Image>();
        icon.sprite = sprite;
        icon.preserveAspect = true;
        icon.raycastTarget = false;
        icon.color = new Color(1f, 1f, 1f, .85f);
    }

    private void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        Manager.Instance.sceneIndex = int.Parse(name);
    }

    public void OnEndlessClick()
    {
        Action markUnlocked = () =>
        {
            hasUnlockedNormalEndless = true;
            SaveManager.Instance.data.hasUnlockedNormalEndless = true;
        };

        if (TryUnlockEndless(hasUnlockedNormalEndless, GPGSIds.achievement_unlock_endless, markUnlocked))
            SceneManager.LoadScene(SceneNames.EndlessNormal);
    }

    public void OnSnowEndlessClick()
    {
        Action markUnlocked = () =>
        {
            hasUnlockedSnowEndless = true;
            SaveManager.Instance.data.hasUnlockedSnowEndless = true;
        };

        if (TryUnlockEndless(hasUnlockedSnowEndless, GPGSIds.achievement_unlock_snow_endless, markUnlocked))
            SceneManager.LoadScene(SceneNames.EndlessSnow);
    }

    /// <summary>
    /// True when the endless mode is open to the player: either bought before or bought just now with special diamonds.
    /// When the player can't afford it we show the "you need more diamonds" tab instead.
    /// </summary>
    private bool TryUnlockEndless(bool alreadyUnlocked, string achievementId, Action markUnlocked)
    {
        if (alreadyUnlocked)
            return true;

        if (SaveManager.Instance.data.specialDiamond < EndlessCost)
        {
            endlessUnlockTab.SetActive(true);
            youNeedMoreDiamonds.text = "You have " + SaveManager.Instance.data.specialDiamond;
            return false;
        }

        PlayServices.UnlockAchievement(achievementId);
        SaveManager.Instance.data.specialDiamond -= EndlessCost;
        markUnlocked();
        SaveManager.Instance.Save();
        return true;
    }

    public void OnSnowVFXClicked()
    {
        SetSnow(!isSnowOn);
        SaveManager.Instance.data.snowOn = isSnowOn;
        SaveManager.Instance.Save();
    }

    private void SetSnow(bool on)
    {
        isSnowOn = on;
        snowONOF.GetComponentInChildren<Text>().text = on ? "SNOW: ON" : "SNOW: OFF";
        if (snowIcon != null)
            snowIcon.color = on ? Color.white : new Color(1f, 1f, 1f, .4f);
        snow.gameObject.SetActive(on);
    }

    public void OnEndlessTabClick()
    {
        endlessUnlockTab.SetActive(false);
    }
}
