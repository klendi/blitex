using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    public GameObject platformDestroyerPoint;
    private void Start()
    {
        platformDestroyerPoint = GameObject.Find("PlatformDestroyerPoint");
    }
    private void Update()
    {
        if (transform.position.y > platformDestroyerPoint.transform.position.y)
        {
            gameObject.SetActive(false);
        }
    }
}
