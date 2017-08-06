using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeRun : MonoBehaviour
{
    public bool isShop = true;

    void Awake()
    {
        if (SaveManager.Instance.data.firstTimeRunning && isShop)
        {
            gameObject.SetActive(true);
            SaveManager.Instance.data.firstTimeRunning = false;
        }
        else if (SaveManager.Instance.data.firstTimeRunninglvl && !isShop)
        {
            gameObject.SetActive(true);
            SaveManager.Instance.data.firstTimeRunninglvl = false;
        }
        else if(!SaveManager.Instance.data.firstTimeRunninglvl && !isShop)
        {
            gameObject.SetActive(false);
        }
        else if (!SaveManager.Instance.data.firstTimeRunning && isShop)
        {
            gameObject.SetActive(false);
        }
    }
}
