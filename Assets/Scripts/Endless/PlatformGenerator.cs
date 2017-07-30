using UnityEngine;

public enum PlatformType
{
    normalOne,
    normalTwo,
    scalingTwo,
    scalingTrio,
    leftRightMoving
};

public class PlatformGenerator : MonoBehaviour
{
    [Header("Constants")]
    public float cameraSpeed = .8f;
    public float playerSpeed = 3f;
    public float trapsSpeed = .5f;
    private int platformIndex = 0;

    [Header("Min Max")]
    public Vector2 platform_one_min_max;
    public Vector2 platform_two_min_max;
    public Vector2 left_right_movement_min_max;
    public Vector2 scaling_duo_min_max;
    public Vector2 scaling_trio_min_max;
    public Vector2 scaling_one_min_max;
    public Vector2 trapsLeft, trapsRight;

    [Header("Attachables")]
    public Transform constructionPoint;
    public ObjectPooling[] thePoolingPlatforms;

    private float[] platform_two_fav_pos = { -5.24f, -1.6f, -2f, -3.65f, -4.73f };

    private void Update()
    {
        if (transform.position.y > constructionPoint.position.y)
        {
            platformIndex = Random.Range(0, thePoolingPlatforms.Length);

            transform.position = new Vector3(Random.Range(-1f, 2f), transform.position.y - 1, transform.position.z);

            GameObject newPlatform = thePoolingPlatforms[platformIndex].GetPoolObject();

            switch (newPlatform.tag)
            {
                case "left_right":
                    newPlatform.transform.position = new Vector3(Random.Range(left_right_movement_min_max.x, left_right_movement_min_max.y), newPlatform.transform.position.y);
                    break;
                case "platform_one":
                    newPlatform.transform.position = new Vector3(Random.Range(platform_one_min_max.x, platform_one_min_max.y), newPlatform.transform.position.y);
                    break;
                case "platform_two":
                    int i = Random.Range(0, platform_two_fav_pos.Length);
                    //newPlatform.transform.position = new Vector3(Random.Range(platform_two_min_max.x, platform_two_min_max.y), newPlatform.transform.position.y);
                    newPlatform.transform.position = new Vector3(platform_two_fav_pos[i], 0f);
                    break;
                case "scaling_duo":
                    newPlatform.transform.position = new Vector3(Random.Range(scaling_duo_min_max.x, scaling_duo_min_max.y), newPlatform.transform.position.y);
                    break;
                case "scaling_trio":
                    newPlatform.transform.position = new Vector3(Random.Range(scaling_trio_min_max.x, scaling_trio_min_max.y), newPlatform.transform.position.y);
                    break;
                case "scaling_one":
                    newPlatform.transform.position = new Vector3(Random.Range(scaling_one_min_max.x, scaling_one_min_max.y), newPlatform.transform.position.y);
                    break;

                default:
                    Debug.LogWarning("the object doesnt have a tag that is declared at switch function");
                    break;
            }
            newPlatform.transform.position = new Vector3(newPlatform.transform.position.x, transform.position.y);
            newPlatform.SetActive(true);
        }
    }
}
