using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; set; }

    [Header("Variables")]
    public float speed;
    float screenHalfInWorldUnits;

    [HideInInspector]
    public Rigidbody2D rigid;

    [HideInInspector]
    public bool isGoingLeft = true;
    [HideInInspector]
    public bool isGoingRight = false;
    [HideInInspector]                   //in the right side the values are positive
    public bool isReady = false;     //is this is false game cant start
    [HideInInspector]
    public bool gameOver = false;
    [HideInInspector]
    public bool paused = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        float halfPlayerWidth = transform.localScale.x / 2f;
        screenHalfInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize + halfPlayerWidth;
    }
    private void Update()
    {

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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Diamond")
        {
            col.gameObject.SetActive(false);
            LevelManager.Instance.diamonds++;
            LevelManager.Instance.PlaySound(LevelManager.Instance.sounds[1]);
        }
        if (col.tag == "Final")
        {
            LevelManager.Instance.OnGameSucces();
            LevelManager.Instance.gameOver = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            LevelManager.Instance.OnGameOver();
        }
    }
    private void OnBecameInvisible()
    {
        if (!LevelManager.Instance.gameOver)
        {
            LevelManager.Instance.OnGameOver();
        }
    }
}
