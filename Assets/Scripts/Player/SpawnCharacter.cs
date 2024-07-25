using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RonaldSunglassesEmoji.Personalities;
using UnityEngine.Playables;
using System.ComponentModel.Design;
using System.IO;
using System;
using System.Linq;


public class SpawnCharacter : MonoBehaviour
{
    public static SpawnCharacter Instance;

    [SerializeField] private List<string> sporeNames = new List<string>()
    {
        "Gob"
    };

    private List<GameObject> spores;
    private List<CharacterStats> sporeSkillsList;

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

    private string traitFolderPath = "Traits/";
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
        TextAsset[] textAssets = Resources.LoadAll<TextAsset>(path);
        //Debug.LogError("AMOUNT OF TRAITS: " + textAssets.Length);
        if(textAssets.Length > 0){
            string[] newStrings = new string[textAssets.Length];
            for(int i = 0; i < textAssets.Length; i++){
                newStrings[i] = textAssets[i].name;
            }
            return newStrings;
        }else{
            Debug.LogError("The specified folder path does not exist or is empty: " + path);
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

            SporePersonalities randomSporePersonality = (SporePersonalities)UnityEngine.Random.Range(0, 5);
            stats.sporePersonality = randomSporePersonality;

            stats.sporeTrait = GetRandomTrait();
            if (stats.sporeTrait != null && stats.sporeTrait != "")
            {
                Type newTrait = Type.GetType(stats.sporeTrait);
                if (newTrait != null && typeof(Component).IsAssignableFrom(newTrait))
                {
                    stats.gameObject.AddComponent(newTrait);
                    chosenTraitName = newTrait.ToString().Replace("Trait", "");
                }
                else
                {
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
            if (stats.sporeTrait != null && stats.sporeTrait != "")
            {
                Type newTrait = Type.GetType(stats.sporeTrait);
                if (newTrait != null && typeof(Component).IsAssignableFrom(newTrait))
                {
                    stats.gameObject.AddComponent(newTrait);
                    chosenTraitName = newTrait.ToString().Replace("Trait", "");
                }
                else
                {
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
            int randomMouthIndex = UnityEngine.Random.Range(0, MouthTextures.Count);
            int randomEyeIndex = UnityEngine.Random.Range(0, EyeTextures.Count);
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

        if (sporeCam != null)
        {
            sporeCam.SwitchCamera("GrowCamera");
        }
        newCharacter.GetComponent<Animator>().Play("Sprout");
        StartCoroutine(ResetCamera(newCharacter));

        spores = new List<GameObject>();
        spores.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        spores.AddRange(GameObject.FindGameObjectsWithTag("currentPlayer"));

        // Step 2: Get the CharacterStats component from each object and add it to sporeStatsList
        sporeSkillsList = new List<CharacterStats>();
        foreach (GameObject spore in spores)
        {
            CharacterStats skills = spore.GetComponent<CharacterStats>();
            if (skills != null)
            {
                sporeSkillsList.Add(skills);
            }
        }

        bool hasFungalMight = false;
        bool hasDeathBlossom = false;
        bool hasFairyRing = false;
        bool hasZombify = false;

        foreach (CharacterStats speciesSkill in sporeSkillsList)
        {
            if (speciesSkill.equippedSkills[0].Contains("FungalMight"))
            {
                hasFungalMight = true;
            }
            if (speciesSkill.equippedSkills[0].Contains("DeathBlossom"))
            {
                hasDeathBlossom = true;
            }
            if (speciesSkill.equippedSkills[0].Contains("FairyRing"))
            {
                hasFairyRing = true;
            }
            if (speciesSkill.equippedSkills[0].Contains("Zombify"))
            {
                hasZombify = true;
            }
        }

        if(hasFungalMight && hasDeathBlossom && hasFairyRing && hasZombify)
        {
            PrototypeAchievementManager.Instance.DiversityAch();
        }
    }

            //GameObject.Find("BackgroundMusicPlayer").GetComponent<CarcassSong>().PlayCarcassSong();

    string GetRandomUniqueName()
    {
        // if 0, that means it's on the first batch of names. if 1, it's on the 2nd batch
        int nameBatchIndex = swapCharacter.characters.Count / sporeNames.Count; //  120 / 100 = 1
        Debug.Log(swapCharacter.characters.Count + " / " + sporeNames.Count + " = " + nameBatchIndex);
        
        List<string> usedNames = swapCharacter.characters.Select(character => character.GetComponent<CharacterStats>().sporeName).ToList();
        
        List<string> unusedNames = new List<string>();
        // for each possible spore name,
        foreach (string possibleSporeName in sporeNames)
        {
            int usedAmount = 0;
            // cross reference it with each colony spore name, and count how many times it's used in the colony
            foreach (string usedName in usedNames)
            {
                // count how many times each colony spore name was used
                if (usedName.Contains(possibleSporeName))
                {
                    usedAmount++;
                }
            }

            // if that used amount is less than the nameBatchIndex, then add the name to unusedNames
            // for example, if a name is used 1 time, but it's still only on batch 0, then do not add it to unusedNames
            // however, if it's used 1 time, but it's now on batch 2, it can be used again
            if (usedAmount <= nameBatchIndex)
            {
                unusedNames.Add(possibleSporeName);
            }
        }

        if (unusedNames.Count == 0)
        {
            Debug.LogError("unusedNames.Count is 0 idk why");
            return "Gob";
        }

        int randomNameIndex = UnityEngine.Random.Range(0, unusedNames.Count);
        string randomName = unusedNames[randomNameIndex];

        return randomName + GetOrdinalSuffix(nameBatchIndex);
    }

    string GetOrdinalSuffix(int ordinalNumber)
    {
        string ordinalSuffix = " the ";

        switch (ordinalNumber)
        {
            case 0:
                ordinalSuffix = "";
                break;
            case 1:
                ordinalSuffix += "2nd";
                break;
            case 2:
                ordinalSuffix += "3rd";
                break;
            default:
                ordinalSuffix += (ordinalNumber + 1) + "th";
                break;
        }

        return ordinalSuffix;
    }
    
    //this shit dont work
    string GetCursedName()
    {
        string cursedLetters = " ̷̨̢̡̢̙̣̲̗̪̮̜̲̺̐̓̒̑̿́̐͂̋̊̊̋̚͠͝ ̵̧̫͚̟̲͍̫̥͇̙̲̜̻̩̩̪̉̀͊̆̊̓̒͛̌͗̈̈́̉̌ ̵̢̛̘̩̖̭̬̬̯̖̦̓̋͛̊̂͜ ̸̡̨̰̼̼͍̼̺͚̯͖̦̼̮̄̆̄̓͌̀̈͜ ̴̢̦̙͖̞͑́̓͊͗̓̿̃͂̀͝͝͝";

        int nameLength = UnityEngine.Random.Range(1, 10); // Adjust the range as needed

        string cursedName = "";
        for (int i = 0; i < nameLength; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, cursedLetters.Length);
            char randomChar = cursedLetters[randomIndex];
            cursedName.Append(randomChar);
        }

        return cursedName.ToString();
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
