using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject thePlatform;
    public Transform generationPoint;
    private float distanceBetween;

    private float platformWidth;

    public float distanceBetweenMin;
    public float distanceBetweenMax;

    private int platformSelector;

    private float minHeight;
    public Transform maxHeightPoint;
    private float maxHeight;
    public float maxHeightChange;
    private float heightChange;

    //public GameObject[] thePlatforms;

    private float[] platformWidths;

    public ObjectPooling[] theObjectsPool;

    private CoinGenerator theCoinGenerator;
    public float randomCoinsThreshold;



    private void Start()
    {
        //platformWidth = thePlatform.GetComponent<BoxCollider2D>().size.x;
        platformWidths = new float[theObjectsPool.Length];

        for (int i = 0; i < theObjectsPool.Length; i++)
        {
            platformWidths[i] = theObjectsPool[i].pooledObject.GetComponent<BoxCollider2D>().size.y;
        }

        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;

        theCoinGenerator = FindObjectOfType<CoinGenerator>();
    }

    private void Update()
    {
        distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);

        if (transform.position.y < generationPoint.position.y)
        {
            platformSelector = Random.Range(0, theObjectsPool.Length);

            heightChange = transform.position.x + Random.Range(maxHeightChange, -maxHeightChange);

            if (heightChange > maxHeight)
                heightChange = maxHeight;

            else if (heightChange < minHeight)
                heightChange = minHeight;

            //transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, heightChange, transform.position.z);
            transform.position = new Vector3(heightChange, transform.position.y + (platformWidths[platformSelector] / 2) + distanceBetween, transform.position.z);


            //Instantiate(theObjectsPool[platformSelector], transform.position, transform.rotation);


            GameObject newPlatform = theObjectsPool[platformSelector].GetPoolObject();

            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);

            if (Random.Range(0f, 100) < randomCoinsThreshold)
                theCoinGenerator.SpawnCoins(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z));

            transform.position = new Vector3(transform.position.x, transform.position.y + (platformWidths[platformSelector] / 2), transform.position.z);
        }
    }
}
