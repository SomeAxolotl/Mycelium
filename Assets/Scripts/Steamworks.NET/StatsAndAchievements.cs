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
        StartCoroutine(OnListAllAchievements());
    }

    private IEnumerator OnListAllAchievements()
    {
        bool shouldContinue = false;

        yield return StartCoroutine(DoChecks(result => shouldContinue = result));
        if (shouldContinue == false) { yield break; }

        for (uint i = 0; i < SteamUserStats.GetNumAchievements(); i++)
        {
            Debug.Log(SteamUserStats.GetAchievementName(i));
        }
    }

    //==================================================================================
    //GIVE ACHIEVEMENT
    //==================================================================================

    public void GiveAchievement(string achName)
    {
        StartCoroutine(OnGiveAchievement(achName));
    }

    private IEnumerator OnGiveAchievement(string achName)
    {
        bool shouldContinue = false;

        yield return StartCoroutine(DoChecks(result => shouldContinue = result));
        if (shouldContinue == false) { yield break; }

        SteamUserStats.SetAchievement(achName);
    }

    //==================================================================================
    //GET STATS
    //==================================================================================

    public void GetStat(string statName, out int statValue)
    {
        int valueBuffer = -1;

        StartCoroutine(OnGetStat(statName, value => valueBuffer = value));

        statValue = valueBuffer;
    }

    private IEnumerator OnGetStat(string statName, System.Action<int> callback)
    {
        bool shouldContinue = false;

        yield return StartCoroutine(DoChecks(result => shouldContinue = result));
        if (shouldContinue == false) { yield break; }

        int value;

        SteamUserStats.GetStat(statName, out value);
        callback(value);
    }

    public void GetStat(string statName, out float statValue)
    {
        float valueBuffer = -1;

        StartCoroutine(OnGetStat(value => valueBuffer = value, statName));

        statValue = valueBuffer;
    }

    private IEnumerator OnGetStat(System.Action<float> callback, string statName)
    {
        bool shouldContinue = false;

        yield return StartCoroutine(DoChecks(result => shouldContinue = result));
        if (shouldContinue == false) { yield break; }

        float value;

        SteamUserStats.GetStat(statName, out value);
        callback(value);
    }

    //==================================================================================
    //CHANGE STAT BY
    //==================================================================================

    public void ChangeStatBy(string statName, int changeBy)
    {
        StartCoroutine(OnChangeStatBy(statName, changeBy));
    }

    private IEnumerator OnChangeStatBy(string statName, int changeBy)
    {
        bool shouldContinue = false;

        yield return StartCoroutine(DoChecks(result => shouldContinue = result));
        if (shouldContinue == false) { yield break; }

        int currentValue;

        SteamUserStats.GetStat(statName, out currentValue);
        SteamUserStats.SetStat(statName, currentValue + changeBy);
    }

    public void ChangeStatBy(string statName, float changeBy)
    {
        StartCoroutine(OnChangeStatBy(statName, changeBy));
    }

    private IEnumerator OnChangeStatBy(string statName, float changeBy)
    {
        bool shouldContinue = false;

        yield return StartCoroutine(DoChecks(result => shouldContinue = result));
        if (shouldContinue == false) { yield break; }

        float currentValue;

        SteamUserStats.GetStat(statName, out currentValue);
        SteamUserStats.SetStat(statName, currentValue + changeBy);
    }

    //==================================================================================
    //STORE STATS AND ACHIEVEMENTS
    //==================================================================================

    public void StoreStatsAndAchievements()
    {
        StartCoroutine(OnStoreStatsAndAchievements());
    }

    private IEnumerator OnStoreStatsAndAchievements()
    {
        bool shouldContinue = false;

        yield return StartCoroutine(DoChecks(result => shouldContinue = result));
        if (shouldContinue == false) { yield break; }

        SteamUserStats.StoreStats();
    }

    //==================================================================================
    //RESET STATS AND ACHIEVEMENTS
    //==================================================================================

    public void ResetStatsAndAchievements()
    {
        StartCoroutine(OnResetStatsAndAchievements());
    }

    private IEnumerator OnResetStatsAndAchievements()
    {
        bool shouldContinue = false;

        yield return StartCoroutine(DoChecks(result => shouldContinue = result));
        if (shouldContinue == false) { yield break; }

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

    private IEnumerator DoChecks(System.Action<bool> callback)
    {
        bool result = false;

        if (SteamManager.Initialized == false)
        {
            callback(result);
            yield break;
        }

        float timeElapsed = 0;
        while (statsReceived == false)
        {
            if (timeElapsed > 30)
            {
                callback(result);
                yield break;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        result = true;
        callback(result);
    }

#else
    public void ListAllAchievements() { return; }
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
