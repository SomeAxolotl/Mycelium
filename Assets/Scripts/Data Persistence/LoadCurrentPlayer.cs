using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using RonaldSunglassesEmoji.Personalities;

public class LoadCurrentPlayer : SporeManagerSystem
{
    public static LoadCurrentPlayer Instance;

    [SerializeField] private SporeData tutorialSpore;

    private SporeData currentPlayerSporeData;

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

    protected override void Start()
    {
        base.Start();

        SetPathAndData(GlobalData.profileNumber);

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            LoadSpores(tutorialSpore);
        }
        else
        {
            //Find the currentPlayer spore and populate its stats and design
            foreach (SporeData sporeData in sporeDataList.Spore_Data)
            {
                if (sporeData.sporeTag == "currentPlayer")
                {
                    LoadSpores(sporeData);
                    currentPlayerSporeData = sporeData;
                }
            }
        }
    }

    void SetPathAndData(int profileNumber)
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

        try
        {
            sporeDataList = JsonUtility.FromJson<SporeDataList>(System.IO.File.ReadAllText(filePath));
        }
        catch
        {
            Debug.LogWarning("No SporeData.json file! If this is the tutorial or main menu this is ok.");
        }
    }

    void LoadSpores(SporeData sporeData)
    {
        GameObject Spore;
        CharacterStats stats;
        DesignTracker design;

        //Debug.Log("Loading Main Spore: " + sporeData.sporeName);

        Spore = currentPlayerSpore;

        stats = currentPlayerSpore.GetComponent<CharacterStats>();
        design = currentPlayerSpore.GetComponent<DesignTracker>();

        //stats.HideNametag();

        //Load the saved data into the prefab
        stats.sporeName = sporeData.sporeName;
        //Spore.gameObject.tag = sporeData.sporeTag;

        stats.sporeHappiness = sporeData.sporeHappiness;
        stats.sporePersonality = (SporePersonalities)sporeData.sporePersonality;
        stats.sporeEnergy = sporeData.sporeEnergy;

        stats.sporeTrait = sporeData.sporeTrait;
        //Debug.Log("Trait is: " + sporeData.sporeTrait);
        if(sporeData.sporeTrait != null && sporeData.sporeTrait != ""){
            Type newTrait = Type.GetType(sporeData.sporeTrait);
            if(newTrait != null && typeof(Component).IsAssignableFrom(newTrait)){
                Spore.AddComponent(newTrait);
            }else{
                Debug.LogError("Component type not found or is not a Component: " + sporeData.sporeTrait);
            }
        }

        stats.O_highestLoopBeaten = sporeData.highestLoopBeaten;

        StartCoroutine(StaggerSkillSets(sporeData, Spore));

        stats.primalLevel = sporeData.lvlPrimal;
        stats.speedLevel = sporeData.lvlSpeed;
        stats.sentienceLevel = sporeData.lvlSentience;
        stats.vitalityLevel = sporeData.lvlVitality;

        design.bodyColor = sporeData.bodyColor;
        design.capColor = sporeData.capColor;
        design.EyeOption = sporeData.eyeOption;
        design.MouthOption = sporeData.mouthOption;
        design.EyeTexture = EyeTextures[sporeData.eyeOption];
        design.MouthTexture = MouthTextures[sporeData.mouthOption];

        //Run Spore Setup functions
        StartCoroutine(RunSporeSetup(sporeData, stats, design));
    }

    public void DeleteCurrentPlayerSpore()
    {
        if(sporeDataList == null)
        {
            Debug.LogError("No sporeDataList exists. Go bother ryan about this", gameObject);
            //yo
            return;
        }

        sporeDataList.Spore_Data.Remove(currentPlayerSporeData);

        if(sporeDataList.Spore_Data.Count != 0)
        {
            sporeDataList.Spore_Data[0].sporeTag = "currentPlayer";

            string json = JsonUtility.ToJson(sporeDataList);

            json = json.Replace(":[{", ":[\n\t{");
            json = json.Replace("},{", "},\n\t{");
            json = json.Replace("]}", "\n]}");

            File.WriteAllText(filePath, json);
        }
        else
        {
            File.Delete(filePath);
        }
    }
}
