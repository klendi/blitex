using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtons : MonoBehaviour
{
    private void OnMouseDown()
    {
        ShopManager.Instance.OnBallBuy(int.Parse(gameObject.name));
    }
}