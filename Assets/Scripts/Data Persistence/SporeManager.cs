using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

public class SporeManager : MonoBehaviour
{
    [Tooltip("The list of Spores to be saved. Populates on saving.")] private List<GameObject> spores;
    [Tooltip("What is saved to json. A custom class that is a list of sporeData objects")] private SporeDataList sporeDataList;
    [Tooltip("Where we are saving our json.")] private string filePath;
    [Tooltip("The Spore Spawnpoint transform")] private Transform spawnTransform;
    SwapCharacter swapCharacterScript;
    SkillManager skillManagerScript;
    GameObject currentPlayerSpore;

    [SerializeField] private GameObject SporePrefab;
    [SerializeField][Tooltip("The range on the x and z axis around the spawn transform where spores should be generated")][Range(0f, 10f)] private float spawnRange;
    [SerializeField] SporeDataList defaultSporeData;
    

    void Start()
    {
        spawnTransform = gameObject.transform;
        swapCharacterScript = GameObject.Find("PlayerParent").GetComponent<SwapCharacter>();
        skillManagerScript = GameObject.Find("PlayerParent").GetComponent<SkillManager>();
        currentPlayerSpore = GameObject.Find("Spore");

        swapCharacterScript.characters.RemoveAll(item => item == null);
        swapCharacterScript.currentCharacterIndex = swapCharacterScript.characters.IndexOf(GameObject.FindWithTag("currentPlayer"));

        //if(PlayerParent.activeSelf == true)                   //Necessary to have the SwapCharacters function work, unless SwapCharacters is reworked separately
        //    PlayerParent.SetActive(false);

        //Begin Reading SporeData.json
        if (Application.isEditor)
        {
            filePath = Application.dataPath + "/SporeData.json";
        }
        else
        {
            filePath = Application.persistentDataPath + "/SporeData.json";
        }

        try
        {
            Debug.Log("File Path: " + filePath);
            Debug.Log(System.IO.File.ReadAllText(filePath));
            sporeDataList = JsonUtility.FromJson<SporeDataList>(System.IO.File.ReadAllText(filePath));
            Debug.Log(sporeDataList);
        }
        catch
        {
            Debug.Log("NO SPORES FOUND!!! ---LOADING DEFAULT SPORE DATA---");

            sporeDataList = defaultSporeData;
            Debug.Log(sporeDataList);
        }

        //For each SporeData in the json, populate its stats and design
        foreach (SporeData sporeData in sporeDataList.Spore_Data)
        {
            LoadSpores(sporeData);
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
            Debug.Log("Loading Main Spore: " + sporeData.sporeName);

            Spore = currentPlayerSpore;

            stats = currentPlayerSpore.GetComponent<CharacterStats>();
            design = currentPlayerSpore.GetComponent<DesignTracker>();

            stats.HideNametag();
        }
        else
        {
            Debug.Log("Loading Other Spore: " + sporeData.sporeName);

            //Spawn Spore Prefab
            Spore = Instantiate(SporePrefab, new Vector3(UnityEngine.Random.Range(spawnTransform.position.x - spawnRange, spawnTransform.position.x + spawnRange), spawnTransform.position.y, UnityEngine.Random.Range(spawnTransform.position.z - spawnRange, spawnTransform.position.z + spawnRange)), spawnTransform.rotation);

            //Set references to prefab's components
            stats = Spore.GetComponent<CharacterStats>();
            design = Spore.GetComponent<DesignTracker>();

            //Add the spore to the characters index
            swapCharacterScript.characters.Add(Spore);

            stats.ShowNametag();
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

        //Run Spore Setup functions
        stats.UpdateSporeName();
        design.UpdateBlendshape(sporeData.lvlSentience, sporeData.lvlPrimal, sporeData.lvlVitality, sporeData.lvlSpeed);
        design.UpdateColors();

        //-------Some Dylan Comments-----------
        //design.ForceUpdateBlendshaped(sporeData.lvlSentience,sporeData.lvlPrimal,sporeData.lvlVitality, sporeData.lvlSpeed);      //<---- Moved to Start() for CharacterStats
        //PlayerParent.GetComponent<SwapCharacter>().characters.Add(Spore);     //<---- Unnecessary if PlayerParent does not exist in Spore Manager

        //PlayerParent.SetActive(true);
        //testingManager.gameObject.SetActive(true);
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

                sporeDataList.Spore_Data.Add(currentSporeData);
            }
            string json = JsonUtility.ToJson(sporeDataList);

            json = json.Replace(":[{", ":[\n\t{");
            json = json.Replace("},{", "},\n\t{");
            json = json.Replace("]}", "\n]}");

            Debug.Log(json);
            System.IO.File.WriteAllText(filePath, json);
        }
    }

    IEnumerator StaggerSkillSets(SporeData sporeData, GameObject Spore)
    {
        try
        {
            skillManagerScript.SetSkill(sporeData.skillSlot0, 0, Spore);
        }
        catch
        {

        }

        yield return new WaitForSecondsRealtime(0.1f);

        try
        {
            skillManagerScript.SetSkill(sporeData.skillSlot1, 1, Spore);
        }
        catch
        {

        }

        yield return new WaitForSecondsRealtime(0.1f);

        try
        {
            skillManagerScript.SetSkill(sporeData.skillSlot2, 2, Spore);
        }
        catch
        {

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
            "\nCapColor: " + capColor +
            "\nBodyColor: " + bodyColor
        );
    }

}
