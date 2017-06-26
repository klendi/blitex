using UnityEngine;

public enum MovementType
{
    LeftToRight,
    RightToLeft
}

public class LeftRightMovement : MonoBehaviour
{
    public float speed;
    public float range;
    public MovementType movementType = MovementType.RightToLeft;
    float timeCounter = 0;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (movementType == MovementType.RightToLeft)
        {
            timeCounter += Time.deltaTime * speed;
            transform.position = startPos + new Vector3(Mathf.Sin(timeCounter) * range, 0f);
        }
        else if(movementType == MovementType.LeftToRight)
        {
            timeCounter += Time.deltaTime * speed;
            transform.position = startPos + new Vector3(-Mathf.Sin(timeCounter) * range, 0f);
        }
    }
}