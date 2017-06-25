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
    public int selectedBallIndex = 0;
    private Color green;   //a ball is bought is is green, is not bought if it is red, and active is is cyan

    private void Awake()
    {
        UpdateText();
        SaveManager.Instance.Load();
        activeBallIndex = SaveManager.Instance.data.activeBall;   //load 
        green = new Color(0, 255, 118);
        SetColors();
    }

    private void SetColors()   //this is to refresh the frames colors
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
        SetActiveBall(index);
        SetColors();
    }
    public void SetActiveBall(int index)
    {
        if (index == activeBallIndex)
            return;
        frames[index].color = Color.cyan;               //put the active one on cyan
        frames[selectedBallIndex].color = Color.green;  //put the previous one on green

        selectedBallIndex = index;
        activeBallIndex = index;
        SaveManager.Instance.data.activeBall = activeBallIndex;
        SetColors();
    }
    private void UpdateText()
    {
        diamonds.text = SaveManager.Instance.data.diamonds.ToString();
    }

    public void OnHomeClick()
    {
        SaveManager.Instance.data.activeBall = activeBallIndex;
        SceneManager.LoadScene("Main Menu");
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
                SaveManager.Instance.data.diamonds += 5000;
                UpdateText();
            }
        }
    }
}
