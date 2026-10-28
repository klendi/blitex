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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The class that handle shop things
/// </summary>
public class NewShopManager : MonoBehaviour
{
    public Transform ballsPanel;
    public GameObject notEnoughMoneyTab;
    static readonly Color BuyButtonBase = new Color(0.051f, 0.082f, 0.133f);

    public Graphic buyButtonColor;   //the outline ring of the buy button, we tint it by state
    public Text buttonText, diamondsText, costText, NotEnoughDiamondsText;
    public CenterSnapScroll scroll;

    public int activeBallIndex = 0;
    private int currentPage = 0;
    public int selectedBallIndex = 0;
    public int[] ballCosts = { 0, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 300, 310, 320, 330, 340, 350 };

    private void Awake()
    {
        if (SaveManager.Instance.data.activeBall == 0)
        {
            SaveManager.Instance.UnlockBall(0);     //if this is the first time we run the game and we want to have bought the default ball, and set it as bought
            SetBall(0);
        }
        //we can trust this script, so make it a part of our sys
        scroll.SnapTo(SaveManager.Instance.data.activeBall);
        notEnoughMoneyTab.SetActive(false);
        activeBallIndex = SaveManager.Instance.data.activeBall;
        OnNewPage();
        scroll.IndexChanged += _ => OnNewPage();
        UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(SceneNames.MainMenu);
    }

    public void OnNewPage()
    {
        currentPage = scroll.CurrentIndex;
        OnBallSelect(currentPage);
    }

    public void CloseNotEnoughTab()
    {
        notEnoughMoneyTab.SetActive(false);
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene(SceneNames.MainMenu);
    }

    public void OnBallSelect(int index)
    {
        if (selectedBallIndex == index)
            return;

        selectedBallIndex = index;

        if (SaveManager.Instance.DoesOwnBall(selectedBallIndex))
        {
            if (activeBallIndex == selectedBallIndex)
            {
                buttonText.text = "Current";
                SetBuyColor(new Color(0f, 234f / 255f, 1f));
            }
            else
            {
                buttonText.text = "Select";
                SetBuyColor(new Color(0f, 1f, 171f / 255f));
            }
        }
        else
        {
            if (SaveManager.Instance.data.diamonds >= ballCosts[selectedBallIndex])
            {
                //he can afford it but he hasnt buyed yet, set the color to green
                buttonText.text = "Buy";
                SetBuyColor(new Color(0f, 1f, 171f / 255f));
            }
            else if (SaveManager.Instance.data.diamonds < ballCosts[selectedBallIndex])
            {
                //he cant afford it
                buttonText.text = "Buy";
                SetBuyColor(new Color(1f, 0f, 90f / 255f));
            }
        }

        costText.text = string.Format("Price: {0}", ballCosts[index].ToString());
    }

    public void UpdateText()
    {
        diamondsText.text = SaveManager.Instance.data.diamonds.ToString();
    }

    private void SetBall(int index)
    {
        activeBallIndex = index;
        SaveManager.Instance.data.activeBall = activeBallIndex;
        buttonText.text = "Current";
        SetBuyColor(new Color(0f, 234f / 255f, 1f));
    }

    public void OnBallBuy()
    {
        if (SaveManager.Instance.DoesOwnBall(selectedBallIndex))
        {
            SetBall(selectedBallIndex);
        }
        else
        {
            if (SaveManager.Instance.BuyBall(selectedBallIndex, ballCosts[selectedBallIndex]))
            {
                //succes he bough the ball
                PlayServices.UnlockAchievement(GPGSIds.achievement_first_purchase);
                PlayServices.IncrementAchievement(GPGSIds.achievement_5_balls_purchased, 1);
                SetBall(selectedBallIndex);
                UpdateText();
            }
            else
            {
                //he dont have enough diamonds to buy it :C
                NotEnoughDiamondsText.text = "You need " + (ballCosts[selectedBallIndex] - SaveManager.Instance.data.diamonds);
                notEnoughMoneyTab.SetActive(true);
            }
        }
        OnNewPage();
    }

    /// <summary>
    /// Tints the buy button: the ring gets the state color and the fill a faint wash of it over the dark base
    /// </summary>
    void SetBuyColor(Color accent)
    {
        buyButtonColor.color = accent;

        Graphic fill = buyButtonColor.transform.parent.GetComponent<Graphic>();
        if (fill != null)
            fill.color = Color.Lerp(BuyButtonBase, accent, .15f);
    }
}
