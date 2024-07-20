using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class PrototypeAchievementManager : MonoBehaviour
{
    public static PrototypeAchievementManager Instance;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backslash))
        {
            StatsAndAchievements.Instance.ChangeStatBy("STAT_ENEMIES_KILLED", 990);

            int statTotal;

            StatsAndAchievements.Instance.GetStat("STAT_ENEMIES_KILLED", out statTotal);
            Debug.Log("Total Enemies Killed: " + statTotal);
        }
    }

    /// <summary>
    /// Pest Control: Kill 1000 enemies
    /// </summary>
    public void IncrementAndCheck1000KillsAch()
    {
        int statTotal;
        bool achIsUnlocked;

        //Increment
        StatsAndAchievements.Instance.ChangeStatBy("STAT_ENEMIES_KILLED", 1);

        StatsAndAchievements.Instance.GetStat("STAT_ENEMIES_KILLED", out statTotal);
        //Debug.Log("Total Enemies Killed: " + statTotal);

        //Check
        StatsAndAchievements.Instance.GetAchievement("ACH_1000_KILLS", out achIsUnlocked);

        if(statTotal == 1000)
        {
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("SHOULD UNLOCK");
        }
    }
}
