using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    GameObject playerToSpawn;
    public Transform spawnPoint;
    public float startingSpeed = 5f;
    public bool startingRight = false;
    public Material bwMaterial;
    public bool isBwLevel = false;

    private void Start()
    {
        playerToSpawn = Manager.Instance.playerPrefabs[SaveManager.Instance.data.activeBall];
        GameObject player = Instantiate(playerToSpawn, spawnPoint.position, Quaternion.identity);
        player.GetComponent<PlayerController>().speed = startingSpeed;

        if (isBwLevel)
            player.GetComponent<Renderer>().material = bwMaterial;

        if (startingRight)
        {
            player.GetComponent<PlayerController>().isGoingLeft = false;
            player.GetComponent<PlayerController>().isGoingRight = true;
        }
        else
        {
            player.GetComponent<PlayerController>().isGoingLeft = true;
            player.GetComponent<PlayerController>().isGoingRight = false;
        }
    }
}
