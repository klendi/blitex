﻿/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndlessManager : MonoBehaviour
{
    [Header("Constants")]
    public float speedMultiplier;
    public float speedIncreaseMilestone;
    public float speedMilestoneCount;
    public float score = 0;
    public float highscore = 0;
    public bool isNormalLevel = false;
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

        if (!isNormalLevel)
            highscore = SaveManager.Instance.data.highscore;
        else if (isNormalLevel)
            highscore = SaveManager.Instance.data.normalHighscore;

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

        //we keep the track of the speeds by multiplier
        if (mainCamera.transform.position.y < -speedMilestoneCount)
        {
            print("milestone");
            speedMilestoneCount += speedIncreaseMilestone;
            player.speed *= speedMultiplier;
            mainCamera.cameraMovingSpeed *= speedMultiplier;
            speedIncreaseMilestone *= speedMultiplier;
            rigid.mass *= speedMultiplier;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Endless_snow");
    }

    public void RestartNormalLevel()
    {
        SceneManager.LoadScene("Endless_normal");
    }

    public void OnGameOver()
    {
        if (score > highscore && !isNormalLevel)
        {
            highscore = Mathf.Round(score);
            SaveManager.Instance.data.highscore = Mathf.Round(highscore);
            SaveManager.Instance.Save();
        }

        if (score > highscore && isNormalLevel)
        {
            highscore = Mathf.Round(score);
            SaveManager.Instance.data.normalHighscore = Mathf.Round(highscore);
            SaveManager.Instance.Save();
        }
        AdsManager.Instance.canShowAd = true;

        scoreIncreasing = false;

        if (scoreTxt != null)
            scoreTxt.text = "Score: " + Mathf.Round(score);

        if (highScoreTxt != null)
            highScoreTxt.text = "Highscore: " + Mathf.Round(highscore);
    }

    private void OnHighScore()
    {
        //if highscore isnt 0 then display higscore
        if (score > highscore && !hasDisplayedHighScore && highscore != 0)
        {
            highScoreAnimatedText.SetActive(true);

            if (highScoreAnimatedText.GetComponent<CanvasGroup>().alpha <= 0)
                highScoreAnimatedText.SetActive(false);

            //we set this to this because it mustn's be to 0, this cause when there is no highscore the highscore ui will apear
            highscore = .0001f;
        }
    }
}
