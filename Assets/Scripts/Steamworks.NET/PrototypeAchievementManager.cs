using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeAchievementManager : MonoBehaviour
{
    public static PrototypeAchievementManager Instance;

    private GameObject player;

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
    private void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
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
    public void WithoutAPumpkinAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_TUTORIAL", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_TUTORIAL");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST TUTORIAL DEATH");
        }
    }
    public void IDidItMomAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_KILL_BOSS", out achIsUnlocked);

        if(!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_KILL_BOSS");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST BOSS KILL");
        }
    }

    public void IMadeAFungiAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_GROW_SPORE", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_GROW_SPORE");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST SPORE GROWTH");
        }
    }
    public void ShouldHaveUsedBugSprayAch()
    {
        bool achIsUnlocked;
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        StatsAndAchievements.Instance.GetAchievement("ACH_BUG_SPRAY", out achIsUnlocked);
        Collider[] enemies = Physics.OverlapSphere(player.transform.position, 10f, enemyLayerMask);

        if (!achIsUnlocked && enemies.Length >= 10)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_BUG_SPRAY");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST BUG SPRAY DEATH");
        }
    }
    public void YokedAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_MAX_PRIMAL", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_MAX_PRIMAL");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST MAX PRIMAL UPGRADE");
        }
    }
    public void BrainiacManiacAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_MAX_SENTIENCE", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_MAX_SENTIENCE");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST MAX SENTIENCE UPGRADE");
        }
    }
    public void GottaGoFastAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_MAX_SPEED", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_MAX_SPEED");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST MAX SPEED UPGRADE");
        }
    }
    public void ICanTankItAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_MAX_VITALITY", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_MAX_VITALITY");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST MAX VITALITY UPGRADE");
        }
    }
    public void NutritionalAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_MAX_ALL_STATS", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_MAX_ALL_STATS");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST MAX ALL STATS");
        }
    }
}
