using UnityEngine;

public class BladeSpikes : MonoBehaviour
{
    public float speed = 2f;
    public float range;
    public bool reverse;
    [Tooltip("If this is false than raycast is diabled and spike dont move just spin")]
    public bool canMove = false;
    [Tooltip("If we got the player raycasted")]
    bool detected = false;
    [Tooltip("If you select is right TRUE then spike gonna check for raycast on the right, if FALSE then in the left")]
    public bool isRight = false;
    public float rotatingSpeed = 4f;
    public RotatingPlatform rotating;
    public Transform pointToRaycast;
    Vector3 rayOrigin;
    LevelManager level;

    private void Awake()
    {
        level = FindObjectOfType<LevelManager>();
        rayOrigin = pointToRaycast.position;
        rotating.loopingMode = true;
    }

    void Update()
    {

        if (reverse && detected)
            rotating.speed = rotatingSpeed;
        else if (!reverse && detected)
            rotating.speed = -rotatingSpeed;

        if (detected && !isRight)
            transform.Translate(Vector3.left * speed * Time.deltaTime);

        else if (detected && isRight)
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (canMove && isRight)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, range);
            Debug.DrawRay(rayOrigin, Vector3.right * range, Color.red);

            if (hit)
            {
                if (hit.collider.tag == "Player")
                {
                    Debug.DrawRay(rayOrigin, Vector3.right * range, Color.green);
                    detected = true;
                    //transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
            }
        }
        if (canMove && !isRight)      //we use this for left
        {
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.left, range);
            Debug.DrawRay(rayOrigin, Vector3.left * range, Color.red);

            if (hit)
            {
                if (hit.collider.tag == "Player")
                {
                    Debug.DrawRay(rayOrigin, Vector3.left * range, Color.green);
                    detected = true;
                }
            }
        }
    }


    void OnCollision2D(Collision2D col)
    {
        if (col.collider.tag == "Player")
        {
            print("Gameover by bladeSpike");
            level.OnGameOver();
        }
        else if (col.collider.tag == "Diamond")
        {
            print("Blade Destroyed Diamond");
            Destroy(col.gameObject);
        }
        else if(col.collider.tag == "SuperDiamond")
        {
            print("Blade Destroyed SuperDiamond");
            Destroy(col.gameObject);
        }
        else if(col.collider.tag == "Enemy")
        {
            print("Blade Destroyed Spike");
            Destroy(col.gameObject);
        }
    }
}
