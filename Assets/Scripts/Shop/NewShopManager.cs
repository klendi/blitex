using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class NewShopManager : MonoBehaviour
{
    public Transform ballsPanel;
    public GameObject notEnoughMoneyTab;
    public Outline buyButtonColor;
    public Text buttonText, diamondsText, costText, NotEnoughDiamondsText;
    public VerticalScrollSnap scroll;

    private int[] ballCosts = { 0, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 300, 310, 320, 330, 340, 350 };
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
        notEnoughMoneyTab.SetActive(false);
        activeBallIndex = SaveManager.Instance.data.activeBall;
        SaveManager.Instance.data.diamonds += 50;
        OnNewPage();
        UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Main Menu");
    }

    public void OnNewPage()
    {
        currentPage = scroll.CurrentPage;
        OnBallSelect(currentPage);
    }

    public void CloseNotEnoughTab()
    {
        notEnoughMoneyTab.SetActive(false);
    }

    public void ShowVideoAd()
    {
        AdsManager.Instance.ShowAdVideo();
        notEnoughMoneyTab.SetActive(false);
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
            {
                buttonText.text = "Current";
                buyButtonColor.effectColor = new Color(0f, 234f / 255f, 1f);
            }
            else
            {
                buttonText.text = "Select";
                buyButtonColor.effectColor = new Color(0f, 1f, 171f / 255f);
            }
        }
        else
        {
            if (SaveManager.Instance.data.diamonds >= ballCosts[selectedBallIndex])
            {
                //he can afford it but he hasnt buyed yet, set the color to green
                buttonText.text = "Buy";
                buyButtonColor.effectColor = new Color(0f, 1f, 171f / 255f);
            }
            else if (SaveManager.Instance.data.diamonds < ballCosts[selectedBallIndex])
            {
                buttonText.text = "Buy";
                buyButtonColor.effectColor = new Color(1f, 0f, 90f / 255f);
            }
        }

        costText.text = string.Format("Price: {0}", ballCosts[index].ToString());
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
        buyButtonColor.effectColor = new Color(0f, 234f / 255f, 1f);
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
                NotEnoughDiamondsText.text = "You need " + ballCosts[selectedBallIndex].ToString();
                notEnoughMoneyTab.SetActive(true);
            }
        }
        OnNewPage();
    }
}
