using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    public float coins, diamonds;
    float screenHalfInWorldUnits;

    [Header("Attachments")]
    public Text coinsText;
    public GameObject pauseTab;
    public Button pauseButton;
    public GameObject startTab;
    public GameObject startTabExit;
    public Button startGameButton;

    private Rigidbody2D rigid;
    private Animator animator;

    private bool isReady = false;
    private bool isGoingLeft = true;
    private bool isGoingRight = false;   //in the right side the values are positive
    private bool gameOver = false;
    private bool paused = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = startGameButton.GetComponent<Animator>();
        StartCoroutine(Load(0.2f, true));

        pauseTab.SetActive(false);
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

        //is moving to left ? then set the direction to left and don't stop
        else if (isGoingLeft && isReady && !paused)
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
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

    public void OnGameInit()
    {
        print("Game Started");
        StartCoroutine(Load(0f, false));
        pauseButton.enabled = true;
        isReady = true;
        CameraController.Play();
    }
    public void OnPauseClicked()
    {
        if (!gameOver)
        {
            paused = true;
            pauseTab.SetActive(true);
            CameraController.Stop();
            Time.timeScale = 0;
        }
    }
    public void OnResumeClicked()
    {
        if (!gameOver && paused)
        {
            paused = false;
            pauseTab.SetActive(false);
            CameraController.Play();
            Time.timeScale = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Diamond")
        {
            diamonds++;
            print("Its a diamond");
            col.gameObject.SetActive(false);
        }
        if (col.tag == "Coin")
        {
            coins++;
            print("Its a Coin");
            col.gameObject.SetActive(false);
        }
        if (col.tag == "Final")
        {
            gameOver = true;
            CameraController.Stop();
            print("Succes");
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            gameOver = true;
            CameraController.Stop();
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            print("Game over by Spikes");
        }
    }
    private void OnBecameInvisible()
    {
        if (!gameOver)
        {
            print("Game over");
            gameOver = true;
            CameraController.Stop();
            gameObject.SetActive(false);
        }
    }
}
