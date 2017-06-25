using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LevelSelectorManager : MonoBehaviour
{
    public Transform levelPanel;
    UIVerticalScroller scroll;

    private void Awake()
    {
        scroll = FindObjectOfType<UIVerticalScroller>(); 
        scroll.StartingIndex = SaveManager.Instance.data.completedLevel;

        foreach (Transform t in levelPanel)
        {
            t.GetComponent<Button>().onClick.AddListener(() => LoadLevel(t.GetComponent<Button>().gameObject.name));
        }
    }

    private void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        Manager.Instance.sceneIndex = int.Parse(name);
    }
}
