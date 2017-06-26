using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
    public SaveData data;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //ResetSave();
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize(data));
    }
    public void Load()
    {
        //check if we have a saved data
        if (PlayerPrefs.HasKey("save"))
        {
            data = Helper.DeSerialize<SaveData>(PlayerPrefs.GetString("save"));
            print("Loaded Succesfully");
        }
        else
        {
            data = new SaveData();
            Save();
            print("No save data found, Creating a new one!");
        }
    }

    /// <summary>
    /// Return true or false if the player has bought the ball
    /// </summary>
    /// <param name="index">the index to check the bit</param>
    /// <returns></returns>
    public bool DoesOwnBall(int index)
    {
        //check if the bit is set, so the ball is owned
        return (data.ballOwned & (1 << index)) != 0;
    }
    public bool hasUnlockedLevel(int index)
    {
        //check if the bit is set, so the level is owned
        return (data.completedLevels & (1 << index)) != 0;
    }
    public void UnlockLevel(int index)
    {
        //Toggle on the bit at selected index, so we can detect later
        data.completedLevels |= 1 << index;
    }
    public void UnlockBall(int index)
    {
        data.ballOwned |= 1 << index;
    }
    public void CompleteLevel(int index)
    {
        if (data.completedLevels == index)
        {
            data.completedLevels++;
            Save();
        }
    }
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }


    public bool BuyBall(int index, int cost)
    {
        //we can afford it, buy it bitch
        if (data.diamonds >= cost)
        {
            //:c oh shit my money
            data.diamonds -= cost;
            UnlockBall(index);

            Save();
            return true;
        }
        //we don't have money :c so exit
        else
            return false;
    }
}
