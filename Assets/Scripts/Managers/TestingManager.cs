using System;
using System.Collections;
using System.Collections.Generic;
using RonaldSunglassesEmoji.Personalities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingManager : MonoBehaviour
{
    public static TestingManager Instance;

    public enum StatSkills
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

    public enum WeaponTypes
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

    public enum SubspeciesSkills
    {
        NoSkill,
        FungalMight,
        DeathBlossom,
        FairyRing,
        Zombify
    }

    public enum SubspeciesNames
    {
        Default,
        Poison,
        Coral,
        Cordyceps
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

    [Header("Subspecies Skill - Alpha5")]
    [SerializeField][Tooltip("Alpha5 - Set Subspecies Skill")] private SubspeciesSkills subspeciesSkill;

    [Header("Weapon - Alpha6")]
    [SerializeField][Tooltip("Alpha6 - Set Weapon")] private CustomWeapon customWeapon;

    [Header("Grow a custom Spore - K")]
    [SerializeField][Tooltip("K - Grow a custom Spore")] private CustomSpore customSpore;

    [Header("Go to Daybreak Arboretum - Alpha7")]
    [SerializeField][Tooltip("Alpha7 - Go to Daybreak Arboretum")] private int daybreakBuildIndex = 3;

    [Header("Go to Delta Crag - Alpha8")]
    [SerializeField][Tooltip("Alpha8 - Go to Delta Crag")] private int cragBuildIndex = 4;

    [Header("Go to Impact Barrens - Alpha9")]
    [SerializeField][Tooltip("Alpha9 - Go to Impact Barrens")] private int barrensBuildIndex = 5;

    [Header("Go to The Carcass - Alpha0")]
    [SerializeField][Tooltip("Alpha0 - Go to The Carcass")] private int carcassBuildIndex = 2;

    [Header("Go to the Tutorial - Minus")]
    [SerializeField][Tooltip("Minus - Go to the Tutorial")] private int tutorialBuildIndex = 1;

    #pragma warning disable 0414
    [Header("Unlock all Furniture - L")]
    [SerializeField][Tooltip("L - Unlock all Furniture")] private bool meow = true;

    [Header("Fade HUD In - Z")]
    [SerializeField][Tooltip("Z - Fade HUD In")] private bool meower = true;

    [Header("Fade HUD Out - X")]
    [SerializeField][Tooltip("X - Fade HUD Out")] private bool meowing = true;

    [Header("Disable PlayerParent - Slash (/)")]
    [SerializeField][Tooltip("Slash (/) - Disable PlayerParent")] private bool meowest = true;

    [Header("Set GlobalData.areaCleared to true - C")]
    [SerializeField][Tooltip("C - Set GlobalData.areaCleared to true")] private bool meoww = true;

    [Header("Increment GlobalData.currentLoop by 1 - B")]
    [SerializeField][Tooltip("B - Increment GlobalData.currentLoop by 1")] private bool meeeeeow = true;

    [Header("Spawn a Stat Upgrade")]
    [SerializeField][Tooltip("U - Spawn a Stat Upgrade")] private bool meOw = true;
    #pragma warning restore 0414

    [Header("References")]
    [SerializeField] protected List<GameObject> weaponPrefabs = new List<GameObject>(); //Alpha5
    [SerializeField] GameObject statUpgradePrefab;

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
            StartCoroutine(SetPlayerSubspeciesSkill());
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartCoroutine(SetPlayerWeapon());
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StartCoroutine(SetLevel(daybreakBuildIndex));
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartCoroutine(SetLevel(cragBuildIndex));
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartCoroutine(SetLevel(barrensBuildIndex));
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GlobalData.currentLoop = 1;
            StartCoroutine(SetLevel(carcassBuildIndex));
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            StartCoroutine(SetLevel(tutorialBuildIndex));
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnCustomSpore();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            UnlockAllFurniture();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject.Find("HUD").GetComponent<HUDController>().FadeInHUD();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("HUD").GetComponent<HUDController>().FadeOutHUD();
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            ToggleSporeRenderer();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GlobalData.areaCleared = true;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            GlobalData.currentLoop++;
            Debug.Log("Current Loop: " + GlobalData.currentLoop);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(SpawnStatUpgrade());
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
        if (GameObject.FindWithTag("PlayerParent") != null)
        playerParent.GetComponent<PlayerHealth>().ResetHealth();
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

    IEnumerator SetPlayerSubspeciesSkill()
    {
        GetCurrentPlayer();

        yield return null;
        skillManager.SetSkill(subspeciesSkill.ToString(), 0, player);
        hudSkills.ChangeSkillIcon(subspeciesSkill.ToString(), 0);
    }

    IEnumerator SetPlayerWeapon()
    {
        GetCurrentPlayer();

        yield return null;
        
        customWeapon.SpawnWeapon();
    }

    IEnumerator SetLevel(int buildIndex)
    {
        GetCurrentPlayer();

        yield return null;
        SceneLoader.Instance.BeginLoadScene(buildIndex, true);
    }

    void SpawnCustomSpore()
    {
        string test = customSpore.subspeciesSkill.ToString();

        GameObject tempObject = new GameObject("wtf");

        tempObject.AddComponent<CharacterStats>();
        tempObject.AddComponent<DesignTracker>();

        CharacterStats customStats = tempObject.GetComponent<CharacterStats>();
        customStats.sporeName = customSpore.sporeName;
        customStats.equippedSkills[0] = customSpore.subspeciesSkill.ToString();
        customStats.equippedSkills[1] = customSpore.skill1.ToString();
        customStats.equippedSkills[2] = customSpore.skill2.ToString();
        customStats.sporePersonality = customSpore.sporePersonality;

        DesignTracker customDesign = tempObject.GetComponent<DesignTracker>();
        customDesign.capColor = customSpore.capColor;
        customDesign.bodyColor = customSpore.bodyColor;
        customDesign.EyeOption = customSpore.eyeTextureIndex;
        customDesign.MouthOption = customSpore.mouthTextureIndex;


        GameObject.FindWithTag("PlayerParent").GetComponent<SpawnCharacter>().SpawnNewCharacter(customSpore.subspecies.ToString(), customStats, customDesign, customSpore.randomDesignFromSpecies, customSpore.randomName);

        Destroy(tempObject);
    }

    void UnlockAllFurniture()
    {
        FurnitureManager furnitureManager = GameObject.Find("FurnitureManager").GetComponent<FurnitureManager>();

        furnitureManager.UnlockAllFurniture();
    }

    IEnumerator SpawnStatUpgrade()
    {
        GetCurrentPlayer();

        yield return null;

        Instantiate(statUpgradePrefab, player.transform.position, Quaternion.identity);

    }

    void ToggleSporeRenderer()
    {
        GetCurrentPlayer();

        Renderer[] childRenderers = playerParent.GetComponentsInChildren<Renderer>();
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", player.transform.position, Quaternion.Euler(-90,0,0));
        
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = !renderer.enabled;
        }
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

    [System.Serializable]
    class CustomSpore
    {
        [Header("Stats")]
        public string sporeName = "Gob";
        [Tooltip("Custom Spore's nanme is just a random name in the pool")] public bool randomName = true;
        public SubspeciesSkills subspeciesSkill = SubspeciesSkills.NoSkill;
        public StatSkills skill1 = StatSkills.NoSkill;
        public StatSkills skill2 = StatSkills.NoSkill;
        public SporePersonalities sporePersonality = SporePersonalities.Energetic;

        [Header("Design")]
        [Tooltip("Custom Spore's design is the same as just growing that subspecies")] public bool randomDesignFromSpecies = true;
        public SubspeciesNames subspecies = SubspeciesNames.Default;
        public Color capColor = Color.green;
        public Color bodyColor = Color.blue;
        [Range(0, 10)] public int eyeTextureIndex = 0;
        [Range(0, 5)] public int mouthTextureIndex = 0;
    }

    [System.Serializable]
    class CustomWeapon
    {
        [SerializeField] public bool randomWeaponType = true;
        [SerializeField] public bool randomAttribute = true;
        [SerializeField] public WeaponTypes weaponType;

        public void SpawnWeapon()
        {
            if (!randomWeaponType)
            {
                string weaponString = weaponType.ToString();
                GameObject instantiatedWeapon;
                foreach (GameObject weapon in Instance.weaponPrefabs)
                {
                    if (weapon.name == weaponString)
                    {
                        instantiatedWeapon = Instantiate(weapon, Instance.player.transform.position, Quaternion.identity);
                    }
                }
            }
            else
            {
                int randomWeaponIndex = UnityEngine.Random.Range(0, Instance.weaponPrefabs.Count);

                Instantiate(Instance.weaponPrefabs[randomWeaponIndex], Instance.player.transform.position, Quaternion.identity);
            }

            if (randomAttribute)
            {
                //AttributeAssigner.Instance.AddRandomAttribute(instantiatedWeapon);
            }
            else
            {
                //AttributeAssigner.Instance.AddSpecificAttribute(instantiatedWeapon);
            }
        }
    }
}
