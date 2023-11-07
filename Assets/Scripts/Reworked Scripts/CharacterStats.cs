using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
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

    void Start()
    {
        levelUpCost = Mathf.RoundToInt((.15f * Mathf.Pow(totalLevel, 3f)) + (3.26f * Mathf.Pow(totalLevel, 2f)) + (80.6f * totalLevel) + 101);
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        designTracker = gameObject.GetComponent<DesignTracker>();
    }

    void Update()
    {        
        //TEMPORARY KEYCODES FOR TESTING ~ WILL BE TURNED INTO UI BUTTONS IN THE FUTURE

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
            primalLevel++;
            primalDmg += 2f;
            UpdateLevel();
            nutrientTracker.SubtractNutrients(levelUpCost);        
        }
        
    }
    public void DeLevelPrimal()
    {        
            
            UpdateLevel();
            primalLevel--;
            primalDmg -= 2f;
            nutrientTracker.AddNutrients(levelUpCost);
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);  
       
    }
    public void LevelSpeed()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost)
        {
            speedLevel++;
            moveSpeed += .1f;
            atkCooldownBuff += .02f;
            UpdateLevel();
            nutrientTracker.SubtractNutrients(levelUpCost);        
        }

    }
    public void DeLevelSpeed()
    {
            UpdateLevel();
            speedLevel--;
            moveSpeed -= .1f;
            atkCooldownBuff -= .02f;
            nutrientTracker.AddNutrients(levelUpCost);     
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);    
    }
    public void LevelSentience()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost)
        { 
            sentienceLevel++;
            skillDmg += 2f;
            UpdateLevel();
            nutrientTracker.SubtractNutrients(levelUpCost);        
        }
    }
     public void DeLevelSentience()
    {
            UpdateLevel();
            sentienceLevel--;
            skillDmg -= 2f;
            nutrientTracker.AddNutrients(levelUpCost);  
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);       
    }
    public void LevelVitality()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost)
        {
            vitalityLevel++;
            baseHealth += 5f;
            baseRegen += .05f;
            UpdateLevel();
            nutrientTracker.SubtractNutrients(levelUpCost);        
        }
        
    }
    public void DeLevelVitality()
    {
            UpdateLevel();
            vitalityLevel--;
            baseHealth -= 5f;
            baseRegen -= .05f;
            nutrientTracker.AddNutrients(levelUpCost);     
            designTracker.ForceUpdateBlendshaped(sentienceLevel,primalLevel,vitalityLevel,speedLevel);    
    }
    public void UpdateLevel()
    {
        totalLevel = primalLevel + speedLevel + sentienceLevel + vitalityLevel;
        levelUpCost = Mathf.RoundToInt((.15f * Mathf.Pow(totalLevel, 3f)) + (3.26f * Mathf.Pow(totalLevel, 2f)) + (80.6f * totalLevel) + 101);

        designTracker.UpdateBlendshape(sentienceLevel,primalLevel,vitalityLevel,speedLevel);
        UpdateAnimatorSpeed();
    }

    public void UpdateAnimatorSpeed()
    {
        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
        Animator currentAnimator = currentPlayer.GetComponent<Animator>();

        currentAnimator.speed = Mathf.Lerp(baseAnimationSpeed, maxAnimationSpeed, speedLevel / 15f);
    }
}
