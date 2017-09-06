/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       5/9/2017. 12:47
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class LevelManager : MonoBehaviour
{
    #region variables
    [Header("Diamonds")]
    public int diamonds = 0;
    int totalDiamonds = 0, totalSpecialDiamonds = 0;
    public int specialDiamonds = 0;
    public bool isSnowLevel = false;
    public bool isEndlessLevel = false;
    [HideInInspector]
    public bool paused = false, gameOver = false;
    bool instanciated = false;

    [Header("Attachments")]
    PlayerController player;
    CameraController cameraController;
    EndlessManager endless;
    Animator anim;
    public Button pauseButton;
    public Button soundButton;
    public Text highScoreDiamonds, highScoreSpecialDiamonds;
    public Text diamondsText;
    public GameObject pauseTab;
    public GameObject succesTab;
    public GameObject uiTab;
    public GameObject startTab;
    public GameObject gameOverTab;
    public GameObject unlockedSnowTheme;
    #endregion


    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(LoadPlayButton(0.2f));    //load the big play button when the level loads, play that after 0.2 seconds
        pauseTab.SetActive(false);           //set the pause tab false
        succesTab.SetActive(false);         //set the succes tab false
        gameOverTab.SetActive(false);       //set the game over tab false
        uiTab.SetActive(true);              //set the ui active
        pauseButton.enabled = false;
        Camera.main.gameObject.AddComponent<CameraShake>();
        anim = startTab.GetComponentInChildren<Animator>();

        if (FindObjectOfType<AudioManager>().IsPlaying("MenuTheme"))
        {
            StartCoroutine(FindObjectOfType<AudioManager>().FadeOut("MenuTheme", .25f));
        }

        if (!FindObjectOfType<AudioManager>().IsPlaying("LevelTheme"))
        {
            StartCoroutine(FindObjectOfType<AudioManager>().FadeIn("LevelTheme", .5f));
        }

        endless = FindObjectOfType<EndlessManager>();
        totalDiamonds = GameObject.FindGameObjectsWithTag("Diamond").Length;
        totalSpecialDiamonds = GameObject.FindGameObjectsWithTag("SuperDiamond").Length;

        if (!Manager.Instance.soundOn)
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[0];
        else
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[1];
    }
    private void Update()
    {
        if (player.isReady && !gameOver && !paused)
        {
            diamondsText.text = diamonds.ToString();
        }
    }

    /// <summary>
    /// Loads the big button anim on start
    /// </summary>
    /// <param name="seconds">seconds that take</param>
    /// <returns></returns>
    private IEnumerator LoadPlayButton(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        startTab.SetActive(true);
    }
    private IEnumerator WaitThenDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        player.rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        player.gameObject.SetActive(false);
    }
    private IEnumerator WaitThenUnplay(float seconds, float minVolume)
    {
        AudioManager.instance.sounds[0].audioSource.volume = minVolume;
        yield return new WaitForSeconds(seconds);
        AudioManager.instance.sounds[0].audioSource.volume = .7f;
    }
    private IEnumerator ContinueGameOver(float sec)
    {
        yield return new WaitForSeconds(sec);

        if (isEndlessLevel)
            endless.OnGameOver();

        if (!SaveManager.Instance.data.firstGameOver)
        {
            print("This is the first gameover");
            //this is the first time we show gameover
            PlayServices.UnlockAchievement(GPGSIds.achievement_first_gameover);
            SaveManager.Instance.data.firstGameOver = true;
            SaveManager.Instance.Save();
        }

        if (Random.Range(0, 100) <= 45 && !AdsManager.Instance.interstitalLoaded)
        {
            print("Time to show some interstital ad at gameover");
            AdsManager.Instance.ShowInterstitalAd();
        }

        gameOver = true;
        uiTab.SetActive(false);
        gameOverTab.SetActive(true);
        AudioManager.instance.PlaySound("GameOverSound");
        StartCoroutine(WaitThenUnplay(2.2f, .2f));
        cameraController.Stop();
    }

    public void OnGameInit()
    {
        print("Game Started");
        anim.SetBool("isExiting", true);
        startTab.GetComponentInChildren<Image>().raycastTarget = false;
        pauseButton.enabled = true;    //enable the pause button

        player.isReady = true;  //give the player permission to move
        cameraController.Play(cameraController.cameraMovingSpeed, true);

        if (isEndlessLevel)
            endless.scoreIncreasing = true;
    }
    public void OnPauseClicked()
    {
        if (!gameOver)
        {
            player.paused = true;
            paused = true;
            pauseTab.SetActive(true);
            //player.rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            cameraController.Stop();
            Time.timeScale = 0;                 //freeze the time
            if (isEndlessLevel)
                endless.scoreIncreasing = false;
        }
    }
    public void OnResumeClicked()
    {
        if (!gameOver && paused)
        {
            player.paused = false;
            pauseTab.SetActive(false);
            //player.rigid.constraints = RigidbodyConstraints2D.None;
            cameraController.Play();
            Time.timeScale = 1;

            player.isGoingLeft = !player.isGoingLeft;   //this is for maintaining the player directions when resume the game
            player.isGoingRight = !player.isGoingRight;

            if (isEndlessLevel)
                endless.scoreIncreasing = true;
        }
    }
    public void OnHomeClicked()
    {
        Time.timeScale = 1;
        gameOver = true;

        if (isSnowLevel)
            SceneManager.LoadScene("LevelSelectorSnow");
        else
            SceneManager.LoadScene("LevelSelector");
    }
    public void OnSoundClicked()
    {
        Manager.Instance.OnSoundClick(soundButton);
    }
    public void OnNextClicked()
    {
        //so when we are at index 3 we load the scene named 3
        succesTab.SetActive(false);
        uiTab.SetActive(false);
        Manager.Instance.sceneIndex++;
        SceneManager.LoadScene(Manager.Instance.sceneIndex.ToString());      //I have named all the scenes with numbers like this (1 ,2, 3, ) etc
    }
    public void OnReplayClicked()
    {
        //reload the current scene
        SceneManager.LoadScene(Manager.Instance.sceneIndex.ToString());
        Time.timeScale = 1;
    }
    public void OnGameSucces()
    {
        cameraController.Play(.2f, false);
        AudioManager.instance.PlaySound("SuccesSound");
        StartCoroutine(WaitThenUnplay(2.2f, .2f));
        gameOver = true;
        succesTab.SetActive(true);
        uiTab.SetActive(false);
        highScoreDiamonds.text = string.Format("{0} / {1}", diamonds, totalDiamonds);
        highScoreSpecialDiamonds.text = string.Format("{0} / {1}", specialDiamonds, totalSpecialDiamonds);
        SaveManager.Instance.data.diamonds += diamonds;
        SaveManager.Instance.data.specialDiamond += specialDiamonds;


        if (isEndlessLevel)
        {
            EndlessManager endless = FindObjectOfType<EndlessManager>();
            PlayGamesPlatform.Instance.LoadScores(
             GPGSIds.leaderboard_blitexs_endless,
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
        (LeaderboardScoreData data) =>
        {
            if (data.Valid)
            {
                long total = data.PlayerScore.value + (long)endless.score;
                PlayServices.AddScoreToLeaderBoard(GPGSIds.leaderboard_blitexs_endless, total);
            }
        });
        }

        if (SaveManager.Instance.data.completedLevels == 0 && SaveManager.Instance.data.completedSnowLevels == 27)
        {
            //first level that is played
            PlayServices.UnlockAchievement(GPGSIds.achievement_first_level);
        }
        if (!isSnowLevel)
        {
            SaveManager.Instance.CompleteLevel(Manager.Instance.sceneIndex, false);

            if (SaveManager.Instance.data.completedLevels >= 15 && !SaveManager.Instance.data.hasUnlockedSnowTheme)
            {
                //player has completed 15 levels so now he unlocks snow theme
                print("Congrats u unlocked snow theme");
                PlayServices.UnlockAchievement(GPGSIds.achievement_unlocked_snow_theme);
                unlockedSnowTheme.SetActive(true);
                SaveManager.Instance.data.hasUnlockedSnowTheme = true;
                SaveManager.Instance.Save();
            }
        }

        else if (isSnowLevel)
        {
            print("It is a snow level and now calling complete level");
            SaveManager.Instance.CompleteLevel(Manager.Instance.sceneIndex, true);
        }

        PlayGamesPlatform.Instance.LoadScores(
             GPGSIds.leaderboard_blitexs_diamonds,
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
        (LeaderboardScoreData data) =>
        {
            if (data.Valid)
            {
                long total = data.PlayerScore.value + (long)diamonds;
                PlayServices.AddScoreToLeaderBoard(GPGSIds.leaderboard_blitexs_diamonds, total);
            }
        });

        PlayServices.IncrementAchievement(GPGSIds.achievement_100_diamonds, diamonds);
        PlayServices.IncrementAchievement(GPGSIds.achievement_1000_diamonds, diamonds);
        PlayServices.IncrementAchievement(GPGSIds.achievement_10_000_diamonds, diamonds);

        StartCoroutine(WaitThenDestroy(1.5f));
    }
    public void OnGameOver()
    {
        FindObjectOfType<CameraShake>().ShakeCamera();
        player.gameObject.SetActive(false);

        if (!instanciated)
        {
            FindObjectOfType<AudioManager>().PlaySound("BlastSound");
            Instantiate(Manager.Instance.hitVFX, player.transform.position, Quaternion.identity, transform);
            instanciated = true;
        }

        //wait 1 sec untill the shake has done playing then continue with game over tab
        StartCoroutine(ContinueGameOver(1f));
    }

    public void AddScoreToLeaderBoard(LevelType type)
    {
        if (type == LevelType.NormalLevels)
        {
            print("Adding snow score to leaderboard");
            PlayServices.IncrementAchievement(GPGSIds.achievement_completed_10_normal_levels, 1);
            PlayServices.IncrementAchievement(GPGSIds.achievement_completed_all_normal_levels, 1);
        }
        else if (type == LevelType.SnowLevels)
        {
            print("Adding snow score to leaderboard");
            PlayServices.IncrementAchievement(GPGSIds.achievement_completed_10_snow_levels, 1);
            PlayServices.IncrementAchievement(GPGSIds.achievement_completed_all_snow_levels, 1);
        }
        else if (type == LevelType.BlackAndWhite)
        {
            //TODO(Klendi): BW levels implement
        }
    }
    public void OnUnlockedSnowExit()
    {
        unlockedSnowTheme.SetActive(false);
    }
}
