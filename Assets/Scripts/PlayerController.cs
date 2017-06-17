using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    public float coins, diamonds;
    float screenHalfInWorldUnits;

    [Header("Attachments")]
    public Button pauseButton;
    public Button soundButton;
    public Button startGameButton;
    public Text highScoreCoins;
    public Text coinsText;
    public Text highScoreDiamonds;
    public GameObject pauseTab;
    public GameObject succesTab;
    public GameObject soundManager;
    public GameObject uiTab;
    public GameObject startTab;
    public GameObject gameOverTab;
    public GameObject startTabExit;
    public GameObject totalCoins, totalDiamonds;
    public AudioClip[] sounds; //first one is coins sound then diamonds
    public Sprite[] soundSprites;

    private Rigidbody2D rigid;
    private AudioSource gameAudio;

    private bool isReady = false;
    private bool isGoingLeft = true;
    private bool isGoingRight = false;   //in the right side the values are positive
    private bool gameOver = false;
    private bool paused = false;
    private bool soundOn = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(Load(0.2f, true));
        gameAudio = soundManager.GetComponent<AudioSource>();
        gameAudio.Play();
        pauseTab.SetActive(false);
        succesTab.SetActive(false);
        gameOverTab.SetActive(false);
        uiTab.SetActive(true);
        pauseButton.enabled = false;

        float halfPlayerWidth = transform.localScale.x / 2f;
        screenHalfInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize + halfPlayerWidth;
    }
    private void Update()
    {

        //if game is started and isn't over then count the score
        if (isReady && !gameOver && !paused)
        {
            coinsText.text = Mathf.Round(coins).ToString();
        }

        //If the player goes on the left too much then he spawn to other side
        if (transform.position.x < -screenHalfInWorldUnits)
        {
            transform.position = new Vector3(screenHalfInWorldUnits, transform.position.y);
        }

        //Vice-Versa
        else if (transform.position.x > screenHalfInWorldUnits)
        {
            transform.position = new Vector3(-screenHalfInWorldUnits, transform.position.y);
        }
        //Move the object to the left by adding speed to player
        if (Input.GetMouseButtonDown(0) && isReady && isGoingLeft && !paused)
        {
            //if is going left then change the direction to right
            isGoingRight = true;
            isGoingLeft = false;
        }
        //Move the object to the right by adding speed to player
        else if (Input.GetMouseButtonDown(0) && isReady && isGoingRight && !paused)
        {
            //if is going right then change the direction to left
            isGoingRight = false;
            isGoingLeft = true;
        }
        //is moving to right ? then set the direction to right and don't stop
        if (isGoingRight && isReady && !paused)
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
            //transform.Translate(Vector2.right * speed * Time.deltaTime);


        //is moving to left ? then set the direction to left and don't stop
        else if (isGoingLeft && isReady && !paused)
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
            //transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void PlaySound(AudioClip clip)
    {
        soundManager.GetComponent<AudioSource>().clip = clip;
        soundManager.GetComponent<AudioSource>().Play();
    }
    //time to wait when we load the play button at start
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

    public void OnGameInit()
    {
        print("Game Started");
        StartCoroutine(Load(0f, false));
        pauseButton.enabled = true;
        isReady = true;
        CameraController.Instance.Play(CameraController.Instance.cameraMovingSpeed, true);
    }
    public void OnPauseClicked()
    {
        if (!gameOver)
        {
            paused = true;
            pauseTab.SetActive(true);
            CameraController.Instance.Stop();
            Time.timeScale = 0;
        }
    }
    public void OnResumeClicked()
    {
        if (!gameOver && paused)
        {
            paused = false;
            pauseTab.SetActive(false);
            CameraController.Instance.Play();
            Time.timeScale = 1;
            isGoingLeft = !isGoingLeft;
            isGoingRight = !isGoingRight;
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
        Manager.Instance.sceneIndex++;
        SceneManager.LoadScene(Manager.Instance.sceneIndex.ToString());
    }
    public void OnReplayClicked()
    {
        //load the current scene which is 1
        SceneManager.LoadScene(Manager.Instance.sceneIndex.ToString());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Diamond")
        {
            diamonds++;
            PlaySound(sounds[1]);
            col.gameObject.SetActive(false);
        }
        if (col.tag == "Coin")
        {
            coins++;
            PlaySound(sounds[0]);
            col.gameObject.SetActive(false);
        }
        if (col.tag == "Final")
        {
            OnGameSucces();
            gameOver = true;
        }
    }
    private void OnGameSucces()
    {
        gameOver = true;
        succesTab.SetActive(true);
        uiTab.SetActive(false);
        gameObject.SetActive(false);
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        highScoreCoins.text = string.Format("{0}/{1}", coins, totalCoins.transform.childCount);
        highScoreDiamonds.text = string.Format("{0} / {1}", diamonds, totalDiamonds.transform.childCount);
        Manager.Instance.coins += (int)coins;
        Manager.Instance.diamonds += (int)diamonds;
        CameraController.Instance.Play(.5f,false);
    }
    private void OnGameOver()
    {
        gameOver = true;
        uiTab.SetActive(false);
        gameOverTab.SetActive(true);
        CameraController.Instance.Stop();
        //play the blowing particles
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            OnGameOver();
        }
    }
    private void OnBecameInvisible()
    {
        if (gameOver == false)
        {
            OnGameOver();
        }
    }
}
