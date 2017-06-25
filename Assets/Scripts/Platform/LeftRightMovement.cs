using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    public float speed;
    public float range;
    float timeCounter = 0;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        timeCounter += Time.deltaTime * speed;
        transform.position = startPos + new Vector3(Mathf.Sin(timeCounter) * range, 0f);
    }
}