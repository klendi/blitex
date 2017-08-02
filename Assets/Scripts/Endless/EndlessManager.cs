using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndlessManager : MonoBehaviour
{
    [Header("Constants")]
    public float cameraSpeed = .8f;
    public float playerSpeed = 3f;
    public float trapsSpeed = .5f;
    public float speedMultiplier;
    public float speedIncreaseMilestone;
    private float speedMilestoneCount;
    public float score = 0;
    public float highscore = 0;
    public bool scoreIncreasing = false;
    public float pointsForSecond = 0;

    public bool hasDisplayedHighScore = false;


    [Header("Attachments")]
    public Text scoreTxt;
    public Text scoreUpdateText;
    public Text highScoreTxt;
    public GameObject highScoreAnimatedText;

    CameraController mainCamera;
    PlayerController player;
    Rigidbody2D rigid;


    private void Start()
    {
        mainCamera = FindObjectOfType<CameraController>();
        player = FindObjectOfType<PlayerController>();
        rigid = FindObjectOfType<Rigidbody2D>();
        speedMilestoneCount = speedIncreaseMilestone;
        highscore = SaveManager.Instance.data.highscore;
        rigid.useAutoMass = false;
    }

    private void Update()
    {
        if (scoreIncreasing)
        {
            score += pointsForSecond * Time.deltaTime;
            scoreUpdateText.text = "Score: " + Mathf.Round(score);
        }

        if (score > highscore)
        {
            OnHighScore();
        }

        if (mainCamera.transform.position.y < -speedMilestoneCount)
        {
            print("milestone");
            speedMilestoneCount += speedIncreaseMilestone;
            player.speed *= speedMultiplier;
            mainCamera.cameraMovingSpeed *= speedMultiplier;
            speedIncreaseMilestone *= speedMultiplier;
            trapsSpeed *= speedMultiplier;
            rigid.mass *= speedMultiplier;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Endless");
    }

    public void OnGameOver()
    {
        if (score > highscore)
        {
            highscore = Mathf.Round(score);
            SaveManager.Instance.data.highscore = Mathf.Round(highscore);
            SaveManager.Instance.Save();
        }

        scoreIncreasing = false;

        if (scoreTxt != null)
            scoreTxt.text = "Score: " + Mathf.Round(score);

        if (highScoreTxt != null)
            highScoreTxt.text = "Highscore: " + Mathf.Round(highscore);
    }

    private void OnHighScore()
    {
        if (score > highscore && !hasDisplayedHighScore && highscore != 0)
        {
            highScoreAnimatedText.SetActive(true);

            if (highScoreAnimatedText.GetComponent<CanvasGroup>().alpha <= 0)
                highScoreAnimatedText.SetActive(false);

            highscore = .0001f;
        }
    }
}
