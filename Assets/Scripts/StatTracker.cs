using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    //Displaying current level
    [Header("Primal Level")]
    public int primalLevel;
    //Primal Base Stats
    float meleeDmg = 10f;
    public float primalDmg;
    
    [Header("Speed Level")]
    public int speedLevel;
    //Speed Base Stats
    float moveSpeed = 5f;
    public float finalMoveSpeed;
    float atkCooldown = 1f;
    public float speedAtkCooldown;  
    
    [Header("Sentience Level")]
    public int sentienceLevel;
    //Sentience Base Stats
    float skillDmg = 20f;
    public float sentienceSkillDmg;
    public float skillCooldownBuff;    
    
    [Header("Vitality Level")]
    public int vitalityLevel;
    //Vitality Base Stats
    float baseHealth = 100f;
    public float maxHealth;
    float baseRegen = .05f;
    public float finalRegen;

    // Start is called before the first frame update
    void Start()
    {
        primalLevel = 0;
        speedLevel = 0;
        sentienceLevel = 0;
        vitalityLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Primal Stat Calc
        primalDmg = meleeDmg + (2f * primalLevel); // primalMeleeDmg will be added to the weapon damage that the player is holding to calc total damage output
        //Speed Stat Calc
        finalMoveSpeed = moveSpeed + (.2f * speedLevel);
        speedAtkCooldown = atkCooldown - (.05f * speedLevel); //Weapon cooldowns are calculated seperately in weapon scripts
        //Sentience Stat Calc
        sentienceSkillDmg = skillDmg + (2f * sentienceLevel);
        skillCooldownBuff = (.1f * sentienceLevel); //Skills have different base cooldowns and those are calculated in specific skill scripts
        //Vitality Stat Calc
        maxHealth = baseHealth + (5f * vitalityLevel);
        finalRegen = baseRegen + (.05f * vitalityLevel);
    }
}
