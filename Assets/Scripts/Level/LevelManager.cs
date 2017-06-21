using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int diamonds = 0;


    [Header("Attachments")]
    PlayerController player;
    CameraController cameraController;
    public Button pauseButton;
    public Button soundButton;
    public Button startGameButton;
    public Text highScoreDiamonds;
    public Text diamondsText;
    public GameObject pauseTab;
    public GameObject succesTab;
    public GameObject soundManager;
    public GameObject uiTab;
    public GameObject startTab;
    public GameObject gameOverTab;
    public GameObject startTabExit;
    public GameObject totalDiamonds;  //we find the total number of diamonds counting his childs
    public AudioClip[] sounds;
    public Sprite[] soundSprites;

    private AudioSource gameAudio;

    private bool soundOn = false;   //dont change this , there is a reason i left this that way
    private bool isReady = false;

    [HideInInspector]
    public bool paused = false, gameOver = false;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(Load(0.2f, true));    //load the big play button when the level loads, play that after 0.2 seconds
        gameAudio = soundManager.GetComponent<AudioSource>();
        gameAudio.Play();                    //play the soundtrack
        pauseTab.SetActive(false);           //set the pause tab false
        succesTab.SetActive(false);         //set the succes tab false
        gameOverTab.SetActive(false);       //set the game over tab false
        uiTab.SetActive(true);              //set the ui active
        pauseButton.enabled = false;
    }

    private void Update()
    {
        if (isReady && !gameOver && !paused)
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


    public void OnGameInit()
    {
        print("Game Started");
        //remove the big play button after he is pressed
        StartCoroutine(Load(0f, false));
        pauseButton.enabled = true;    //enable the pause button
        isReady = true;             //give the player permission to move
        player.isReady = true;
        cameraController.Play(cameraController.cameraMovingSpeed, true);
    }
    public void PlaySound(AudioClip clip)
    {
        soundManager.GetComponent<AudioSource>().clip = clip;
        soundManager.GetComponent<AudioSource>().Play();
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
        }
    }
    public void OnHomeClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void OnSoundClicked()
    {
        soundOn = !soundOn;
        AudioListener.pause = soundOn;

        if (soundOn)
        {
            soundButton.GetComponent<Image>().sprite = soundSprites[0];
        }
        if (!soundOn)
        {
            soundButton.GetComponent<Image>().sprite = soundSprites[1];
        }
    }
    public void OnNextClicked()
    {
        Manager.Instance.sceneIndex++;   //if next is clicked , on the object that never is destroyed set the scene index to += 1
        SceneManager.LoadScene(Manager.Instance.sceneIndex.ToString());      //I have named all the scenes with numbers like this (1 ,2, 3, ) etc
        //so when we are at index 3 we load the scene named 3
    }
    public void OnReplayClicked()
    {
        SceneManager.LoadScene(Manager.Instance.sceneIndex.ToString());
        //load the current scene
    }
    public void OnGameSucces()
    {
        cameraController.Play(.2f, false);
        gameOver = true;
        succesTab.SetActive(true);
        uiTab.SetActive(false);
        highScoreDiamonds.text = string.Format("{0} / {1}", diamonds, totalDiamonds.transform.childCount);
        SaveManager.Instance.data.diamonds += diamonds;
        StartCoroutine(WaitThenDestroy(1.5f));
    }
    public void OnGameOver()
    {
        gameOver = true;
        uiTab.SetActive(false);
        gameOverTab.SetActive(true);
        cameraController.Stop();
        //play the blowing particles
        player.gameObject.SetActive(false);
    }
}
