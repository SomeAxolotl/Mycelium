using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private enum Names
    {
        Gidego,
        Gideo,
        Shborb,
        GidShbeeb,
        Shbaybo,
        Gidoof,
        Gob,
        Shbob,
        Shbeeby,
    }
    private Names thisName;

    public Dictionary<string, bool> skillUnlocks = new Dictionary<string, bool>()
    {
        {"Eruption", false},
        {"LivingCyclone", false},
        {"RelentlessFury", false},
        {"Blitz", false},
        {"TrophicCascade", false},
        {"Mycotoxins", false},
        {"Spineshot", false},
        {"UnstablePuffball", false},
        {"Undergrowth", false},
        {"LeechingSpore", false},
        {"Sporeburst", false},
        {"DefenseMechanism", false}
    };

    //Displaying current level
    [Header("Primal Level")]
    public int primalLevel;
    //Primal Base Stats
    public float primalDmg = 10f;
    
    [Header("Speed Level")]
    public int speedLevel;
    //Speed Base Stats
    public float moveSpeed = 5f;
    public float atkCooldownBuff = 0f;
    public float minAttackSpeed;
    public float maxAttackSpeed;
    
    [Header("Sentience Level")]
    public int sentienceLevel;
    
    [Header("Vitality Level")]
    public int vitalityLevel;
    //Vitality Base Stats
    public float baseHealth = 100f;
    public float baseRegen = .2f;
    
    public int totalLevel;
    public int levelUpCost;
    private NutrientTracker nutrientTracker;
    private DesignTracker designTracker;
    private SporeAttributeRanges sporeAttributeRanges;
    private PlayerController playerController;

    private HUDHealth hudHealth;

    void Start()
    {
        primalLevel = 1;
        speedLevel = 1;
        sentienceLevel = 1;
        vitalityLevel = 1;
        totalLevel = primalLevel + speedLevel + sentienceLevel + vitalityLevel;
        levelUpCost = Mathf.RoundToInt((.15f * Mathf.Pow(totalLevel, 3f)) + (3.26f * Mathf.Pow(totalLevel, 2f)) + (80.6f * totalLevel) + 101);
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        designTracker = gameObject.GetComponent<DesignTracker>();
        sporeAttributeRanges = GameObject.FindWithTag("PlayerParent").GetComponent<SporeAttributeRanges>();
        SetSporeName();
    }

    void Update()
    {
        //TEMPORARY KEYCODES FOR TESTING ~ WILL BE TURNED INTO UI BUTTONS IN THE FUTURE
        //Debug.Log("levelupcost: " + levelUpCost);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LevelPrimal();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LevelSpeed();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LevelSentience();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LevelVitality();
        }
    }
    public void LevelPrimal()
    {        
        
        if(nutrientTracker.currentNutrients >= levelUpCost)
        {
            nutrientTracker.SubtractNutrients(levelUpCost);
            primalLevel++;
            
            StartCalculateAttributes();
            UpdateLevel();
        }
        
    }
    public void DeLevelPrimal()
    {
            primalLevel--;
            
            StartCalculateAttributes();
            UpdateLevel();
            nutrientTracker.AddNutrients(levelUpCost);
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);  
    }
    public void LevelSpeed()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost)
        {
            nutrientTracker.SubtractNutrients(levelUpCost);
            speedLevel++;

            StartCalculateAttributes();
            UpdateLevel();
        }

    }
    public void DeLevelSpeed()
    {
            speedLevel--;

            StartCalculateAttributes();
            UpdateLevel();
            nutrientTracker.AddNutrients(levelUpCost);     
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);    
    }
    public void LevelSentience()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost)
        {
            nutrientTracker.SubtractNutrients(levelUpCost);
            sentienceLevel++;
            UpdateLevel();
        }
    }
     public void DeLevelSentience()
    {
            sentienceLevel--;
            UpdateLevel();
            nutrientTracker.AddNutrients(levelUpCost);  
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);       
    }
    public void LevelVitality()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost)
        {
            nutrientTracker.SubtractNutrients(levelUpCost);
            vitalityLevel++;

            StartCalculateAttributes();
            UpdateLevel();
        }
        
    }
    public void DeLevelVitality()
    {
            vitalityLevel--;

            StartCalculateAttributes();
            UpdateLevel();
            nutrientTracker.AddNutrients(levelUpCost);     
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);    
    }

    public void UpdateLevel()
    {
        totalLevel = primalLevel + speedLevel + sentienceLevel + vitalityLevel;
        levelUpCost = Mathf.RoundToInt((.15f * Mathf.Pow(totalLevel, 3f)) + (3.26f * Mathf.Pow(totalLevel, 2f)) + (80.6f * totalLevel) + 101);

        designTracker.UpdateBlendshape(sentienceLevel,primalLevel,vitalityLevel,speedLevel);
        UpdateSporeName();
    }

    public void StartCalculateAttributes()
    {
        StartCoroutine(CalculateAttributes());
    }

    IEnumerator CalculateAttributes()
    {
        yield return null;

        float minAttackDamage = sporeAttributeRanges.attackDamageAt1Primal;
        float maxAttackDamage = sporeAttributeRanges.attackDamageAt15Primal;
        primalDmg = Mathf.RoundToInt(LerpAttribute(minAttackDamage, maxAttackDamage, primalLevel));

        float minHealth = sporeAttributeRanges.healthAt1Vitality;
        float maxHealth = sporeAttributeRanges.healthAt15Vitality;
        baseHealth = Mathf.RoundToInt(LerpAttribute(minHealth, maxHealth, vitalityLevel));

        float minRegen = sporeAttributeRanges.regenAt1Vitality;
        float maxRegen = sporeAttributeRanges.regenAt15Vitality;
        baseRegen = LerpAttribute(minRegen, maxRegen, vitalityLevel);

        float minSpeed = sporeAttributeRanges.moveSpeedAt1Speed;
        float maxSpeed = sporeAttributeRanges.moveSpeedAt15Speed;
        moveSpeed = LerpAttribute(minSpeed, maxSpeed, speedLevel);
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerController.GetStats();

        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
        Animator currentAnimator = currentPlayer.GetComponent<Animator>();
        float minAttackSpeed = sporeAttributeRanges.attackSpeedAt1Speed;
        float maxAttackSpeed = sporeAttributeRanges.attackSpeedAt15Speed;
        float animatorSpeed = LerpAttribute(minAttackSpeed, maxAttackSpeed, speedLevel);
        currentAnimator.speed = animatorSpeed;
    }

    public float LerpAttribute(float minValue, float maxValue, int level)
    {
        float t = (float)level;
        float lerpValue = (-0.081365f) + (0.08f*t) + (0.0015f * Mathf.Pow(t, 2f)) - (0.000135f * Mathf.Pow(t, 3f));
        
        float attributeValue = Mathf.Lerp(minValue, maxValue, lerpValue);
        return attributeValue;
    }

    public void SetSporeName()
    {
        hudHealth = GameObject.Find("HUD").GetComponent<HUDHealth>();
        int enumLength = System.Enum.GetValues(typeof(Names)).Length;
        int randomIndex = Random.Range(0, enumLength);
        thisName = (Names)randomIndex;
    }

    public void UpdateSporeName()
    {
        string thisNameString = thisName.ToString();
        hudHealth.SetSporeName(thisNameString);
    }
}
