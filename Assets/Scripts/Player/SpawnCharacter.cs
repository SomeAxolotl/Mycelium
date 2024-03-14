using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    private enum TestSkills
    {
        LivingCyclone,
        RelentlessFury,
        Blitz,
        TrophicCascade,
        Mycotoxins,
        Spineshot,
        UnstablePuffball,
        Undergrowth,
        LeechingSpore,
        Sporeburst,
        DefenseMechanism
    }

    [SerializeField] private List<string> sporeNames = new List<string>()
    {
        "Gob"
    };
    private List<string> usedSporeNames = new List<string>();


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
    // Start is called before the first frame update
    void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        skillManager = GetComponent<SkillManager>();
        sporeCam = GameObject.Find("GrowCamera").GetComponent<NewSporeCam>();
    }

    // Update is called once per frame
    void Update()
    {
        //TEMPORARY KEYCODE ~ WILL BE TURNED INTO UI MENU BUTTON IN THE FUTURE

        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnNewCharacter("Poison");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SpawnNewCharacter("Default");
        }
    }
    public void SpawnNewCharacter(string subspecies)
    {
        GameObject newCharacter = Instantiate(characterPrefab);
        newCharacter.GetComponent<IdleWalking>().enabled = false;
        string subspeciesSkill = "FungalMight";
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
        CreateSpeciesPalette(newCharacter, subspecies);
        int randomMouthIndex = UnityEngine.Random.Range(0,MouthTextures.Count);
        Debug.Log("Mouth: " + randomMouthIndex);
        int randomEyeIndex = UnityEngine.Random.Range(0,EyeTextures.Count);
        Debug.Log("Eye: " + randomEyeIndex);
        DesignTracker designTracker = newCharacter.GetComponent<DesignTracker>();
        designTracker.EyeOption = randomEyeIndex;
        designTracker.MouthOption = randomMouthIndex;
        designTracker.EyeTexture = EyeTextures[randomEyeIndex];
        designTracker.MouthTexture = MouthTextures[randomMouthIndex];
        designTracker.UpdateColorsAndTexture();
        skillManager.SetSkill(subspeciesSkill, 0, newCharacter);

        swapCharacter.characters.Add(newCharacter);
        newCharacter.transform.position = GameObject.FindWithTag("PlayerSpawn").transform.position;
        sporeCam.SwitchCamera("GrowCamera");
        newCharacter.GetComponent<Animator>().Play("Sprout");
        StartCoroutine(ResetCamera(newCharacter));

        int randomNameIndex = UnityEngine.Random.Range(0, sporeNames.Count - 1);
        newCharacter.GetComponent<CharacterStats>().sporeName = sporeNames[randomNameIndex];
        usedSporeNames.Add(sporeNames[randomNameIndex]);
        sporeNames.Remove(sporeNames[randomNameIndex]);
        //newCharacter.GetComponent<CharacterStats>().ShowNametag();
        //newCharacter.GetComponent<CharacterStats>().UpdateNametagText();
    }
    IEnumerator ResetCamera(GameObject newCharacter)
    { 
        yield return new WaitForSeconds(2.5f);
        sporeCam.SwitchCamera("Main Camera");
        newCharacter.GetComponent<IdleWalking>().enabled = true;

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
}
