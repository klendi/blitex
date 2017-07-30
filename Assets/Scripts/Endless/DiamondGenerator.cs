using UnityEngine;

public class DiamondGenerator : MonoBehaviour
{
    public ObjectPooling diamondPool;
    public float distanceBtwnDiamonds = 1;

    public void SpawnDiamond(Vector3 startPosition)
    {
        //TODO: must fix the diamonds to spawn on the y not the x

        GameObject diamond1 = diamondPool.GetPoolObject();
        diamond1.transform.position = startPosition;
        diamond1.SetActive(true);

        GameObject diamond2 = diamondPool.GetPoolObject();
        diamond2.transform.position = new Vector3(startPosition.x, startPosition.y - distanceBtwnDiamonds, startPosition.z);
        diamond2.SetActive(true);

        GameObject diamond3 = diamondPool.GetPoolObject();
        diamond3.transform.position = new Vector3(startPosition.x, startPosition.y + distanceBtwnDiamonds, startPosition.z);
        diamond3.SetActive(true);
    }

}
