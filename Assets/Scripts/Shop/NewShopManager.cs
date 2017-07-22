using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class NewShopManager : MonoBehaviour
{
    public Transform ballsPanel;
    public Text buttonText, diamondsText;
    public VerticalScrollSnap scroll;
    public Button buyButton;

    private int[] ballCosts = { 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 300, 310, 320, 330, 340, 350 };
    public int activeBallIndex = 0;
    private int currentPage = 0;
    public int selectedBallIndex = 0;

    private void Awake()
    {
        if (SaveManager.Instance.data.activeBall == 0)
        {
            SaveManager.Instance.UnlockBall(0);     //if this is the first time we run the game and we want to have bought the default ball, and set it as bought
            SetBall(0);
        }

        scroll.StartingScreen = SaveManager.Instance.data.activeBall;
        activeBallIndex = SaveManager.Instance.data.activeBall;
        SaveManager.Instance.data.diamonds += 50000;
        OnNewPage();
        UpdateText();
    }

    public void OnNewPage()
    {
        currentPage = scroll.CurrentPage;
        OnBallSelect(currentPage);
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnBallSelect(int index)
    {
        if (selectedBallIndex == index)
            return;

        selectedBallIndex = index;

        if (SaveManager.Instance.DoesOwnBall(selectedBallIndex))
        {
            if (activeBallIndex == selectedBallIndex)
                buttonText.text = "Current";
            else
                buttonText.text = "Select";
        }
        else
        {
            buttonText.text = "Buy " + ballCosts[index].ToString();
        }
    }

    private void UpdateText()
    {
        diamondsText.text = SaveManager.Instance.data.diamonds.ToString();
    }

    private void SetBall(int index)
    {
        activeBallIndex = index;
        SaveManager.Instance.data.activeBall = activeBallIndex;
        buttonText.text = "Current";
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
                SetBall(selectedBallIndex);
                UpdateText();
            }
            else
            {
                print("Not enough gold");
            }
        }
        OnNewPage();
    }
}
