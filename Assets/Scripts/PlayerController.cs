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
    public float score;

    private Rigidbody2D rigid;
    public Text scoreText;
    public GameColor currentColor;

    private bool isReady = false;
    private bool isGoingRight = false;   //in the right side the values are positive
    private bool succes = false;

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
        if (isReady && !succes)
        {
            score = Time.time / 8;
            scoreText.text = Mathf.Round(score).ToString();
        }

        //init the game
        if (Input.GetMouseButtonDown(0) && !isReady)
        {
            isReady = true;
        }

        //If the player goes on the left too much then he spawn to other side
        if (transform.position.x < -screenHalfInWorldUnits)
        {
            rigid.gravityScale = 0;
            transform.position = new Vector3(screenHalfInWorldUnits, transform.position.y);
            rigid.gravityScale = 1;
        }

        //Vice-Versa
        else if (transform.position.x > screenHalfInWorldUnits)
        {
            rigid.gravityScale = 0;
            transform.position = new Vector3(-screenHalfInWorldUnits, transform.position.y);
            rigid.gravityScale = 1;
        }

        //Move the object to the left by adding speed to player
        if (Input.GetMouseButtonDown(0) && isReady && !isGoingRight)
            isGoingRight = true;

        //Move the object to the right by adding speed to player
        else if (Input.GetMouseButtonDown(0) && isReady && isGoingRight)
            isGoingRight = false;


        //is moving to right ? then set the direction to right and don't stop
        if (isGoingRight && isReady)
            rigid.velocity = new Vector2(speed, rigid.velocity.y);

        //is moving to left ? then set the direction to left and don't stop
        if (!isGoingRight && isReady)
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
    }

    //Detect our final platform
    private void OnCollisionEnter2D(Collision2D col)
    {
        //If the last collider is the one as we set as final then succes
        if (col.gameObject.tag == "Final")
        {
            //Succes , end the game
            isReady = false;
            succes = true;
            CameraController.cameraMovingSpeed = 0f;
            print("Game over by final");
        }
        if (col.gameObject.tag == "Enemy")
        {
            //Succes , end the game
            isReady = false;
            succes = true;
            CameraController.cameraMovingSpeed = 0f;
            print("Game over by Spikes");
        }
    }

    private void OnBecameInvisible()
    {
        print("Game over");
        CameraController.cameraMovingSpeed = 0f;
        gameObject.SetActive(false);
    }
}
