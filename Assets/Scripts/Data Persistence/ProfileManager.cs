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

    [HideInInspector] public List<bool> tutorialIsDone = new List<bool>();

    [SerializeField] private ProfileData defaultProfileData;

    void Start()
    {
        tutorialIsDone.Add(false);
        tutorialIsDone.Add(false);
        tutorialIsDone.Add(false);

        GlobalData.profileNumber = 0;

        SetFilePath();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            //Do this when we are NOT on the Main Menu

            nutrientTrackerScript = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();

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

    void SetFilePath()
    {
        //Check profile number
        if(GlobalData.profileNumber > 2 || GlobalData.profileNumber < 0)
        {
            Debug.LogError("Bad profile number! Profile numbers must be 0, 1, or 2");
        }

        //Set the file path
        if (Application.isEditor)
        {
            filePath = Application.dataPath + "/ProfileData" + GlobalData.profileNumber + ".json";
        }
        else
        {
            filePath = Application.persistentDataPath + "/ProfileData" + GlobalData.profileNumber + ".json";
        }

        //Begin Reading ProfileData.json
        try
        {
            //Debug.Log("File Path: " + filePath);
            profileData = JsonUtility.FromJson<ProfileData>(System.IO.File.ReadAllText(filePath));
            //Debug.Log(profileData);
        }
        catch
        {
            //Debug.Log("NO PROFILE DATA FOUND!!! ---LOADING DEFAULT PROFILE DATA---");
            profileData = defaultProfileData;
            //Debug.Log(profileData);
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
        tutorialIsDone[GlobalData.profileNumber] = data.tutroialIsDone;
    }

    bool CheckTutorialCompletion()
    {
        return tutorialIsDone[GlobalData.profileNumber];
    }

    public void Save()
    {
        ProfileData newProfileData = new ProfileData();

        newProfileData.nutrients = nutrientTrackerScript.currentNutrients;

        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            //If in DA then dont save materials when quitting.
            newProfileData.log = nutrientTrackerScript.storedLog;
            newProfileData.exoskeleton = nutrientTrackerScript.storedExoskeleton;
            newProfileData.calcite = nutrientTrackerScript.storedCalcite;
            newProfileData.flesh = nutrientTrackerScript.storedFlesh;
        }
        else
        {
            //If not, save the materials when quitting
            newProfileData.log = profileData.log;
            newProfileData.exoskeleton = profileData.exoskeleton;
            newProfileData.calcite = profileData.calcite;
            newProfileData.flesh = profileData.flesh;
        }
        
        newProfileData.tutroialIsDone = tutorialIsDone[GlobalData.profileNumber];

        string json = JsonUtility.ToJson(newProfileData);
        //Debug.Log(json);
        System.IO.File.WriteAllText(filePath, json);
    }

    public void SaveOverride()
    {
        ProfileData newProfileData = new ProfileData();

        newProfileData.nutrients = nutrientTrackerScript.currentNutrients;

        newProfileData.log = nutrientTrackerScript.storedLog;
        newProfileData.exoskeleton = nutrientTrackerScript.storedExoskeleton;
        newProfileData.calcite = nutrientTrackerScript.storedCalcite;
        newProfileData.flesh = nutrientTrackerScript.storedFlesh;

        newProfileData.tutroialIsDone = tutorialIsDone[GlobalData.profileNumber];

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
