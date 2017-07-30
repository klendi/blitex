using UnityEngine;

public class DiamondGenerator : MonoBehaviour
{
    public ObjectPooling diamondPool;

    public void SpawnDiamond(Vector3 startPosition)
    {
        GameObject diamond1 = diamondPool.GetPoolObject();
        diamond1.transform.position = startPosition;
        diamond1.SetActive(true);
    }

}
