/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

/// <summary>
/// Achievements and leaderboards. Google Play Games is gone (App Store only for now),
/// so these are empty on purpose: this is where Game Center goes when we add it.
/// </summary>
public static class PlayServices
{
    public static void UnlockAchievement(string id) { }

    public static void IncrementAchievement(string id, int stepsToIncrement) { }

    public static void ShowAchievementsUI() { }

    public static void AddScoreToLeaderBoard(string leaderBoardID, long score) { }

    public static void ShowLeaderBoardUI() { }
}
