using UnityEngine;
using UnityEngine.UI;

public enum LevelType
{
    NormalLevels,
    SnowLevels,
    LevelText
}
public class LevelCompleted : MonoBehaviour 
{
    Text tx;
    public LevelType levelType = LevelType.NormalLevels;

    private void Awake()
    {
        tx = GetComponent<Text>();

        if (levelType == LevelType.NormalLevels)
            tx.text = string.Format("YOU COMPLETED LEVEL {0}", Manager.Instance.sceneIndex);

        else if (levelType == LevelType.SnowLevels)
            tx.text = string.Format("YOU COMPLETED LEVEL {0}", Manager.Instance.sceneIndex - 15);

        else if(levelType == LevelType.LevelText)
        {
            gameObject.GetComponent<TextMesh>().text = string.Format("LEVEL {0}", Manager.Instance.sceneIndex);
        }
	}
}
