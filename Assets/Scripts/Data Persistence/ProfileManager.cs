using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    private string filePath;
    private ProfileData profileData;
    private NutrientTracker nutrientTrackerScript;

    [HideInInspector] public bool tutorialIsDone;

    [SerializeField] private ProfileData defaultProfileData;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            nutrientTrackerScript = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        }

        //Begin Reading ProfileData.json
        if (Application.isEditor)
        {
            filePath = Application.dataPath + "/ProfileData.json";
        }
        else
        {
            filePath = Application.persistentDataPath + "/ProfileData.json";
        }

        try
        {
            Debug.Log("File Path: " + filePath);
            Debug.Log(System.IO.File.ReadAllText(filePath));
            profileData = JsonUtility.FromJson<ProfileData>(System.IO.File.ReadAllText(filePath));
            Debug.Log(profileData);
        }
        catch
        {
            Debug.Log("NO PROFILE DATA FOUND!!! ---LOADING DEFAULT PROFILE DATA---");

            profileData = defaultProfileData;
            Debug.Log(profileData);
        }

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            LoadProfile(profileData);
        }

        LoadTutorialCompletion(profileData);
    }

    void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Save();
        }
    }

    void LoadProfile(ProfileData data)
    {
        nutrientTrackerScript.SetNutrients(data.nutrients);

        nutrientTrackerScript.storedLog = data.log;
        nutrientTrackerScript.storedExoskeleton = data.exoskeleton;
        nutrientTrackerScript.storedCalcite = data.calcite;
        nutrientTrackerScript.storedFlesh = data.flesh;
    }

    void LoadTutorialCompletion(ProfileData data)
    {
        tutorialIsDone = data.tutroialIsDone;
    }

    bool CheckTutorialCompletion(int profileNumber)
    {
        return tutorialIsDone;
    }

    public void Save()
    {
        ProfileData newProfileData = new ProfileData();

        newProfileData.nutrients = nutrientTrackerScript.currentNutrients;

        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            newProfileData.log = nutrientTrackerScript.storedLog;
            newProfileData.exoskeleton = nutrientTrackerScript.storedExoskeleton;
            newProfileData.calcite = nutrientTrackerScript.storedCalcite;
            newProfileData.flesh = nutrientTrackerScript.storedFlesh;
        }
        else
        {
            newProfileData.log = profileData.log;
            newProfileData.exoskeleton = profileData.exoskeleton;
            newProfileData.calcite = profileData.calcite;
            newProfileData.flesh = profileData.flesh;
        }
        
        newProfileData.tutroialIsDone = tutorialIsDone;

        string json = JsonUtility.ToJson(newProfileData);
        Debug.Log(json);
        System.IO.File.WriteAllText(filePath, json);
    }

    [Serializable]
    public class ProfileData
    {
        public int nutrients;

        public int log;
        public int exoskeleton;
        public int calcite;
        public int flesh;

        public bool tutroialIsDone;

        public override string ToString()
        {
            return
            (
                "Nutrients: " + nutrients +
                "\nLogs: " + log +
                "\nExoskeletons: " + exoskeleton +
                "\nCalcites: " + calcite +
                "\nFlesh: " + flesh +
                "\nTutorial Is Done:  " + tutroialIsDone
            );
        }

    }
}
