using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [Header("Constants")]
    public float diamondThreshold = 30;
    private int platformIndex = 0;
    private float[] platform_two_fav_pos = { -5.24f, -1.6f, -2f, -4.73f };
    private float[] scaling_duo_fav_pos = { .3f, .7f, -1f, -1.6f };
    private float[] left_right_fav_pos = { -2.5f, -3f, -1.85f };
    private float[] scaling_one_fav_pos = { 0.0f, 1.72f, -1.6f };

    [Header("Attachables")]
    public Transform constructionPoint;
    public ObjectPooling[] thePoolingPlatforms;
    private DiamondGenerator diamondGen;

    private void Start()
    {
        diamondGen = FindObjectOfType<DiamondGenerator>();
    }


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
                    int k = Random.Range(0, left_right_fav_pos.Length);
                    newPlatform.transform.position = new Vector3(left_right_fav_pos[k], transform.position.y - .43f);

                    if (Random.Range(0, 100) < diamondThreshold)
                        diamondGen.SpawnDiamond(new Vector3(Random.Range(-2f, 2f), newPlatform.transform.position.y + .43f + .5f));

                    break;
                case "platform_one":
                    int p = Random.Range(0, scaling_one_fav_pos.Length);
                    newPlatform.transform.position = new Vector3(scaling_one_fav_pos[p], transform.position.y);

                    if (Random.Range(0, 100) < diamondThreshold)
                        diamondGen.SpawnDiamond(new Vector3(Random.Range(-2f, 2f), newPlatform.transform.position.y + .5f));

                    break;
                case "platform_two":
                    int i = Random.Range(0, platform_two_fav_pos.Length);
                    newPlatform.transform.position = new Vector3(platform_two_fav_pos[i], transform.position.y);

                    if (Random.Range(0, 100) < diamondThreshold)
                        diamondGen.SpawnDiamond(new Vector3(Random.Range(-2f, 2f), newPlatform.transform.position.y + .5f));

                    break;
                case "scaling_duo":
                    int j = Random.Range(0, scaling_duo_fav_pos.Length);
                    newPlatform.transform.position = new Vector3(scaling_duo_fav_pos[j], transform.position.y - .43f);

                    if (Random.Range(0, 100) < diamondThreshold)
                        diamondGen.SpawnDiamond(new Vector3(Random.Range(-2f, 2f), newPlatform.transform.position.y + .5f + .43f));

                    break;
                case "scaling_trio":
                    newPlatform.transform.position = new Vector3(-0.2f, transform.position.y - .43f);

                    if (Random.Range(0, 100) < diamondThreshold)
                        diamondGen.SpawnDiamond(new Vector3(Random.Range(-2f, 2f), newPlatform.transform.position.y + .5f + .43f));
                    break;
                case "scaling_one":
                    int o = Random.Range(0, scaling_one_fav_pos.Length);
                    newPlatform.transform.position = new Vector3(scaling_one_fav_pos[o], transform.position.y);

                    if (Random.Range(0, 100) < diamondThreshold)
                        diamondGen.SpawnDiamond(new Vector3(Random.Range(-2f, 2f), newPlatform.transform.position.y + .5f));
                    break;

                default:
                    Debug.LogWarning("the object doesnt have a tag that is declared at switch function");
                    break;
            }

            newPlatform.SetActive(true);
        }
    }
}
