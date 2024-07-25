using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;

public class PrototypeAchievementManager : MonoBehaviour
{
    public static PrototypeAchievementManager Instance;

    private SporeDataList sporeDataList;
    private CurrentSporeStats currrentCharStats = new CurrentSporeStats();
    //private SporeData currentSpore;
    private string filePath;

    private GameObject currentPlayer;
    private GameObject playerParent;
    private SwapWeapon swapWeaponScript;

    //VersatileSpore + UseEmAll Stuff
    private GameObject mostRecentWeapon;
    private WeaponStats currentWeaponStats;
    Enigmatic enigmaticComponent;

    //InsectaPenta Stuff
    private int killsBuffer;

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
        if (SceneManager.GetActiveScene().name == "Main Menu") return;

        GetJSONdata(GlobalData.profileNumber);
        currentPlayer = GameObject.FindWithTag("currentPlayer");
        playerParent = GameObject.FindWithTag("PlayerParent");
        swapWeaponScript = playerParent.GetComponent<SwapWeapon>();
        SporeData currentSpore = sporeDataList.Spore_Data.Find(spore => spore.sporeTag == "currentPlayer");
        currrentCharStats.primalLevel = currentSpore.lvlPrimal;
        currrentCharStats.speedLevel = currentSpore.lvlSpeed;
        currrentCharStats.sentienceLevel = currentSpore.lvlSentience;
        currrentCharStats.vitalityLevel = currentSpore.lvlVitality;

        killsBuffer = 0;
    }

    void GetJSONdata(int profileNumber)
    {
        //Begin Reading SporeData.json
        if (Application.isEditor)
        {
            filePath = Application.dataPath + "/SporeData" + profileNumber + ".json";
        }
        else
        {
            filePath = Application.persistentDataPath + "/SporeData" + profileNumber + ".json";
        }

        sporeDataList = JsonUtility.FromJson<SporeDataList>(System.IO.File.ReadAllText(filePath));
    }

    /// <summary>
    /// Pest Control: Kill 1000 enemies
    /// </summary>
    public void IncrementAndCheck1000KillsAch()
    {
        int statTotal;

        //Increment
        StatsAndAchievements.Instance.ChangeStatBy("STAT_ENEMIES_KILLED", 1);

        StatsAndAchievements.Instance.GetStat("STAT_ENEMIES_KILLED", out statTotal);
        //Debug.Log("Total Enemies Killed: " + statTotal);

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
        Collider[] enemies = Physics.OverlapSphere(currentPlayer.transform.position, 10f, enemyLayerMask);

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

    public void VersatileSporeAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_20_KILLS_ALL_WEAPON_TYPE", out achIsUnlocked);

        //If the achievement is unlocked just stop running the function
        if (achIsUnlocked == true) return;

        //This is an optimization so that we dont use GetComponent() every time an enemy is killed;
        if (mostRecentWeapon != swapWeaponScript.O_curWeapon)
        {
            mostRecentWeapon = swapWeaponScript.O_curWeapon;
            currentWeaponStats = swapWeaponScript.O_curWeapon.GetComponent<WeaponStats>();
        }

        switch(currentWeaponStats.weaponType.ToString())
        {
            case "Slash":
                GlobalData.slashKills += 1;
                break;

            case "Smash":
                GlobalData.smashKills += 1;
                break;

            case "Stab":
                GlobalData.stabKills += 1;
                break;

            default:
                break;
        }

        if (GlobalData.slashKills >= 20 && GlobalData.smashKills >= 20 && GlobalData.stabKills >= 20 && achIsUnlocked == false)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_20_KILLS_ALL_WEAPON_TYPE");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON 20 KILLS WITH EACH WEAPON TYPE");
        }
    }

    public void BoldTenfoldAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_10_KILLS_SAME_TIME", out achIsUnlocked);

        //If the achievement is unlocked just stop running the function
        if (achIsUnlocked == true) return;

        killsBuffer += 1;

        if (killsBuffer == 1)
        {
            StartCoroutine(InsectaPentaCoroutine());
        }

        if (killsBuffer >= 10 && achIsUnlocked == false)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_10_KILLS_SAME_TIME");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON 10 KILLS AT THE SAME TIME");
        }
    }

    private IEnumerator InsectaPentaCoroutine()
    {
        float resetBufferTime = 0.5f;

        while(killsBuffer > 0)
        {
            yield return new WaitForSeconds(resetBufferTime / 5);

            killsBuffer -= 1;
        }

        killsBuffer = 0;
    }
    public void SelfSufficientAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_LOOP_2_BASE_STATS", out achIsUnlocked);

        if (!achIsUnlocked &&
            currrentCharStats.primalLevel == 1 &&
            currrentCharStats.speedLevel == 1 &&
            currrentCharStats.sentienceLevel == 1 &&
            currrentCharStats.vitalityLevel == 1)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_LOOP_2_BASE_STATS");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST LOOP 2 BASE STATS");
        }
    }
    public void FullyFurnishedAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_ALL_FURNITURE", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_ALL_FURNITURE");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST ALL FURNITURE UNLOCK");
        }
    }

    public void LoopFiveAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_LOOP_5", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_LOOP_5");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST LOOP 5 BEATEN");
        }
    }

    public void LoopTenAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_LOOP_10", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_LOOP_10");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST LOOP 10 BEATEN");
        }
    }

    public void FeedingTheFishAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_20_WATER_KILLS", out achIsUnlocked);

        GlobalData.waterKills += 1;

        if (GlobalData.waterKills >= 20 && !achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_20_WATER_KILLS");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON 20 WATER KILLS");
        }
    }

    public void UseEmAllAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_ALL_ENIGMATIC", out achIsUnlocked);

        //If the achievement is unlocked just stop running the function
        if (achIsUnlocked == true) return;

        swapWeaponScript.O_curWeapon.TryGetComponent(out enigmaticComponent);

        //If it's not enigmatic stop running the function
        if (enigmaticComponent == null) return;

        GlobalData.enigmaticWeaponKills.Add(swapWeaponScript.O_curWeapon.name.Replace("(Clone)", ""));
        GlobalData.enigmaticWeaponKills = GlobalData.enigmaticWeaponKills.Distinct().ToList();

        Debug.Log("ENIGMATIC UINQUES: " + GlobalData.enigmaticWeaponKills.Count(), mostRecentWeapon);

        if (GlobalData.enigmaticWeaponKills.Count >= 10 && achIsUnlocked == false)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_ALL_ENIGMATIC");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON KILLS WITH ALL ENIGMATIC WEAPONS");
        }
    }
    public void CloseCallAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_CLOSE_CALL", out achIsUnlocked);
        Debug.Log("UNLOCK ONCLOSE CALL");
        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_CLOSE_CALL");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST CLOSE CALL");
        }
    }
    public void DiversityAch()
    {
        bool achIsUnlocked;

        StatsAndAchievements.Instance.GetAchievement("ACH_SPORE_DIVERSITY", out achIsUnlocked);

        if (!achIsUnlocked)
        {
            StatsAndAchievements.Instance.GiveAchievement("ACH_SPORE_DIVERSITY");
            StatsAndAchievements.Instance.StoreStatsAndAchievements();
            Debug.Log("UNLOCK ON FIRST SPORE DIVERSITY");
        }
    }
}


[Serializable]
public class CurrentSporeStats
{
    public int primalLevel;
    public int speedLevel;
    public int sentienceLevel;
    public int vitalityLevel;
}
