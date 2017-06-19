using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLevel : MonoBehaviour
{
    public static ShopLevel Instance { get; set; }


    private void Awake()
    {
        Instance = this;
    }

    public void SpawnPlayer(GameObject playerSkin)
    {

    }
}
