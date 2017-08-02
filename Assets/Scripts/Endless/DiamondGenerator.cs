using UnityEngine;

public class DiamondGenerator : MonoBehaviour
{
    public GameObject diamond;

    public void SpawnDiamond(Vector3 pos)
    {
        GameObject diamondd = Instantiate(diamond, pos, Quaternion.identity);
    }
}
