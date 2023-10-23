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
    int levelUpCost;
    private NutrientTracker nutrientTracker;
    private DesignTracker designTracker;

    void Start()
    {
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
            nutrientTracker.SubtractNutrients(levelUpCost);        
        }
        else
        {
            Debug.Log("Not Enough Nutrients!");
        }
        UpdateLevel();
    }
    public void LevelSpeed()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost)
        {
            speedLevel++;
            moveSpeed += .1f;
            atkCooldownBuff += .02f;
            nutrientTracker.SubtractNutrients(levelUpCost);        
        }
        else
        {
            Debug.Log("Not Enough Nutrients!");
        }
        UpdateLevel();
    }
    public void LevelSentience()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost)
        { 
            sentienceLevel++;
            skillDmg += 2f;
            nutrientTracker.SubtractNutrients(levelUpCost);        
        }
        else
        {
            Debug.Log("Not Enough Nutrients!");
        }
        UpdateLevel();
    }
    public void LevelVitality()
    {
        if(nutrientTracker.currentNutrients >= levelUpCost)
        {
            vitalityLevel++;
            baseHealth += 5f;
            baseRegen += .05f;
            nutrientTracker.SubtractNutrients(levelUpCost);        
        }
        else
        {
            Debug.Log("Not Enough Nutrients!");
        }
        UpdateLevel();
    }

    private void UpdateLevel()
    {
        totalLevel = primalLevel + speedLevel + sentienceLevel + vitalityLevel;
        levelUpCost = Mathf.RoundToInt((.15f * Mathf.Pow(totalLevel, 3f)) + (3.26f * Mathf.Pow(totalLevel, 2f)) + (80.6f * totalLevel) + 101);

        designTracker.UpdateBlendshape(sentienceLevel,primalLevel,vitalityLevel,speedLevel);
    }
}
