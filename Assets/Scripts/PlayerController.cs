using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameColor
{
    Red,
    Purple,
    Yellow,
    Cyan
};

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    float screenHalfInWorldUnits;
    public float coins, diamonds;

    private Rigidbody2D rigid;
    public Text coinsText;
    public Text diamondText;
    public GameColor currentColor;

    private bool isReady = false;
    private bool isGoingLeft = false;    //This make system more stable
    private bool isGoingRight = true;   //in the right side the values are positive
    private bool gameOver = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentColor = GameColor.Purple;

        switch (gameObject.tag)
        {
            case "Cyan":
                currentColor = GameColor.Cyan;
                break;
            case "Purple":
                currentColor = GameColor.Purple;
                break;
            case "Yellow":
                currentColor = GameColor.Yellow;
                break;
            case "Red":
                currentColor = GameColor.Red;
                break;
            default:
                currentColor = GameColor.Purple;
                break;
        }

        float halfPlayerWidth = transform.localScale.x / 2f;
        screenHalfInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize + halfPlayerWidth;
    }

    private void Update()
    {
        //if game is started and isn't over then count the score
        if (isReady && !gameOver)
        {
            coinsText.text = Mathf.Round(coins).ToString();
            diamondText.text = Mathf.Round(diamonds).ToString();
        }

        //init the game
        if (Input.GetMouseButtonDown(0) && !isReady)
        {
            isReady = true;
            CameraController.isReady = true;
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
        if (Input.GetMouseButtonDown(0) && isReady && isGoingLeft)
        {
            //if is going left then change the direction to right
            isGoingRight = true;
            isGoingLeft = false;
        }
        //Move the object to the right by adding speed to player
        else if (Input.GetMouseButtonDown(0) && isReady && isGoingRight)
        {
            //if is going right then change the direction to left
            isGoingRight = false;
            isGoingLeft = true;
        }
        //is moving to right ? then set the direction to right and don't stop
        if (isGoingRight && isReady)
            rigid.velocity = new Vector2(speed, rigid.velocity.y);

        //is moving to left ? then set the direction to left and don't stop
        if (isGoingLeft && isReady)
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
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
            isReady = false;
            gameOver = true;
            CameraController.cameraMovingSpeed = 0f;
            CameraController.isReady = false;
            isGoingLeft = false;
            isGoingRight = false;
            print("Succes");
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            //Succes , end the game
            isReady = false;
            gameOver = true;
            CameraController.cameraMovingSpeed = 0f;
            CameraController.isReady = false;
            isGoingLeft = false;
            isGoingRight = false;
            print("Game over by Spikes");
        }
    }

    private void OnBecameInvisible()
    {
        if (!gameOver)
        {
            print("Game over");
            CameraController.cameraMovingSpeed = 0f;
            gameObject.SetActive(false);
        }
    }
}
