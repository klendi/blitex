using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; set; }
    public int sceneIndex = 1;
    public int activePlayerBall = 0;
    public GameObject[] playerPrefabs;

    [HideInInspector]
    public GameObject playerToSpawn;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        activePlayerBall = SaveManager.Instance.data.activeBall;
        playerToSpawn = playerPrefabs[activePlayerBall];
    }
}
