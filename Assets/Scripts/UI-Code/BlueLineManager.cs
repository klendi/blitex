using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class BlueLineManager : MonoBehaviour
{
    public Transform[] levelButtons;
    private LineRenderer line;

    private void Awake()
    {
        levelButtons = GetComponentsInChildren<Transform>();
        line = GetComponent<LineRenderer>();
        line.numPositions = levelButtons.Length;
    }

    private void Update()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i == 0)
                line.SetPosition(0, levelButtons[0].position);

            if (i != 0 && i < levelButtons.Length)
                line.SetPosition(i, levelButtons[i].position);
        }
    }
}