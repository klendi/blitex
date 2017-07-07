using UnityEngine;
using UnityEngine.UI;

public class LevelCompleted : MonoBehaviour 
{
    Text tx;
    private void Awake()
    {
        tx = GetComponent<Text>();
    }
    private void Update () 
	{
        tx.text = string.Format("YOU COMPLETED LEVEL {0}", Manager.Instance.sceneIndex);
	}
}
