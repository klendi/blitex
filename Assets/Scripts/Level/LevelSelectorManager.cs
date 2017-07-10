using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LevelSelectorManager : MonoBehaviour
{
    public Transform levelPanel;
    UIVerticalScroller scroll;

    private void Start()
    {
        scroll = FindObjectOfType<UIVerticalScroller>();
        scroll.StartingIndex = SaveManager.Instance.data.completedLevels;
        InitLevel();
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    //Testing
    public void CompleteLevel()
    {
        SaveManager.Instance.data.completedLevels++;
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

            b.onClick.AddListener(() => LoadLevel(t.GetComponent<Button>().gameObject.name));

            if (b != null)
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
}
