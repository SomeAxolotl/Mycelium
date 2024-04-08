using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadCurrentPlayer : SporeManagerSystem
{
    [SerializeField] private SporeData tutorialSpore;

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
            return;
        }

        foreach (SporeData sporeData in sporeDataList.Spore_Data)
        {
            if (sporeData.sporeTag == "currentPlayer")
            {
                sporeDataList.Spore_Data.Remove(sporeData);
            }
        }

        sporeDataList.Spore_Data[0].sporeTag = "currentPlayer";
    }
}
