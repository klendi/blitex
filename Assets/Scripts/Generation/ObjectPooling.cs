using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    public GameObject pooledObject;

    public int poolAmount;

    List<GameObject> pooledObjects;

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject) as GameObject;
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPoolObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        GameObject obj = Instantiate(pooledObject) as GameObject;
        obj.SetActive(false);
        pooledObjects.Add(obj);

        return obj;
    }
}
