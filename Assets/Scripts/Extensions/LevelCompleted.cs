using UnityEngine;
using UnityEngine.UI;

public enum LevelType
{
    NormalLevels,
    SnowLevels,
    BlackAndWhite
}
public class LevelCompleted : MonoBehaviour 
{
    Text tx;
    public LevelType levelType = LevelType.NormalLevels;

    private void Awake()
    {
        tx = GetComponent<Text>();

        if (levelType == LevelType.NormalLevels)
            tx.text = string.Format("YOU COMPLETED LEVEL {0}", Manager.Instance.sceneIndex + 1);

        else if (levelType == LevelType.SnowLevels)
            tx.text = string.Format("YOU COMPLETED LEVEL {0}", Manager.Instance.sceneIndex - Manager.Instance.totalNumSnowLevels);
	}
}
