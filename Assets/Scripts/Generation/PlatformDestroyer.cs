using UnityEngine;
using System.Collections;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject thePlatformDestructionPoint;


    private void Start()
    {
        thePlatformDestructionPoint = GameObject.Find("PlatformDestroyerPoint");
    }

    private void Update()
    {
        if(transform.position.x < thePlatformDestructionPoint.transform.position.x)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
