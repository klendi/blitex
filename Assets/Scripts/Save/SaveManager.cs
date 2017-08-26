/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       25/8/2017. 10:49
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
    public SaveData data;   //this is the instace that we are saving all the time, this is where we make the changes

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    public void UnlockBall(int index)
    {
        data.ballOwned |= 1 << index;
    }

    /// <summary>
    /// This completes the level at specific index
    /// </summary>
    /// <param name="index">The index from where to complete</param>
    /// <param name="isSnowLevel">If this is true then it will unlock from the snows levels, if false it will load from normal levels</param>
    public void CompleteLevel(int index, bool isSnowLevel)
    {
        if (!isSnowLevel)
        {
            if (data.completedLevels == index)
            {
                FindObjectOfType<LevelManager>().AddScoreToLeaderBoard(LevelType.NormalLevels);
                data.completedLevels++;
                Save();
            }
        }
        else if (isSnowLevel)
        {
            if (data.completedSnowLevels == index)
            {
                FindObjectOfType<LevelManager>().AddScoreToLeaderBoard(LevelType.SnowLevels);
                data.completedSnowLevels++;
                Save();
            }
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }

    /// <summary>
    /// Return true if player can buy the ball with selected cost
    /// </summary>
    /// <param name="index">the ball index</param>
    /// <param name="cost">the cost of the ball</param>
    /// <returns></returns>
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
