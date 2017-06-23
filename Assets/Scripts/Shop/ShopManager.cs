using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text diamonds;
    public SpriteRenderer[] frames;
    public ShopButtons[] buttons;
    private int[] BallCosts = { 60, 100, 110, 120, 130, 150, 160, 170, 180, 190, 120, 130, 240 };
    public int activeBallIndex;
    private Color green;   //a ball is bought is is green, is not bought if it is red, and active is is greenish

    private void Awake()
    {
        UpdateText();
        //SaveManager.Instance.Load();
        activeBallIndex = SaveManager.Instance.data.activeBall;
        green = new Color(0, 255, 118);
    }

    private void Update()   //this is to refresh the frames colors
    {
        foreach (var button in buttons)
        {
            int index = int.Parse(button.name) - 1;

            if (SaveManager.Instance.DoesOwnBall(index) && index != activeBallIndex)
            {
                frames[activeBallIndex].color = green;  //if is bought and its not active
            }
            else if (SaveManager.Instance.DoesOwnBall(index) && index == activeBallIndex)
            {
                frames[activeBallIndex].color = Color.cyan;   //is bought and not active
            }
            else if (!SaveManager.Instance.DoesOwnBall(index))
            {
                frames[index].color = Color.red;   //is not bought
            }
        }
    }

    public void SetBall(int index)
    {
        activeBallIndex = index;
        //SaveManager.Instance.data.activeBall = index;
        // SaveManager.Instance.Save();
    }
    private void UpdateText()
    {
        diamonds.text = SaveManager.Instance.data.diamonds.ToString();
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene("Main Menu");
        SaveManager.Instance.data.activeBall = activeBallIndex;
        SaveManager.Instance.Save();
    }

    public void OnBallBuy(int index)
    {
        if (SaveManager.Instance.DoesOwnBall(index))
        {
            //set the ball , we own it so we can set it :)
            SetBall(index);
            print("Set the ball at index " + index);
        }
        else
        {
            //Attempt to buy the ball
            if (SaveManager.Instance.BuyBall(index, BallCosts[index]))
            {
                //Succes
                SetBall(index);
                print("buy the ball at index " + index);
                UpdateText();
            }
            else
            {
                print("Not enough coins or diamonds");
                print("Now ur rich bitch bro");
                SaveManager.Instance.data.diamonds += 5000;
                UpdateText();
            }
        }
    }
}
