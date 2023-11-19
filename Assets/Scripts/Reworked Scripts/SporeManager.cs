using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

public class SporeManager : MonoBehaviour
{
   [Tooltip("The list of Spores to be saved. Populates on saving.")] private List<GameObject> spores;
   [Tooltip("What is saved to json. A custom class that is a list of sporeData objects")] private SporeDataList sporeDataList;
   [Tooltip("Where we are saving our json.")]private string filePath;
   [Tooltip("The Spore Spawnpoint transform")] private Transform spawnTransform;

   [SerializeField] private GameObject SporePrefab;
   [SerializeField][Tooltip("The range on the x and z axis around the spawn transform where spores should be generated")][Range(0f, 10f)] private float spawnRange;

   void Start()
   {
    spawnTransform = gameObject.transform;
    //if(PlayerParent.activeSelf == true)                   //Necessary to have the SwapCharacters function work, unless SwapCharacters is reworked separately
    //    PlayerParent.SetActive(false);

    //Begin Reading SporeData.json
    filePath = Application.persistentDataPath + "/SporeData.json";
    Debug.Log("File Path: " + filePath);
    Debug.Log(System.IO.File.ReadAllText(filePath));
    sporeDataList = JsonUtility.FromJson<SporeDataList>(System.IO.File.ReadAllText(filePath));
    Debug.Log(sporeDataList);
    GameObject Spore;

    //For each SporeData in the json, populate its stats and design
    foreach(SporeData sporeData in sporeDataList.Spore_Data)
    {
        Debug.Log("SPORE DETECTED");
        Spore = Instantiate(SporePrefab, new Vector3(UnityEngine.Random.Range(spawnTransform.position.x - spawnRange, spawnTransform.position.x + spawnRange), spawnTransform.position.y, UnityEngine.Random.Range(spawnTransform.position.z - spawnRange, spawnTransform.position.z + spawnRange)), spawnTransform.rotation);
        DesignTracker design = Spore.GetComponent<DesignTracker>();
        CharacterStats stats = Spore.GetComponent<CharacterStats>();
        design.bodyColor = sporeData.bodyColor;
        design.capColor = sporeData.capColor;
        stats.primalLevel = sporeData.lvlPrimal;
        stats.sentienceLevel = sporeData.lvlSentience;
        stats.speedLevel = sporeData.lvlSpeed;
        stats.vitalityLevel = sporeData.lvlVitality;
        //design.ForceUpdateBlendshaped(sporeData.lvlSentience,sporeData.lvlPrimal,sporeData.lvlVitality, sporeData.lvlSpeed);      //<---- Moved to Start() for CharacterStats
        //PlayerParent.GetComponent<SwapCharacter>().characters.Add(Spore);     //<---- Unnecessary if PlayerParent does not exist in Spore Manager
    }
    //PlayerParent.SetActive(true);
    //testingManager.gameObject.SetActive(true);
    }

    void OnApplicationQuit(){
        Save();
    }

   void Save(){
        spores = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>();
        spores.Add(GameObject.FindGameObjectWithTag("currentPlayer"));
        sporeDataList = new SporeDataList
            {
                Spore_Data = new List<SporeData>()
            };
        foreach(GameObject spore in spores)
        {
            if(spore.CompareTag("Player") || spore.CompareTag("currentPlayer"))
            {
                SporeData currentSporeData = new SporeData();
                DesignTracker currentSporeDesign = spore.GetComponent<DesignTracker>();
                CharacterStats currentSporeStats = spore.GetComponent<CharacterStats>();

                currentSporeData.lvlPrimal = currentSporeStats.primalLevel;
                currentSporeData.lvlSentience = currentSporeStats.sentienceLevel;
                currentSporeData.lvlSpeed = currentSporeStats.speedLevel;
                currentSporeData.lvlVitality = currentSporeStats.vitalityLevel;
                currentSporeData.bodyColor = currentSporeDesign.bodyColor;
                currentSporeData.capColor = currentSporeDesign.capColor;
                
                sporeDataList.Spore_Data.Add(currentSporeData);
            }
            string json = JsonUtility.ToJson(sporeDataList);
            Debug.Log(json);
            System.IO.File.WriteAllText(filePath, json);
        }
   }
   
   
}

//[SporeDataList]- Class that is saved to Json: a list of SporeData class objects
[Serializable] public class SporeDataList
{
    public List<SporeData> Spore_Data;
}

//[SporeData]- All of the data to be saved for a Spore
[Serializable] public class SporeData
{
    public int lvlPrimal;
    public int lvlSentience;
    public int lvlSpeed;
    public int lvlVitality;
    public Color bodyColor;
    public Color capColor;
    public string sporeName = "NA";

    public override string ToString(){
        return ("Primal: " + lvlPrimal +
        "\nSentience: " + lvlSentience +
        "\nSpeed: " + lvlSpeed +
        "\nVitality: " + lvlVitality +
        "\nBodyColor: " + bodyColor +
        "\nCapColor: " + capColor +
        "\nName: " + sporeName);
    }

}
