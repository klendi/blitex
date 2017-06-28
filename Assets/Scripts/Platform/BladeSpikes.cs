using UnityEngine;

public enum Direction
{
    right,
    left
};

public class BladeSpikes : MonoBehaviour
{
    public float speed = 2f;
    public float rotatingSpeed = 4f;
    public Direction dirToGo = Direction.right;
    public GameObject bladeSpike;
    public RotatingPlatform rotating;

    void Start()
    {
    }

    void OnCollision2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (dirToGo == Direction.right)
            {
                bladeSpike.transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
            else if (dirToGo == Direction.left)
            {
                bladeSpike.transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
        }
    }
}
