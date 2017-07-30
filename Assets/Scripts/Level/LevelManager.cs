using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    #region variables
    [Header("Diamonds")]
    public int diamonds = 0;
    int totalDiamonds = 0, totalSpecialDiamonds = 0;
    public int specialDiamonds = 0;
    [Tooltip("If level is of snow theme then use this activate this")]
    public bool isSnowLevel = false;
    public bool isEndlessLevel = false;
    [HideInInspector]
    public bool paused = false, gameOver = false;

    [Header("Attachments")]
    PlayerController player;
    CameraController cameraController;
    EndlessManager endless;
    public Button pauseButton;
    public Button soundButton;
    public Button startGameButton;
    public Text highScoreDiamonds, highScoreSpecialDiamonds;
    public Text diamondsText;
    public GameObject pauseTab;
    public GameObject succesTab;
    public GameObject uiTab;
    public GameObject startTab;
    public GameObject gameOverTab;
    public GameObject startTabExit;
    public Sprite[] soundSprites;
    #endregion


    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(Load(0.2f, true));    //load the big play button when the level loads, play that after 0.2 seconds
        pauseTab.SetActive(false);           //set the pause tab false
        succesTab.SetActive(false);         //set the succes tab false
        gameOverTab.SetActive(false);       //set the game over tab false
        uiTab.SetActive(true);              //set the ui active
        pauseButton.enabled = false;
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

    private IEnumerator Load(float seconds, bool entering)
    {
        if (entering)
        {
            yield return new WaitForSeconds(seconds);
            startTab.SetActive(true);
        }
        else if (!entering)
        {
            yield return new WaitForSeconds(seconds);
            startTab.SetActive(false);
            startTabExit.SetActive(true);
        }
    }
    private IEnumerator WaitThenDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        player.rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        player.gameObject.SetActive(false);
    }
    private IEnumerator WaitThenUnplay(float seconds, float minVolume)
    {
        AudioManager.instance.sounds[0].source.volume = minVolume;
        yield return new WaitForSeconds(seconds);
        AudioManager.instance.sounds[0].source.volume = .7f;
    }

    public void OnGameInit()
    {
        print("Game Started");
        //remove the big play button after he is pressed
        StartCoroutine(Load(0f, false));
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
            player.rigid.constraints = RigidbodyConstraints2D.FreezeAll;
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
            player.rigid.constraints = RigidbodyConstraints2D.None;
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
        SceneManager.LoadScene("Main Menu");
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

        if (!isSnowLevel)
            SaveManager.Instance.CompleteLevel(Manager.Instance.sceneIndex, false);

        else if (isSnowLevel)
            SaveManager.Instance.CompleteLevel(Manager.Instance.sceneIndex, true);

        StartCoroutine(WaitThenDestroy(1.5f));
    }
    public void OnGameOver()
    {
        if (isEndlessLevel)
            endless.OnGameOver();

        gameOver = true;
        uiTab.SetActive(false);
        gameOverTab.SetActive(true);
        AudioManager.instance.PlaySound("GameOverSound");
        StartCoroutine(WaitThenUnplay(2.2f, .2f));
        cameraController.Stop();
        //play the blowing particles
        player.gameObject.SetActive(false);
    }
}
