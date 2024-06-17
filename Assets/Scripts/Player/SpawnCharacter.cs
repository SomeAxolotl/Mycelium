using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RonaldSunglassesEmoji.Personalities;
using UnityEngine.Playables;
using System.ComponentModel.Design;
using System.IO;
using System;


public class SpawnCharacter : MonoBehaviour
{
    public static SpawnCharacter Instance;

    [SerializeField] private List<string> sporeNames = new List<string>()
    {
        "Gob"
    };

    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private List<Texture2D> MouthTextures;
    [SerializeField] private List<Texture2D> EyeTextures;

    [Header("Default Colors")]
    [SerializeField] private List<UnityEngine.Color> defaultCapColors;
    [SerializeField] private List<UnityEngine.Color> defaultBodyColors;
    [Header("Poison Colors")]
    [SerializeField] private List<UnityEngine.Color> poisonCapColors;
    [SerializeField] private List<UnityEngine.Color> poisonBodyColors;
    [Header("Coral Colors")]
    [SerializeField] private List<UnityEngine.Color> coralCapColors;
    [SerializeField] private List<UnityEngine.Color> coralBodyColors;
    [Header("Cordyceps Colors")]
    [SerializeField] private List<UnityEngine.Color> cordycepsCapColors;
    [SerializeField] private List<UnityEngine.Color> cordycepsBodyColors;


    SwapCharacter swapCharacter;
    SkillManager skillManager;
    NewSporeCam sporeCam;

    private string traitFolderPath = "Assets/Scripts/Character/Traits/ActualTraits";
    public static string[] traitFiles;

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
        traitFiles = GetScriptFiles(traitFolderPath);
    }
    string[] GetScriptFiles(string path){
        if(Directory.Exists(path)){
            string[] newStrings = Directory.GetFiles(path, "*.cs");
            for(int i = newStrings.Length - 1; i >= 0; i--){
                newStrings[i] = newStrings[i].Replace(traitFolderPath + @"\", "");
                newStrings[i] = newStrings[i].Replace(".cs", "");
            }
            return newStrings;
        }else{
            Debug.LogError("The specified folder path does not exist: " + path);
            return new string[0];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        skillManager = GetComponent<SkillManager>();
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            sporeCam = GameObject.Find("GrowCamera").GetComponent<NewSporeCam>();
        }
        else
        {
            sporeCam = null;
        }
    }

    public void SpawnNewCharacter(string subspecies, CharacterStats customStats = null, DesignTracker customDesign = null, bool randomDesignFromSpecies = true, bool isRandomName = true)
    {
        GameObject growSpawn = GameObject.Find("GrowSpawn");

        GameObject newCharacter = Instantiate(characterPrefab, growSpawn.transform.position, Quaternion.identity);
        newCharacter.GetComponent<WanderingSpore>().enabled = false;

        string subspeciesSkill = "FungalMight";
        string statSkill1 = "NoSkill";
        string statSkill2 = "NoSkill";
        CharacterStats stats = newCharacter.GetComponent<CharacterStats>();
        string chosenTraitName = "";
        //If growing a Spore normally from Sporemother
        if (customStats == null)
        {
            switch (subspecies)
            {
                case "Default":
                    subspeciesSkill = "FungalMight";
                    break;

                case "Poison":
                    subspeciesSkill = "DeathBlossom";
                    break;

                case "Coral":
                    subspeciesSkill = "FairyRing";
                    break;

                case "Cordyceps":
                    subspeciesSkill = "Zombify";
                    break;
            }

            stats.sporeName = GetRandomUniqueName();

            SporePersonalities randomSporePersonality = (SporePersonalities) UnityEngine.Random.Range(0, 5);
            stats.sporePersonality = randomSporePersonality;

            stats.sporeTrait = GetRandomTrait();
            if(stats.sporeTrait != null && stats.sporeTrait != ""){
                Type newTrait = Type.GetType(stats.sporeTrait);
                if(newTrait != null && typeof(Component).IsAssignableFrom(newTrait)){
                    stats.gameObject.AddComponent(newTrait);
                    chosenTraitName = newTrait.ToString().Replace("Trait", "");
                }else{
                    Debug.LogError("Component type not found or is not a Component: " + stats.sporeTrait);
                }
            }
        }
        else
        {
            if (isRandomName)
            {
                stats.sporeName = GetRandomUniqueName();
            }
            else 
            {
                stats.sporeName = customStats.sporeName;
            }

            subspeciesSkill = customStats.equippedSkills[0];
            statSkill1 = customStats.equippedSkills[1];
            statSkill2 = customStats.equippedSkills[2];

            stats.sporePersonality = customStats.sporePersonality;

            stats.sporeTrait = GetRandomTrait();
            if(stats.sporeTrait != null && stats.sporeTrait != ""){
                Type newTrait = Type.GetType(stats.sporeTrait);
                if(newTrait != null && typeof(Component).IsAssignableFrom(newTrait)){
                    stats.gameObject.AddComponent(newTrait);
                    chosenTraitName = newTrait.ToString().Replace("Trait", "");
                }else{
                    Debug.LogError("Component type not found or is not a Component: " + stats.sporeTrait);
                }
            }
        }

        skillManager.SetSkill(subspeciesSkill, 0, newCharacter);
        skillManager.SetSkill(statSkill1, 1, newCharacter);
        skillManager.SetSkill(statSkill2, 2, newCharacter);

        DesignTracker designTracker = newCharacter.GetComponent<DesignTracker>();
        if (randomDesignFromSpecies)
        {
            CreateSpeciesPalette(newCharacter, subspecies);
            int randomMouthIndex = UnityEngine.Random.Range(0,MouthTextures.Count);
            int randomEyeIndex = UnityEngine.Random.Range(0,EyeTextures.Count);
            designTracker.EyeOption = randomEyeIndex;
            designTracker.MouthOption = randomMouthIndex;
            designTracker.EyeTexture = EyeTextures[randomEyeIndex];
            designTracker.MouthTexture = MouthTextures[randomMouthIndex];
            designTracker.UpdateColorsAndTexture();
        }
        else
        {
            designTracker.SetCapColor(customDesign.capColor);
            designTracker.SetBodyColor(customDesign.bodyColor);
            designTracker.EyeTexture = EyeTextures[customDesign.EyeOption];
            designTracker.MouthTexture = MouthTextures[customDesign.MouthOption];
            designTracker.UpdateColorsAndTexture();
        }

        //string coloredSporeName = "<color=#" + ColorUtility.ToHtmlStringRGB(designTracker.bodyColor) + ">" + newCharacter.GetComponent<CharacterStats>().sporeName+"</color>";
        string coloredSporeName = newCharacter.GetComponent<CharacterStats>().GetColoredSporeName();
        NotificationManager.Instance.Notification
        (
            coloredSporeName + " was born",
            "They are " + chosenTraitName
        );

        swapCharacter.characters.Add(newCharacter);

        HUDHappiness hudHappiness = GameObject.Find("HUD").GetComponent<HUDHappiness>();
        hudHappiness.ShowColonyHappinessMeter();
        HappinessManager.Instance.SporeGrown(newCharacter);
        hudHappiness.UpdateHappinessMeter();

        GameObject.Find("BackgroundMusicPlayer").GetComponent<CarcassMuffling>().CalculateMuffleSnapshot();

        if(sporeCam != null )
        {
            sporeCam.SwitchCamera("GrowCamera");
        }
        newCharacter.GetComponent<Animator>().Play("Sprout");
        StartCoroutine(ResetCamera(newCharacter));

        //GameObject.Find("BackgroundMusicPlayer").GetComponent<CarcassSong>().PlayCarcassSong();
    }

    string GetRandomUniqueName()
    {
        int randomNameIndex = UnityEngine.Random.Range(0, sporeNames.Count);
        string randomName;

        if (swapCharacter.characters.Count >= sporeNames.Count)
        {
            return sporeNames[randomNameIndex];
        }
        else
        {
            while (true)
            {
                randomNameIndex = UnityEngine.Random.Range(0, sporeNames.Count);
                randomName = sporeNames[randomNameIndex];
                bool nameIsUnique = true;
                foreach (GameObject character in swapCharacter.characters)
                {
                    if (character.GetComponent<CharacterStats>().sporeName == randomName)
                    {
                        nameIsUnique = false;
                        break;
                    }
                }

                if (nameIsUnique)
                {
                    return randomName;
                }
            }
        }
    }

    public IEnumerator ResetCamera(GameObject newCharacter)
    { 
        yield return new WaitForSeconds(2.5f);
        if (sporeCam != null)
        {
            sporeCam.SwitchCamera("Main Camera");
        }
        newCharacter.GetComponent<WanderingSpore>().enabled = true;

    }

    void CreateSpeciesPalette(GameObject character, string subspecies)
    {
        int randomColorIndex = 0;
        switch (subspecies)
        {
            case "Default":
                randomColorIndex = UnityEngine.Random.Range(0, defaultCapColors.Count);
                character.GetComponent<DesignTracker>().SetCapColor(defaultCapColors[randomColorIndex]);
                randomColorIndex = UnityEngine.Random.Range(0, defaultBodyColors.Count);
                character.GetComponent<DesignTracker>().SetBodyColor(defaultBodyColors[randomColorIndex]);
                break;

            case "Poison":
                randomColorIndex = UnityEngine.Random.Range(0, poisonCapColors.Count);
                character.GetComponent<DesignTracker>().SetCapColor(poisonCapColors[randomColorIndex]);
                randomColorIndex = UnityEngine.Random.Range(0, poisonBodyColors.Count);
                character.GetComponent<DesignTracker>().SetBodyColor(poisonBodyColors[randomColorIndex]);
                break;

            case "Coral":
                randomColorIndex = UnityEngine.Random.Range(0, coralCapColors.Count);
                character.GetComponent<DesignTracker>().SetCapColor(coralCapColors[randomColorIndex]);
                randomColorIndex = UnityEngine.Random.Range(0, coralBodyColors.Count);
                character.GetComponent<DesignTracker>().SetBodyColor(coralBodyColors[randomColorIndex]);
                break;

            case "Cordyceps":
                randomColorIndex = UnityEngine.Random.Range(0, cordycepsCapColors.Count);
                character.GetComponent<DesignTracker>().SetCapColor(cordycepsCapColors[randomColorIndex]);
                randomColorIndex = UnityEngine.Random.Range(0, cordycepsBodyColors.Count);
                character.GetComponent<DesignTracker>().SetBodyColor(cordycepsBodyColors[randomColorIndex]);
                break;
        }
    }

    public string GenerateRandomSporeName()
    {
        int randomNameIndex = UnityEngine.Random.Range(0, sporeNames.Count - 1);
        return sporeNames[randomNameIndex];
    }

    public string GetRandomTrait(){
        int randomIndex = UnityEngine.Random.Range(0, traitFiles.Length);
        Debug.Log(traitFiles[randomIndex]);
        return traitFiles[randomIndex];
    }
}
