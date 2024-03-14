using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingManager : MonoBehaviour
{
    public static TestingManager Instance;

    private enum StatSkills
    {
        NoSkill,
        Eruption,
        LivingCyclone,
        RelentlessFury,
        Blitz,
        TrophicCascade,
        Mycotoxins,
        Spineshot,
        UnstablePuffball,
        Undergrowth,
        LeechingSpore,
        Sporeburst,
        DefenseMechanism
    }

    private enum WeaponTypes
    {
        AvocadoFlamberge,
        BambooPartisan,
        RoseMace,
        ObsidianScimitar,
        OpalRapier,
        GeodeHammer,
        MandibleSickle,
        CarpalSais,
        FemurClub
    }

    private enum SubspeciesSkills
    {
        FungalMight,
        DeathBlossom,
        FairyRing,
        Zombify
    }

    [Header("Nutrients - Alpha1")]
    [SerializeField][Tooltip("Alpha1 - Add Nutrients")] private int nutrientsToGain = 1000;

    [Header("Materials - Alpha2")]
    [SerializeField][Tooltip("Alpha2 - Add Materials")] private int logsToGain = 1;
    [SerializeField][Tooltip("Alpha2 - Add Materials")] private int exoskeletonsToGain = 1;
    [SerializeField][Tooltip("Alpha2 - Add Materials")] private int calcitesToGain = 1;
    [SerializeField][Tooltip("Alpha2 - Add Materials")] private int fleshesToGain = 1;

    [Header("Stats - Alpha3")]
    [SerializeField][Tooltip("Alpha3 - Set Stats")] private int primalLevelsToGain = 1;
    [SerializeField][Tooltip("Alpha3 - Set Stats")] private int sentienceLevelsToGain = 1;
    [SerializeField][Tooltip("Alpha3 - Set Stats")] private int speedLevelsToGain = 1;
    [SerializeField][Tooltip("Alpha3 - Set Stats")] private int vitalityLevelsToGain = 1;

    [Header("Stat Skills - Alpha4")] 
    [SerializeField][Tooltip("Alpha4 - Set Skills")] private StatSkills skill1;
    [SerializeField][Tooltip("Alpha4 - Set Skills")] private StatSkills skill2;

    [Header("Weapon - Alpha5")]
    [SerializeField][Tooltip("Alpha5 - Set Weapon")] private WeaponTypes weaponType;

    [Header("Subspecies Skill - Alpha6")]
    [SerializeField][Tooltip("Alpha6 - Grow New Spore")] private SubspeciesSkills subspeciesSkill;

    [Header("References")]
    [SerializeField] List<GameObject> weaponPrefabs = new List<GameObject>(); //Alpha5

    private GameObject playerParent;
    private GameObject player;

    private NutrientTracker nutrientTracker; //Alpha1 and Alpha2
    private CharacterStats playerStats; //Alpha3
    private SkillManager skillManager; //Alpha4
    private HUDSkills hudSkills; //Alpha4
    private SpawnCharacter spawnCharacter; //Alpha6

    void Awake()
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(SetPlayerNutrients());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(SetPlayerMaterials());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(SetPlayerStats());
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(SetPlayerSkills());
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartCoroutine(SetPlayerWeapon());
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartCoroutine(SetPlayerSubspeciesSkill());
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StartCoroutine(SetLevel(3));
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartCoroutine(SetLevel(4));
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartCoroutine(SetLevel(6));
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(SetLevel(2));
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            StartCoroutine(SetLevel(1));
        }
    }

    IEnumerator SetPlayerNutrients()
    {
        GetCurrentPlayer();

        yield return null;
        nutrientTracker.AddNutrients(nutrientsToGain);
    }

    IEnumerator SetPlayerMaterials()
    {
        GetCurrentPlayer();

        yield return null;
        nutrientTracker.storedLog += logsToGain;
        nutrientTracker.storedExoskeleton += exoskeletonsToGain;
        nutrientTracker.storedCalcite += calcitesToGain;
        nutrientTracker.storedFlesh += fleshesToGain;
    }

    IEnumerator SetPlayerStats()
    {
        GetCurrentPlayer();

        yield return null;
        playerStats.primalLevel += primalLevelsToGain;
        playerStats.sentienceLevel += sentienceLevelsToGain;
        playerStats.speedLevel += speedLevelsToGain;
        playerStats.vitalityLevel += vitalityLevelsToGain;

        playerStats.StartCalculateAttributes();
        playerStats.UpdateLevel();
    }

    IEnumerator SetPlayerSkills()
    {
        GetCurrentPlayer();

        yield return null;
        skillManager.SetSkill(skill1.ToString(), 1, player);
        hudSkills.ChangeSkillIcon(skill1.ToString(), 1);
        yield return null;
        skillManager.SetSkill(skill2.ToString(), 2, player);
        hudSkills.ChangeSkillIcon(skill2.ToString(), 2);
    }

    IEnumerator SetPlayerWeapon()
    {
        GetCurrentPlayer();

        yield return null;
        string weaponString = weaponType.ToString();
        foreach (GameObject weapon in weaponPrefabs)
        {
            if (weapon.name == weaponString)
            {
                Instantiate(weapon, player.transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator SetPlayerSubspeciesSkill()
    {
        GetCurrentPlayer();

        yield return null;
        skillManager.SetSkill(subspeciesSkill.ToString(), 0, player);
        hudSkills.ChangeSkillIcon(subspeciesSkill.ToString(), 0);
    }

    IEnumerator SetLevel(int buildIndex)
    {
        GetCurrentPlayer();

        yield return null;
        SceneLoader.Instance.BeginLoadScene(buildIndex, true);
    }

    void GetCurrentPlayer()
    {
        playerParent = GameObject.Find("PlayerParent");
        player = GameObject.FindWithTag("currentPlayer");
        
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>(); //Alpha1 and Alpha2
        playerStats = player.GetComponent<CharacterStats>(); //Alpha3
        skillManager = playerParent.GetComponent<SkillManager>(); //Alpha4
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        spawnCharacter = playerParent.GetComponent<SpawnCharacter>(); //Alpha6
    }
}
