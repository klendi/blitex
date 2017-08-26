/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;

public class PlayServices : MonoBehaviour
{
    public static PlayServices Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Debug.Log("Activated Services");
            SignIn();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SignIn()
    {
        Debug.Log("Trying to sign in");

        Social.localUser.Authenticate(succes =>
        {
            if (succes)
            {
                Debug.Log("Succes loggin the player");
            }
            else if (!succes)
            {
                Debug.Log("Error logging the player");
            }
        });
    }

    #region Achievements

    public static void UnlockAchievement(string id)
    {
        Social.ReportProgress(id, 100, succes =>
         {

         });
    }

    public static void IncrementAchievement(string id, int stepsToIncrement)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, succes =>
         {

         });
    }

    public static void ShowAchievementsUI()
    {
        Debug.Log("Show Achievement called");
        Social.ShowAchievementsUI();
    }
    #endregion

    #region LeaderBoard

    public static void AddScoreToLeaderBoard(string leaderBoardID, long score)
    {
        Social.ReportScore(score, leaderBoardID, succes =>
        {

        });
    }
    public static void ShowLeaderBoardUI()
    {
        Debug.Log("Show LeaderBoard called");
        Social.ShowLeaderboardUI();
    }
    #endregion
}
