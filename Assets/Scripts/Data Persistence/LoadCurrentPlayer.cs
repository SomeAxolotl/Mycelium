using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadCurrentPlayer : MonoBehaviour
{
    [Tooltip("A custom class that is a list of sporeData objects")] private SporeDataList sporeDataList;
    [Tooltip("Where the .json is")] private string filePath;
    [SerializeField] private SporeData tutorialSpore;
    [SerializeField] private List<Texture2D> MouthTextures;
    [SerializeField] private List<Texture2D> EyeTextures;
    SwapCharacter swapCharacterScript;
    SkillManager skillManagerScript;
    PlayerHealth playerHealthScript;
    PlayerController playerControllerScript;
    GameObject currentPlayerSpore;

    void Start()
    {
        swapCharacterScript = GameObject.Find("PlayerParent").GetComponent<SwapCharacter>();
        skillManagerScript = GameObject.Find("PlayerParent").GetComponent<SkillManager>();
        playerHealthScript = GameObject.Find("PlayerParent").GetComponent<PlayerHealth>();
        playerControllerScript = GameObject.Find("PlayerParent").GetComponent<PlayerController>();
        currentPlayerSpore = GameObject.Find("Spore");

        swapCharacterScript.characters.RemoveAll(item => item == null);
        swapCharacterScript.currentCharacterIndex = swapCharacterScript.characters.IndexOf(GameObject.FindWithTag("currentPlayer"));

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
            sporeDataList = JsonUtility.FromJson<SporeDataList>(System.IO.File.ReadAllText(filePath));
        }
        catch
        {
            Debug.LogWarning("No SporeData.json file! If this is the tutorial or main menu this is ok.");

            //Application.Quit();

        }

        if(SceneManager.GetActiveScene().buildIndex == 1)
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

    IEnumerator RunSporeSetup(SporeData sporeData, CharacterStats stats, DesignTracker design)
    {
        //Delay by one frame
        yield return null;

        stats.UpdateSporeName();
        stats.StartCalculateAttributes();
        design.UpdateBlendshape(sporeData.lvlSentience, sporeData.lvlPrimal, sporeData.lvlVitality, sporeData.lvlSpeed);
        design.UpdateColorsAndTexture();
        playerHealthScript.ResetHealth();
        playerControllerScript.GetStats();
        stats.GetComponent<Animator>().speed = stats.animatorSpeed;
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
