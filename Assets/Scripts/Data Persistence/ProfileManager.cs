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

        LoadProfile(profileData);
    }

    void OnApplicationQuit()
    {
        Save();
    }

    void LoadProfile(ProfileData data)
    {

    }

    void Save()
    {
        profileData = new ProfileData();

        profileData.nutrients = nutrientTrackerScript.currentNutrients;

        profileData.log = nutrientTrackerScript.storedLog;
        profileData.exoskeleton = nutrientTrackerScript.storedExoskeleton;
        profileData.calcite = nutrientTrackerScript.storedCalcite;
        profileData.flesh = nutrientTrackerScript.storedFlesh;

        profileData.tutroialIsDone = tutorialIsDone;

        //profileData
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
