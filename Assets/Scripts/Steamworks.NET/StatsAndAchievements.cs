using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class StatsAndAchievements : MonoBehaviour
{
    public static StatsAndAchievements Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

#if !DISABLESTEAMWORKS
    private const uint APPID = 2969710;
    private const string RCS_ERROR = "RequestCurrentStats() went wrong or something go bother ryan | Error Code: ";
    private const string SS_ERROR = "StoreStats() went wrong or something go bother ryan | Error Code: ";

    protected Callback<UserStatsReceived_t> cb_userStatsReceived;
    protected Callback<UserStatsStored_t> cb_userStatsStored;
    private bool statsReceived;

    private void OnEnable()
    {
        if (SteamManager.Initialized == false) { return; }

        statsReceived = false;

        cb_userStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        cb_userStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);

        SteamUserStats.RequestCurrentStats();
    }

    private void OnDisable()
    {
        StoreStatsAndAchievements();

        if (cb_userStatsReceived != null) { cb_userStatsReceived.Dispose(); }
        if (cb_userStatsStored != null) { cb_userStatsStored.Dispose(); }
    }

    private void OnUserStatsReceived(UserStatsReceived_t cb)
    {
        if (SteamManager.Initialized == false) { return; }

        if (cb.m_nGameID != APPID)
        {
            Debug.LogError(RCS_ERROR + "wrong game");
            return;
        }

        if (cb.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError(RCS_ERROR + cb.m_eResult);
            return;
        }

        statsReceived = true;
    }

    private void OnUserStatsStored(UserStatsStored_t cb)
    {
        if (SteamManager.Initialized == false) { return; }

        if (cb.m_nGameID != APPID)
        {
            Debug.LogError(SS_ERROR + "wrong game");
            return;
        }

        if (cb.m_eResult == EResult.k_EResultInvalidParam)
        {
            UserStatsReceived_t callback = new UserStatsReceived_t();
            callback.m_eResult = EResult.k_EResultOK;
            callback.m_nGameID = APPID;
            OnUserStatsReceived(callback);
            return;
        }

        if (cb.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError(SS_ERROR + cb.m_eResult);
            return;
        }
    }

    //==================================================================================
    //LIST ALL ACHIEVEMENTS
    //==================================================================================

    public void ListAllAchievements()
    {
        if (DoChecks() == false) { return; }

        for (uint i = 0; i < SteamUserStats.GetNumAchievements(); i++)
        {
            Debug.Log(SteamUserStats.GetAchievementName(i));
        }
    }

    //==================================================================================
    //GET ACHIEVEMENT
    //==================================================================================

    public void GetAchievement(string achName, out bool isUnlocked)
    {
        if (DoChecks() == false) { isUnlocked = false; return; }

        SteamUserStats.GetAchievement(achName, out isUnlocked);
    }

    //==================================================================================
    //GIVE ACHIEVEMENT
    //==================================================================================

    public void GiveAchievement(string achName)
    {
        if (DoChecks() == false) { return; }

        SteamUserStats.SetAchievement(achName);
    }

    //==================================================================================
    //GET STATS
    //==================================================================================

    public void GetStat(string statName, out int value)
    {
        if (DoChecks() == false) { value = -1; return; }

        SteamUserStats.GetStat(statName, out value);
    }

    public void GetStat(string statName, out float value)
    {
        if (DoChecks() == false) { value = -1f; return; }

        SteamUserStats.GetStat(statName, out value);
    }

    //==================================================================================
    //CHANGE STAT BY
    //==================================================================================

    public void ChangeStatBy(string statName, int changeBy)
    {
        if (DoChecks() == false) { return; }

        int currentValue;

        SteamUserStats.GetStat(statName, out currentValue);
        SteamUserStats.SetStat(statName, currentValue + changeBy);
    }

    public void ChangeStatBy(string statName, float changeBy)
    {
        if (DoChecks() == false) { return; }

        float currentValue;

        SteamUserStats.GetStat(statName, out currentValue);
        SteamUserStats.SetStat(statName, currentValue + changeBy);
    }

    //==================================================================================
    //STORE STATS AND ACHIEVEMENTS
    //==================================================================================

    public void StoreStatsAndAchievements()
    {
        if (DoChecks() == false) { return; }

        SteamUserStats.StoreStats();

        //do NOT comment this debug out because if it's annoying then we're calling this function too much -ryan
        Debug.Log("STATS AND ACHIEVEMENTS STORED!!!");
    }

    //==================================================================================
    //RESET STATS AND ACHIEVEMENTS
    //==================================================================================

    public void ResetStatsAndAchievements()
    {
        if (DoChecks() == false) { return; }

        StartCoroutine(OnResetStatsAndAchievements());
    }

    private IEnumerator OnResetStatsAndAchievements()
    {
        SteamUserStats.ResetAllStats(true);
        Debug.Log("Resetting All Stats and Achievements");

        yield return new WaitForSecondsRealtime(1f);

        statsReceived = false;
        SteamUserStats.RequestCurrentStats();
        Debug.Log("Requesting Current Stats");
    }

    //==================================================================================
    //DO CHECKS
    //==================================================================================

    private bool DoChecks()
    {
        if (SteamManager.Initialized == false)
        {
            return false;
        }

        if (statsReceived == false)
        {
            return false;
        }

        return true;
    }

#else
    public void ListAllAchievements() { return; }
    public void GetAchievement(string achName, out bool isUnlocked) { isUnlocked = false; return; }
    public void GiveAchievement(string achName) { return; }
    public void GetStat(string statName, out int statValue) { statValue = -1; return; }
    public void GetStat(string statName, out float statValue) { statValue = -1; return; }
    public void ChangeStatBy(string statName, int changeBy) { return; }
    public void ChangeStatBy(string statName, float changeBy) { return; }
    public void ResetStatsAndAchievements() { return; }
    public void StoreStatsAndAchievements() { return; }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(StatsAndAchievements))]
class StatsAndAchievementsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Setup Stuff
        base.OnInspectorGUI();

        var statsAndAchievements = (StatsAndAchievements)target;
        if (statsAndAchievements == null) return;

        //Actual Stuff
        if (GUILayout.Button("List Internal Achievement Names"))
        {
            if(Application.isPlaying == false) { return; }
            statsAndAchievements.ListAllAchievements();
        }

        if (GUILayout.Button("Reset Stats and Achievements"))
        {
            if (Application.isPlaying == false) { return; }
            statsAndAchievements.ResetStatsAndAchievements();
        }
    }
}
#endif
