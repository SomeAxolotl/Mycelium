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
            SpawnNewCharacter();
        }
    }
    void SpawnNewCharacter()
    {
        GameObject newCharacter = Instantiate(characterPrefab); //WE HAVE TO EVENTUALLY BE ABLE TO SPAWN 4 DIFF TYPES OF SHROOMS, RIGHT NOW THIS IS JUST A SINGLE TYPE
        DesignTracker designTracker = newCharacter.GetComponent<DesignTracker>();
        int colorindex = Random.Range(0,BodyColors.Count);
        designTracker.SetBodyColor(GetRandomColor(BodyColors, colorindex));
        designTracker.SetCapColor(GetRandomColor(CapColors, colorindex));
        swapCharacter.characters.Add(newCharacter);
        newCharacter.transform.position = GameObject.FindWithTag("PlayerSpawn").transform.position;
        StartCoroutine(SetTestSkills(newCharacter));
        newCharacter.GetComponent<NewSporeAnimation>().StartGrowAnimation();

    }

    IEnumerator SetTestSkills(GameObject newCharacter)
    {
        TestSkills randomSkill = (TestSkills)Random.Range(0, 10);
        string randomSkillString = randomSkill.ToString();

        skillManager.SetSkill("FungalMight", 0, newCharacter);
        yield return null;
        skillManager.SetSkill("Eruption", 1, newCharacter);
        yield return null;
        skillManager.SetSkill(randomSkillString, 2, newCharacter);
        yield return null;
    }

    Color GetRandomColor(List<Color> colors, int colorindex)
    {
        if (colors.Count == 0 || colorindex >= colors.Count)
            return new Color(255,255,255);
        Color retColor = colors[colorindex];
        return retColor;
    }
}
