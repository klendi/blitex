using UnityEngine;
using UnityEngine.UI;

public enum LevelType
{
    NormalLevels,
    SnowLevels
}
public class LevelCompleted : MonoBehaviour 
{
    Text tx;
    public LevelType levelType = LevelType.NormalLevels;
    private void Awake()
    {
        tx = GetComponent<Text>();
    }
    private void Update () 
	{
        if (levelType == LevelType.NormalLevels)
            tx.text = string.Format("YOU COMPLETED LEVEL {0}", Manager.Instance.sceneIndex);

        else if (levelType == LevelType.SnowLevels)
            tx.text = string.Format("YOU COMPLETED LEVEL {0}", 15 - Manager.Instance.sceneIndex);
	}
}
