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

        SteamUserStats.RequestCurrentStats();
    }

    public void ResetStatsAndAchievements()
    {
        if (SteamManager.Initialized == false)
        {
            Debug.LogError("SteamManager was not initialized");
            return;
        }

        SteamUserStats.RequestCurrentStats();

        SteamUserStats.ResetAllStats(true);

        SteamUserStats.RequestCurrentStats();
    }
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

        if (GUILayout.Button("Reset All Stats and Achievements"))
        {
            steamTesting.ResetStatsAndAchievements();
        }
    }
}
#endif
