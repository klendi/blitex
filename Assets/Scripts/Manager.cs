using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; set; }
    public int sceneIndex = 1;
    public int coins = 0;
    public int diamonds = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}
