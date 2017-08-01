using UnityEngine;

public class SpikeGenerator : MonoBehaviour
{
    public ObjectPooling spikePool;


    public void SpawnSpikes(Vector3 startPos)
    {
        GameObject spike1 = spikePool.GetPoolObject();
        spike1.transform.position = startPos;
        spike1.SetActive(true);
    }
}
