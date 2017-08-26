/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
