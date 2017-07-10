using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    void Awake()
    {
        if (Instance == null)         //if already is a game object then destroy this and keep that
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        AudioManager.instance.PlaySound("Theme");
    }
}
