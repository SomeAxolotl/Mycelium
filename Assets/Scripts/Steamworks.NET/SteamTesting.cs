using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class SteamTesting : MonoBehaviour
{
#if !DISABLESTEAMWORKS
    public void GiveAchievement()
    {
        if (SteamManager.Initialized == false)
        {
            Debug.LogError("SteamManager was not initialized");
            return;
        }

        SteamUserStats.RequestCurrentStats();

        SteamUserStats.SetAchievement("ACH_TEST");

        SteamUserStats.StoreStats();
    }

    public void IncrementStat()
    {
        if (SteamManager.Initialized == false)
        {
            Debug.LogError("SteamManager was not initialized");
            return;
        }

        int stat = -1;

        SteamUserStats.RequestCurrentStats();

        Debug.Log("Intial: " + stat);
        SteamUserStats.GetStat("STAT_CACHES_OPENED", out stat);
        Debug.Log("First Get: " + stat);

        SteamUserStats.SetStat("STAT_CACHES_OPENED", stat + 1);

        SteamUserStats.GetStat("STAT_CACHES_OPENED", out stat);
        Debug.Log("First Set: " + stat);

        SteamUserStats.StoreStats();
    }

    public void ResetStatsAndAchievements()
    {
        if (SteamManager.Initialized == false)
        {
            Debug.LogError("SteamManager was not initialized");
            return;
        }

        SteamUserStats.ResetAllStats(true);

        SteamUserStats.RequestCurrentStats();
    }
#else
    public void GiveAchievement() { return; }
    public void IncrementStat() { return; }
    public void ResetStatsAndAchievements() { return; }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(SteamTesting))]
class SteamTestingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Setup Stuff
        base.OnInspectorGUI();

        var steamTesting = (SteamTesting)target;
        if (steamTesting == null) return;

        //Actual Stuff
        if (GUILayout.Button("Give Test Achievement"))
        {
            steamTesting.GiveAchievement();
        }

        if (GUILayout.Button("Give +1 Caches Opened"))
        {
            steamTesting.IncrementStat();
        }

        if (GUILayout.Button("Reset All Stats and Achievements"))
        {
            steamTesting.ResetStatsAndAchievements();
        }
    }
}
#endif
