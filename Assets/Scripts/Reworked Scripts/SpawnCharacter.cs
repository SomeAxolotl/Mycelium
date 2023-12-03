using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private List<Color> CapColors;
    [SerializeField] private List<Color> BodyColors;
    SwapCharacter swapCharacter;
    SkillManager skillManager;
    // Start is called before the first frame update
    void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        skillManager = GetComponent<SkillManager>();  
    }

    // Update is called once per frame
    void Update()
    {
        //TEMPORARY KEYCODE ~ WILL BE TURNED INTO UI MENU BUTTON IN THE FUTURE

        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnNewCharacter("Poison");
        }
    }
    public void SpawnNewCharacter(string subspecies)
    {
        GameObject newCharacter = Instantiate(characterPrefab);

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
        newCharacter.GetComponent<DesignTracker>().CreateSpeciesPalette(subspecies);
        skillManager.SetSkill(subspeciesSkill, 0, newCharacter);

        swapCharacter.characters.Add(newCharacter);
        newCharacter.transform.position = GameObject.FindWithTag("PlayerSpawn").transform.position;
        newCharacter.GetComponent<NewSporeAnimation>().StartGrowAnimation();

    }

    Color GetRandomColor(List<Color> colors, int colorindex)
    {
        if (colors.Count == 0 || colorindex >= colors.Count)
            return new Color(255,255,255);
        Color retColor = colors[colorindex];
        return retColor;
    }
}
