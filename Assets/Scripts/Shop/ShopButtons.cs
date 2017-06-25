using UnityEngine;

public class ShopButtons : MonoBehaviour
{
    ShopManager shopMg;

    private void Awake()
    {
        shopMg = FindObjectOfType<ShopManager>();
    }
    private void OnMouseDown()
    {
        shopMg.OnBallBuy(int.Parse(gameObject.name) - 1);
    }
}