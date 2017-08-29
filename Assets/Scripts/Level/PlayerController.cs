/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    float screenHalfInWorldUnits;

    [HideInInspector]
    public Rigidbody2D rigid;

    LevelManager level;

    [HideInInspector]
    public bool isGoingLeft = true, isGoingRight = false, isReady = false, gameOver = false, paused = false;


    private void Start()
    {
        level = FindObjectOfType<LevelManager>();
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
        {
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        }

        //is moving to left ? then set the direction to left and don't stop
        else if (isGoingLeft && isReady && !paused)
        {
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Diamond")
        {
            Instantiate(Manager.Instance.diamondHitFx, col.gameObject.transform.position, Quaternion.identity);
            col.gameObject.SetActive(false);
            level.diamonds++;
            AudioManager.instance.PlaySound("DiamondCollect");
        }
        else if (col.tag == "SuperDiamond")
        {
            Instantiate(Manager.Instance.redDiamondHitFx, col.gameObject.transform.position, Quaternion.identity);
            col.gameObject.SetActive(false);
            level.specialDiamonds++;
            AudioManager.instance.PlaySound("DiamondCollect");
        }
        else if (col.tag == "Final")
        {
            level.OnGameSucces();
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && !gameOver)
        {
            gameOver = true;
            level.OnGameOver();
        }
    }
    private void OnBecameInvisible()
    {
        if (!level.gameOver)
        {
            level.OnGameOver();
        }
    }
}
