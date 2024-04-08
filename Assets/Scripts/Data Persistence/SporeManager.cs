using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

public class SporeManager : SporeManagerSystem
{
    [Tooltip("The list of Spores to be saved. Populates on saving.")] private List<GameObject> spores;
    [Tooltip("The Spore Spawnpoint transform")] private Transform spawnTransform;

    [SerializeField] private GameObject SporePrefab;
    [SerializeField][Tooltip("The range on the x and z axis around the spawn transform where spores should be generated")][Range(0f, 10f)] private float spawnRange;
    [SerializeField] SporeDataList defaultSporeData;

    protected override void Start()
    {
        base.Start();

        spawnTransform = gameObject.transform;

        SetPathAndData(GlobalData.profileNumber);

        //For each SporeData in the json, populate its stats and design
        foreach (SporeData sporeData in sporeDataList.Spore_Data)
        {
            LoadSpores(sporeData);
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
            //Debug.Log("File Path: " + filePath);
            //Debug.Log(System.IO.File.ReadAllText(filePath));
            sporeDataList = JsonUtility.FromJson<SporeDataList>(System.IO.File.ReadAllText(filePath));
            //Debug.Log(sporeDataList);
        }
        catch
        {
            //Debug.Log("NO SPORES FOUND!!! ---LOADING DEFAULT SPORE DATA---");

            sporeDataList = defaultSporeData;
            //Debug.Log(sporeDataList);
        }
    }

    void OnApplicationQuit()
    {
        Save();
    }

    void LoadSpores(SporeData sporeData)
    {
        GameObject Spore;
        CharacterStats stats;
        DesignTracker design;

        if (sporeData.sporeTag == "currentPlayer")
        {
            //Debug.Log("Loading Main Spore: " + sporeData.sporeName);

            Spore = currentPlayerSpore;

            stats = currentPlayerSpore.GetComponent<CharacterStats>();
            design = currentPlayerSpore.GetComponent<DesignTracker>();

            //stats.HideNametag();
        }
        else
        {
            //Debug.Log("Loading Other Spore: " + sporeData.sporeName);

            //Spawn Spore Prefab
            Spore = Instantiate(SporePrefab, new Vector3(UnityEngine.Random.Range(spawnTransform.position.x - spawnRange, spawnTransform.position.x + spawnRange), spawnTransform.position.y, UnityEngine.Random.Range(spawnTransform.position.z - spawnRange, spawnTransform.position.z + spawnRange)), spawnTransform.rotation);

            //Set references to prefab's components
            stats = Spore.GetComponent<CharacterStats>();
            design = Spore.GetComponent<DesignTracker>();

            //Add the spore to the characters index
            swapCharacterScript.characters.Add(Spore);

            //stats.ShowNametag();
        }

        //Load the saved data into the prefab
        stats.sporeName = sporeData.sporeName;
        //Spore.gameObject.tag = sporeData.sporeTag;

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

    public void Save()
    {
        spores = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>();
        spores.Add(GameObject.FindGameObjectWithTag("currentPlayer"));
        sporeDataList = new SporeDataList
        {
            Spore_Data = new List<SporeData>()
        };
        foreach (GameObject spore in spores)
        {
            if (spore.CompareTag("Player") || spore.CompareTag("currentPlayer"))
            {
                SporeData currentSporeData = new SporeData();
                DesignTracker currentSporeDesign = spore.GetComponent<DesignTracker>();
                CharacterStats currentSporeStats = spore.GetComponent<CharacterStats>();

                //From the Character Stats
                currentSporeData.sporeName = currentSporeStats.sporeName;
                currentSporeData.sporeTag = spore.tag;

                currentSporeData.skillSlot0 = currentSporeStats.equippedSkills[0];
                currentSporeData.skillSlot1 = currentSporeStats.equippedSkills[1];
                currentSporeData.skillSlot2 = currentSporeStats.equippedSkills[2];

                currentSporeData.lvlPrimal = currentSporeStats.primalLevel;
                currentSporeData.lvlSpeed = currentSporeStats.speedLevel;
                currentSporeData.lvlSentience = currentSporeStats.sentienceLevel;
                currentSporeData.lvlVitality = currentSporeStats.vitalityLevel;

                //From the Design Tracker
                currentSporeData.bodyColor = currentSporeDesign.bodyColor;
                currentSporeData.capColor = currentSporeDesign.capColor;
                currentSporeData.mouthOption = currentSporeDesign.MouthOption;
                currentSporeData.eyeOption = currentSporeDesign.EyeOption;

                sporeDataList.Spore_Data.Add(currentSporeData);
            }
            string json = JsonUtility.ToJson(sporeDataList);

            json = json.Replace(":[{", ":[\n\t{");
            json = json.Replace("},{", "},\n\t{");
            json = json.Replace("]}", "\n]}");

            //Debug.Log(json);
            System.IO.File.WriteAllText(filePath, json);
        }
    }
}

//[SporeDataList]- Class that is saved to Json: a list of SporeData class objects
[Serializable]
public class SporeDataList
{
    public List<SporeData> Spore_Data;
}

//[SporeData]- All of the data to be saved for a Spore
[Serializable]
public class SporeData
{
    public string sporeName;
    public string sporeTag;

    public string skillSlot0;
    public string skillSlot1;
    public string skillSlot2;

    public int lvlPrimal;
    public int lvlSpeed;
    public int lvlSentience;
    public int lvlVitality;

    public Color bodyColor;
    public Color capColor;
    public int mouthOption;
    public int eyeOption;

    public override string ToString()
    {
        return
        (
            "Name: " + sporeName +
            "\nName: " + sporeTag +
            "\nSkill Slot 0: " + skillSlot0 +
            "\nSkill Slot 1: " + skillSlot1 +
            "\nSkill Slot 2: " + skillSlot2 +
            "\nPrimal:  " + lvlPrimal +
            "\nSpeed: " + lvlSpeed +
            "\nSentience: " + lvlSentience +
            "\nVitality: " + lvlVitality +
            "\nBodyColor: " + bodyColor +
            "\nCapColor: " + capColor +
            "\nMouthOption: " + mouthOption +
            "\nEyeOption: " + eyeOption
        );
    }

}
