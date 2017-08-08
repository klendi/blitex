using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LevelSelectorManager : MonoBehaviour
{
    public Transform levelPanel;
    public GameObject endlessUnlockTab;
    UIVerticalScroller scroll;
    public bool isSnowSelector = false;

    private void Start()
    {
        scroll = FindObjectOfType<UIVerticalScroller>();
        scroll.StartingIndex = SaveManager.Instance.data.completedLevels;
        endlessUnlockTab.SetActive(false);

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

    //******TESTING******//
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
    //*******TESTING******//

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

        int i = 16;

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
        if (SaveManager.Instance.data.specialDiamond <= 10)
        {
            endlessUnlockTab.SetActive(true);
            SaveManager.Instance.data.specialDiamond -= 10;
        }
        else if (SaveManager.Instance.data.completedLevels > 5)
            SceneManager.LoadScene("Endless_normal");
    }

    public void OnSnowEndlessClick()
    {
        if (SaveManager.Instance.data.specialDiamond <= 10)
        {
            endlessUnlockTab.SetActive(true);
            SaveManager.Instance.data.specialDiamond -= 10;
        }
        else if (SaveManager.Instance.data.completedSnowLevels > 5)
            SceneManager.LoadScene("Endless_snow");
    }

    public void OnEndlessTabClick()
    {
        endlessUnlockTab.SetActive(false);
    }
}
