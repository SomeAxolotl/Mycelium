using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string sporeName = "Gob";

    public List<string> equippedSkills = new List<string>()
    {
        {"NoSkill"},
        {"NoSkill"},
        {"NoSkill"}
    };

    //Able to be purchased
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
    //Able to be equipped
    public Dictionary<string, bool> skillEquippables = new Dictionary<string, bool>()
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
    public int primalLevel = 1;
    //Primal Base Stats
    public float primalDmg = 10f;
    
    [Header("Speed Level")]
    public int speedLevel = 1;
    //Speed Base Stats
    public float moveSpeed = 4f;
    public float atkCooldownBuff = 0f;
    public float minAttackSpeed;
    public float maxAttackSpeed;
    
    [Header("Sentience Level")]
    public int sentienceLevel = 1;
    
    [Header("Vitality Level")]
    public int vitalityLevel = 1;
    //Vitality Base Stats
    public float baseHealth = 100f;
    public float baseRegen = .2f;
    
    public int totalLevel;
    public int levelUpCost;
    private NutrientTracker nutrientTracker;
    private DesignTracker designTracker;
    private SporeAttributeIncrements sporeAttributeIncrements;
    private PlayerController playerController;
    private SkillManager skillManager;
    [SerializeField] private Nametag nametag;

    private HUDHealth hudHealth;

    public float animatorSpeed;

    void Start()
    {
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        designTracker = GetComponent<DesignTracker>();
        GameObject playerParent = GameObject.FindWithTag("PlayerParent");
        skillManager = playerParent.GetComponent<SkillManager>();
        sporeAttributeIncrements = playerParent.GetComponent<SporeAttributeIncrements>();
    }

    void Update()
    {
        totalLevel = primalLevel + speedLevel + sentienceLevel + vitalityLevel;
        levelUpCost = Mathf.RoundToInt((.15f * Mathf.Pow(totalLevel, 3f)) + (3.26f * Mathf.Pow(totalLevel, 2f)) + (80.6f * totalLevel) + 101);
    }
    public void LevelPrimal()
    {        
        
        if(nutrientTracker.currentNutrients >= levelUpCost && primalLevel < 15)
        {
            nutrientTracker.SubtractNutrients(levelUpCost);
            primalLevel++;
            Debug.Log("Leveled Primal");
            StartCalculateAttributes();
            UpdateLevel();
        }
        
    }
    public void DeLevelPrimal()
    {
            if(primalLevel == 1)
            {
                return;
            }
            else
            {
            primalLevel--;
            
            StartCalculateAttributes();
            UpdateLevel();
            nutrientTracker.AddNutrients(levelUpCost);
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel); 
            } 
    }
    public void LevelSpeed()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost && speedLevel < 15)
        {
            nutrientTracker.SubtractNutrients(levelUpCost);
            speedLevel++;

            StartCalculateAttributes();
            UpdateLevel();
        }

    }
    public void DeLevelSpeed()
    {
            if(speedLevel == 1)
            {
                return;
            }
            else
            {
            speedLevel--;

            StartCalculateAttributes();
            UpdateLevel();
            nutrientTracker.AddNutrients(levelUpCost);     
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);
            }    
    }
    public void LevelSentience()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost && sentienceLevel < 15)
        {
            nutrientTracker.SubtractNutrients(levelUpCost);
            sentienceLevel++;
            UpdateLevel();
        }
    }
     public void DeLevelSentience()
    {
            if(sentienceLevel == 1)
            {
                return;
            }
            else
            {
            sentienceLevel--;
            UpdateLevel();
            nutrientTracker.AddNutrients(levelUpCost);  
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);       
            }    
    }
    public void LevelVitality()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost && vitalityLevel < 15)
        {
            nutrientTracker.SubtractNutrients(levelUpCost);
            vitalityLevel++;

            StartCalculateAttributes();
            UpdateLevel();
        }
        
    }
    public void DeLevelVitality()
    {
            if(vitalityLevel == 1)
            {
                return;
            }
            else
            {
            vitalityLevel--;
            StartCalculateAttributes();
            UpdateLevel();
            nutrientTracker.AddNutrients(levelUpCost);     
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel); 
            }   
    }

    public void SetEquippedSkill(string skillName, int slot)
    {
        equippedSkills[slot] = skillName;
        Debug.Log("CharacterStats equippedSkill ["+slot+"] - " + equippedSkills[slot]);
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

        primalDmg = Mathf.RoundToInt(sporeAttributeIncrements.attackDamageBase + ((primalLevel - 1) * sporeAttributeIncrements.attackDamageIncrement));
        baseHealth = Mathf.RoundToInt(sporeAttributeIncrements.healthBase + ((vitalityLevel - 1) * sporeAttributeIncrements.healthIncrement));
        baseRegen = sporeAttributeIncrements.regenBase + ((vitalityLevel - 1) * sporeAttributeIncrements.regenIncrement);
        moveSpeed = sporeAttributeIncrements.moveSpeedBase + ((speedLevel - 1) * sporeAttributeIncrements.moveSpeedIncrement);
        animatorSpeed = sporeAttributeIncrements.attackSpeedBase + ((speedLevel - 1) * sporeAttributeIncrements.attackSpeedIncrement);

        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerController.GetStats();

        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
        Animator currentAnimator = currentPlayer.GetComponent<Animator>();
        currentAnimator.speed = animatorSpeed;

        /*float minAttackDamage = sporeAttributeRanges.attackDamageAt1Primal;
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
        animatorSpeed = LerpAttribute(minAttackSpeed, maxAttackSpeed, speedLevel);
        currentAnimator.speed = animatorSpeed;*/
    }

    /*public float LerpAttribute(float minValue, float maxValue, int level)
    {
        float t = (float)level;
        float lerpValue = (-0.081365f) + (0.08f*t) + (0.0015f * Mathf.Pow(t, 2f)) - (0.000135f * Mathf.Pow(t, 3f));
        
        float attributeValue = Mathf.Lerp(minValue, maxValue, lerpValue);
        return attributeValue;
    }*/

    public void UpdateSporeName()
    {
        hudHealth = GameObject.Find("HUD").GetComponent<HUDHealth>();
        hudHealth.SetSporeName(sporeName);
        UpdateNametagText();
    }
    public void UpdateNametagText()
    {
        nametag.SetSporeNametagText(sporeName);
    }
    public void ShowNametag()
    {
        Debug.Log(sporeName + " showing nametag");
        nametag.ShowNametag();
    }
    public void HideNametag()
    {
        nametag.HideNametag();
        Debug.Log(sporeName + " hiding nametag");
    }

    public void UnlockSkill(string skillName)
    {
        skillUnlocks[skillName] = true;
    }
    public void RelockSkill(string skillName)
    {
        skillUnlocks[skillName] = false;
    }
    public void PurchaseSkill(string skillName)
    {
        skillEquippables[skillName] = true;
    }
}
