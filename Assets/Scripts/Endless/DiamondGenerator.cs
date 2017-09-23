/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/
using UnityEngine;

public class DiamondGenerator : MonoBehaviour
{
    public GameObject diamondPrefb;

    public void SpawnDiamond(Vector3 pos)
    {
        GameObject diamondd = Instantiate(diamondPrefb, pos, Quaternion.identity);
    }
}
