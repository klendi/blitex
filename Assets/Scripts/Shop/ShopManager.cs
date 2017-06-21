using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; set; }


    public GameObject[] playersPrefab;
    public Text coins, diamonds;
    public Sprite[] sprites;
    private int[] BallCosts = { 60, 100, 110, 120, 130, 150, 160, 170, 180, 190, 120, 130, 240 };
    public SpriteRenderer[] shopIcons;
    public int selectedBallIndex = 0;
    public int activeBallIndex = 0;

    private void Awake()
    {
        Instance = this;
        UpdateText();
    }

    public void SetBall(int index)
    {
        activeBallIndex = index;
        Manager.Instance.activePlayerBall = index;
    }
    private void UpdateText()
    {
        diamonds.text = SaveManager.Instance.data.diamonds.ToString();
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnBallBuy(int index)
    {
        if (SaveManager.Instance.DoesOwnBall(index))
        {
            //set the ball , we own it so we can set it :)
            SetBall(index);
        }
        else
        {
            //Attempt to buy the ball
            if (SaveManager.Instance.BuyBall(index, BallCosts[index]))
            {
                //Succes
                SetBall(index);
                UpdateText();
            }
            else
            {
                print("Not enough coins or diamonds");
            }
        }
    }
}
