using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerToSpawn;
    public Transform spawnPoint;

    private void Start()
    {
        playerToSpawn = Manager.Instance.playerPrefabs[SaveManager.Instance.data.activeBall];
        GameObject player = Instantiate(playerToSpawn, spawnPoint.position, Quaternion.identity);
    }
}
