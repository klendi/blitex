using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerToSpawn;
    public Transform spawnPoint;
    public int savemanagerindex;

    private void Start()
    {
        playerToSpawn = Manager.Instance.playerPrefabs[SaveManager.Instance.data.activeBall];
        Instantiate(playerToSpawn, spawnPoint.position, Quaternion.identity);
    }

    private void Update()
    {
        savemanagerindex = SaveManager.Instance.data.activeBall;
    }
}
