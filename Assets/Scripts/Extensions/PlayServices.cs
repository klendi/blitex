using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;

public class PlayServices : MonoBehaviour
{
    private void Awake()
    {
        PlayGamesClientConfiguration config = new
            PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        SignIn();
    }

    public void SignIn()
    {
        Social.localUser.Authenticate(succes =>
        {

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
        Social.ShowLeaderboardUI();
    }
    #endregion
}
