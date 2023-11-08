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
    public float baseAnimationSpeed = 1.5f;
    public float animationScalar = 1f;
    public float maxAnimationSpeed = 3f;
    
    [Header("Sentience Level")]
    public int sentienceLevel;
    //Sentience Base Stats
    public float skillDmg = 10f;   
    
    [Header("Vitality Level")]
    public int vitalityLevel;
    //Vitality Base Stats
    public float baseHealth = 100f;
    public float baseRegen = .2f;
    public int totalLevel;
    public int levelUpCost;
    private NutrientTracker nutrientTracker;
    private DesignTracker designTracker;

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
        SetSporeName();
    }

    void Update()
    {
        //TEMPORARY KEYCODES FOR TESTING ~ WILL BE TURNED INTO UI BUTTONS IN THE FUTURE
        Debug.Log("levelupcost: " + levelUpCost);
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
            primalDmg += 2f;
            UpdateLevel();
        }
        
    }
    public void DeLevelPrimal()
    {
            primalLevel--;
            primalDmg -= 2f;
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
            moveSpeed += .1f;
            atkCooldownBuff += .02f;
            UpdateLevel();
        }

    }
    public void DeLevelSpeed()
    {
            speedLevel--;
            moveSpeed -= .1f;
            atkCooldownBuff -= .02f;
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
            skillDmg += 2f;
            UpdateLevel();
        }
    }
     public void DeLevelSentience()
    {
            sentienceLevel--;
            skillDmg -= 2f;
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
            baseHealth += 5f;
            baseRegen += .05f;
            UpdateLevel();
        }
        
    }
    public void DeLevelVitality()
    {
            vitalityLevel--;
            baseHealth -= 5f;
            baseRegen -= .05f;
            UpdateLevel();
            nutrientTracker.AddNutrients(levelUpCost);     
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);    
    }
    public void UpdateLevel()
    {
        totalLevel = primalLevel + speedLevel + sentienceLevel + vitalityLevel;
        levelUpCost = Mathf.RoundToInt((.15f * Mathf.Pow(totalLevel, 3f)) + (3.26f * Mathf.Pow(totalLevel, 2f)) + (80.6f * totalLevel) + 101);

        designTracker.UpdateBlendshape(sentienceLevel,primalLevel,vitalityLevel,speedLevel);
        UpdateAnimatorSpeed();
        UpdateSporeName();
    }

    public void UpdateAnimatorSpeed()
    {
        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
        Animator currentAnimator = currentPlayer.GetComponent<Animator>();

        currentAnimator.speed = Mathf.Lerp(baseAnimationSpeed, maxAnimationSpeed, speedLevel / 15f);
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
