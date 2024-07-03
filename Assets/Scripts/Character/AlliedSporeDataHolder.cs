using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using RonaldSunglassesEmoji.Personalities;
using JetBrains.Annotations;

public class AlliedSporeDataHolder : SporeManagerSystem
{
    [SerializeField] private GameObject alliedSporePrefab;
    private List<string> loadedSporeNames = new List<string>();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        GetJSONdata(GlobalData.profileNumber);
        //InvokeRepeating("LoadRandomAlliedSpore", 3f, 3f);
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
    void LoadRandomAlliedSpore()
    {
        GameObject loadedSpore;
        CharacterStats loadedSporeStats;
        DesignTracker loadedSporeDesign;
        List<SporeData> availableSpores = sporeDataList.Spore_Data.FindAll(spore => !loadedSporeNames.Contains(spore.sporeName));

        if (availableSpores.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableSpores.Count);
            SporeData selectedSpore = availableSpores[randomIndex];
            loadedSporeNames.Add(selectedSpore.sporeName);

            loadedSpore = Instantiate(alliedSporePrefab);
            loadedSpore.transform.position = GameObject.FindWithTag("currentPlayer").transform.position + new Vector3 (0,15,0);
            loadedSporeStats = loadedSpore.GetComponent<CharacterStats>();
            loadedSporeDesign = loadedSpore.GetComponent<DesignTracker>();

            loadedSporeStats.sporeName = selectedSpore.sporeName;
            loadedSporeStats.sporeHappiness = selectedSpore.sporeHappiness;
            loadedSporeStats.sporePersonality = (SporePersonalities)selectedSpore.sporePersonality;
            loadedSporeStats.sporeEnergy = selectedSpore.sporeEnergy;
            loadedSporeStats.sporeTrait = selectedSpore.sporeTrait;

            loadedSporeStats.primalLevel = selectedSpore.lvlPrimal;
            loadedSporeStats.speedLevel = selectedSpore.lvlSpeed;
            loadedSporeStats.sentienceLevel = selectedSpore.lvlSentience;
            loadedSporeStats.vitalityLevel = selectedSpore.lvlVitality;

            loadedSporeDesign.bodyColor = selectedSpore.bodyColor;
            loadedSporeDesign.capColor = selectedSpore.capColor;
            loadedSporeDesign.EyeOption = selectedSpore.eyeOption;
            loadedSporeDesign.MouthOption = selectedSpore.mouthOption;
            loadedSporeDesign.UpdateColorsAndTexture();
            loadedSporeDesign.UpdateBlendshape(loadedSporeStats.sentienceLevel, loadedSporeStats.primalLevel, loadedSporeStats.vitalityLevel, loadedSporeStats.speedLevel);
            //loadedSporeDesign.EyeTexture = EyeTextures[selectedSpore.eyeOption];
            //loadedSporeDesign.MouthTexture = MouthTextures[selectedSpore.mouthOption];
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

