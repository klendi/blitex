using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LevelSelectorManager : MonoBehaviour
{
    public Transform levelPanel;
    public GameObject endlessUnlockTab;
    public GameObject snowONOF;
    bool hasUnlockedNormalEndless = false;
    bool hasUnlockedSnowEndless = false;
    bool isSnowOn = true;
    public bool isSnowSelector = false;

    UIVerticalScroller scroll;
    public SnowParticles snow;

    private void Start()
    {
        scroll = FindObjectOfType<UIVerticalScroller>();
        scroll.StartingIndex = SaveManager.Instance.data.completedLevels;
        endlessUnlockTab.SetActive(false);
        hasUnlockedNormalEndless = SaveManager.Instance.data.hasUnlockedNormalEndless;
        hasUnlockedSnowEndless = SaveManager.Instance.data.hasUnlockedSnowEndless;
        isSnowOn = SaveManager.Instance.data.snowOn;

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
            InitLevel();
        else if (isSnowSelector)
            InitSnowLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Main Menu");
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnFrozenPlay()
    {
        SceneManager.LoadScene("LevelSelectorSnow");
    }

    public void CompleteLevel()
    {
        SaveManager.Instance.data.completedLevels++;
        SaveManager.Instance.Save();
    }
    public void CompleteLevelSnow()
    {
        SaveManager.Instance.data.completedSnowLevels++;
        SaveManager.Instance.Save();
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

        int i = Manager.Instance.totalNumSnowLevels;

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
                        img.color = Color.green;
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
        }
    }

    public void OnSnowVFXClicked()
    {
        if(isSnowOn)
        {
            isSnowOn = false;
            snowONOF.GetComponentInChildren<Text>().text = "SNOW: OFF";
            SaveManager.Instance.data.snowOn = false;
            SaveManager.Instance.Save();
            snow.gameObject.SetActive(false);
        }
        else if(!isSnowOn)
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
        else if(SaveManager.Instance.data.specialDiamond < 10 && !hasUnlockedSnowEndless)
        {
            //not enough diamonds
            endlessUnlockTab.SetActive(true);
        }
    }

    public void OnEndlessTabClick()
    {
        endlessUnlockTab.SetActive(false);
    }
}
