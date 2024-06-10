using System;
using System.Collections;
using System.Collections.Generic;
using RonaldSunglassesEmoji.Personalities;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    [Header("Subspecies Skill")]
    [SerializeField][Tooltip("Alpha5 - Set Subspecies Skill")] private SubspeciesSkills subspeciesSkill;

    [Header("Weapon")]
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

    [Header("Spawn loot cache - I")]
    [SerializeField][Tooltip("I - Spawn loot cache")] private bool isDeltaCragCache = false;

    [Header("References")]
    [SerializeField] protected List<GameObject> weaponPrefabs = new List<GameObject>(); //Alpha5
    [SerializeField] GameObject statUpgradePrefab;
    [SerializeField] GameObject daybreakCachePrefab;
    [SerializeField] GameObject deltaCragCachePrefab;

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

        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift))
        {
            GlobalData.canShowTooltips = true;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject.Find("HUD").GetComponent<HUDController>().FadeHUD(true);
        }

        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.LeftShift))
        {
            GlobalData.canShowTooltips = false;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("HUD").GetComponent<HUDController>().FadeHUD(false);
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            ToggleSporeRenderer();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GlobalData.areaCleared = true;
            Debug.Log("areaCleared set to true");
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(SpawnLootCache());
        }

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SelectTestingManager();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SelectPlayerParent();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SelectCurrentPlayer();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SelectObjectFromString("SoundEffectManager");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SelectObjectFromString("Steam");
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SelectCurrentWeapon();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            SelectObjectFromString("HUD");
        }


        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {   
            LockCursor();
        }
        #endif
    }

    #if UNITY_EDITOR
    void SelectTestingManager()
    {
        EditorGUIUtility.PingObject(this.gameObject);
        Selection.activeObject = this.gameObject;
        UnlockCursor();
    }
    void SelectCurrentPlayer()
    {
        UpdateCurrentPlayer();

        EditorGUIUtility.PingObject(player);
        Selection.activeObject = player;
        UnlockCursor();
    }
    void SelectPlayerParent()
    {
        UpdateCurrentPlayer();

        EditorGUIUtility.PingObject(playerParent);
        Selection.activeObject = playerParent;
        UnlockCursor();
    }
    void SelectObjectFromString(string objString)
    {
        GameObject obj = GameObject.Find(objString);

        if (obj != null)
        {
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
            UnlockCursor();
        }
        else
        {
            Debug.LogError("Object " + objString + " not found in the scene.");
        }
    }
    void SelectCurrentWeapon()
    {
        GameObject weapon = GameObject.FindWithTag("currentWeapon");

        if (weapon != null)
        {
            EditorGUIUtility.PingObject(weapon);
            Selection.activeObject = weapon;
            UnlockCursor();
        }
        else
        {
            Debug.LogError("No object with tag 'currentWeapon' found");
        }
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void LockCursor()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Cursor.visible == true)
        {
            Cursor.visible = false;
        }
    }
    #endif

    IEnumerator SetPlayerNutrients()
    {
        UpdateCurrentPlayer();

        yield return null;
        nutrientTracker.AddNutrients(nutrientsToGain);
    }

    IEnumerator SetPlayerMaterials()
    {
        UpdateCurrentPlayer();

        yield return null;
        nutrientTracker.storedLog += logsToGain;
        nutrientTracker.storedExoskeleton += exoskeletonsToGain;
        nutrientTracker.storedCalcite += calcitesToGain;
        nutrientTracker.storedFlesh += fleshesToGain;
    }

    IEnumerator SetPlayerStats()
    {
        UpdateCurrentPlayer();

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
        UpdateCurrentPlayer();

        yield return null;
        skillManager.SetSkill(skill1.ToString(), 1, player);
        hudSkills.ChangeSkillIcon(skill1.ToString(), 1);
        yield return null;
        skillManager.SetSkill(skill2.ToString(), 2, player);
        hudSkills.ChangeSkillIcon(skill2.ToString(), 2);
    }

    IEnumerator SetPlayerSubspeciesSkill()
    {
        UpdateCurrentPlayer();

        yield return null;
        skillManager.SetSkill(subspeciesSkill.ToString(), 0, player);
        hudSkills.ChangeSkillIcon(subspeciesSkill.ToString(), 0);
    }

    IEnumerator SetPlayerWeapon()
    {
        UpdateCurrentPlayer();

        yield return null;
        
        customWeapon.SpawnWeapon();
    }

    IEnumerator SetLevel(int buildIndex)
    {
        UpdateCurrentPlayer();

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

        Debug.Log("Furniture unlocked! Either exit play mode or leave the Carcass to save.");

        furnitureManager.UnlockAllFurniture();
    }

    IEnumerator SpawnStatUpgrade()
    {
        UpdateCurrentPlayer();

        yield return null;

        Instantiate(statUpgradePrefab, player.transform.position, Quaternion.identity);

    }

    IEnumerator SpawnLootCache()
    {
        UpdateCurrentPlayer();

        yield return null;

        GameObject prefabToSpawn; 
        if (isDeltaCragCache)
        {
            prefabToSpawn = deltaCragCachePrefab;
        }
        else
        {
            prefabToSpawn = daybreakCachePrefab;
        }
        GameObject spawnedCache = Instantiate(prefabToSpawn, player.transform.position, Quaternion.identity);
        StartCoroutine(ForceActivation(spawnedCache));
    }
    IEnumerator ForceActivation(GameObject obj)
    {
        yield return null;
        
        obj.SetActive(true);
    }

    void ToggleSporeRenderer()
    {
        UpdateCurrentPlayer();

        Renderer[] childRenderers = playerParent.GetComponentsInChildren<Renderer>();
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", player.transform.position, Quaternion.Euler(-90,0,0));
        
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = !renderer.enabled;
        }
    }

    void UpdateCurrentPlayer()
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
        [SerializeField] public int randomAttribute = 0;
        [SerializeField] public List<string> attributeNames = new List<string>(){""};
        [SerializeField] public WeaponTypes weaponType;

        public void SpawnWeapon()
        {
            GameObject instantiatedWeapon = null;
            if (!randomWeaponType)
            {
                string weaponString = weaponType.ToString();
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

                instantiatedWeapon = Instantiate(Instance.weaponPrefabs[randomWeaponIndex], Instance.player.transform.position, Quaternion.identity);
            }


            if(instantiatedWeapon != null){
                instantiatedWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
                
                for(int i = 0; i < randomAttribute; i++){
                    AttributeAssigner.Instance.AddRandomAttribute(instantiatedWeapon);
                }

                foreach(string attributeName in attributeNames){
                    if(attributeName != "" && attributeName != "Nothing"){
                        AttributeAssigner.Instance.PickAttFromString(instantiatedWeapon, attributeName);
                    }
                }
            }
        }
    }
}
