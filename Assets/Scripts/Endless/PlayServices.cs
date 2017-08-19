using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;

public class PlayServices : MonoBehaviour
{
    private void Awake()
    {
        /*
        PlayGamesClientConfiguration config = new
            PlayGamesClientConfiguration.Builder()
            .Build();
        PlayGamesPlatform.DebugLogEnabled = true;


        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        */

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
      .RequestEmail()
      // requests a server auth code be generated so it can be passed to an
      //  associated back end server application and exchanged for an OAuth token.
      .RequestServerAuthCode(false)
      .RequestIdToken()
      .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }
}
