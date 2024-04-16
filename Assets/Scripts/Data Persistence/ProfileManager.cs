using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;

    private string filePath;
    public ProfileData profileData;
    private NutrientTracker nutrientTrackerScript;
    private FurnitureManager furnitureManagerScript;

    [HideInInspector] public List<bool> tutorialIsDone = new List<bool>();
    [HideInInspector] public List<bool> permadeathIsOn = new List<bool>();

    [SerializeField] private ProfileData defaultProfileData;

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

    void Start()
    {
        if(GlobalData.gameIsStarting == true)
        {
            GlobalData.profileNumber = 0;
        }

        SetPathAndData(GlobalData.profileNumber);

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            //Do this when we are NOT on the Main Menu

            nutrientTrackerScript = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
            furnitureManagerScript = GameObject.Find("FurnitureManager").GetComponent<FurnitureManager>();

            LoadProfile(profileData);
        }

        LoadTutorialCompletion();
        LoadPermadeathData();
    }

    void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Save();
        }
    }

    void SetPathAndData(int profileNumber)
    {
        //Check profile number
        if(profileNumber > 2 || profileNumber < 0)
        {
            Debug.LogError("Bad profile number! Profile numbers must be 0, 1, or 2");
        }

        //Set the file path
        if (Application.isEditor)
        {
            filePath = Application.dataPath + "/ProfileData" + profileNumber + ".json";
        }
        else
        {
            filePath = Application.persistentDataPath + "/ProfileData" + profileNumber + ".json";
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

        furnitureManagerScript.bedIsUnlocked = data.bedIsUnlocked;
        furnitureManagerScript.drumIsUnlocked = data.drumIsUnlocked;
        furnitureManagerScript.chairIsUnlocked = data.chairIsUnlocked;
        furnitureManagerScript.fireflyIsUnlocked = data.fireflyIsUnlocked;
        furnitureManagerScript.gameboardIsUnlocked = data.gameboardIsUnlocked;
        furnitureManagerScript.fireIsUnlocked = data.fireIsUnlocked;
    }

    void LoadTutorialCompletion()
    {
        for(int i = 0; i <= 2; i++)
        {
            SetPathAndData(i);
            tutorialIsDone.Add(profileData.tutroialIsDone);
        }

        SetPathAndData(GlobalData.profileNumber);
    }

    void LoadPermadeathData()
    {
        for (int i = 0; i <= 2; i++)
        {
            SetPathAndData(i);
            permadeathIsOn.Add(profileData.permadeathIsOn);
        }

        SetPathAndData(GlobalData.profileNumber);
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

        newProfileData.bedIsUnlocked = furnitureManagerScript.bedIsUnlocked;
        newProfileData.drumIsUnlocked = furnitureManagerScript.drumIsUnlocked;
        newProfileData.chairIsUnlocked = furnitureManagerScript.chairIsUnlocked;
        newProfileData.fireflyIsUnlocked = furnitureManagerScript.fireflyIsUnlocked;
        newProfileData.gameboardIsUnlocked = furnitureManagerScript.gameboardIsUnlocked;
        newProfileData.fireIsUnlocked = furnitureManagerScript.fireIsUnlocked;

        newProfileData.tutroialIsDone = tutorialIsDone[GlobalData.profileNumber];
        newProfileData.permadeathIsOn = permadeathIsOn[GlobalData.profileNumber];

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

        newProfileData.bedIsUnlocked = furnitureManagerScript.bedIsUnlocked;
        newProfileData.drumIsUnlocked = furnitureManagerScript.drumIsUnlocked;
        newProfileData.chairIsUnlocked = furnitureManagerScript.chairIsUnlocked;
        newProfileData.fireflyIsUnlocked = furnitureManagerScript.fireflyIsUnlocked;
        newProfileData.gameboardIsUnlocked = furnitureManagerScript.gameboardIsUnlocked;
        newProfileData.fireIsUnlocked = furnitureManagerScript.fireIsUnlocked;

        newProfileData.tutroialIsDone = tutorialIsDone[GlobalData.profileNumber];
        newProfileData.permadeathIsOn = permadeathIsOn[GlobalData.profileNumber];

        string json = JsonUtility.ToJson(newProfileData);
        Debug.Log(json);
        System.IO.File.WriteAllText(filePath, json);
    }

    public void ActivatePermadeath() //ONLY USE THIS FUNCTION WHEN CREATING A NEW PROFILE!!! WILL SET EVERYTHING ELSE TO DEFAULT VALUES!!!
    {
        ProfileData newProfileData = new ProfileData();

        SetPathAndData(GlobalData.profileNumber);

        permadeathIsOn[GlobalData.profileNumber] = true;
        newProfileData.permadeathIsOn = permadeathIsOn[GlobalData.profileNumber];

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
        public bool permadeathIsOn;

        public bool bedIsUnlocked;
        public bool drumIsUnlocked;
        public bool chairIsUnlocked;
        public bool fireflyIsUnlocked;
        public bool gameboardIsUnlocked;
        public bool fireIsUnlocked;

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
