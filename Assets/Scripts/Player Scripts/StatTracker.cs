using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    //Blendshape Management
    private int blendShapeCount = 4;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private List<float> statLevelWeights;

    // Start is called before the first frame update
    void Start()
    {
        statLevelWeights = new List<float>();

        primalLevel = 0;
        speedLevel = 0;
        sentienceLevel = 0;
        vitalityLevel = 0;
    }

    void Awake()
    {   
        foreach (Transform child in this.transform.Find("SporePlaceholder"))
        {
            if (child.tag == "playerCap")
            {
                skinnedMeshRenderer = child.gameObject.GetComponent<SkinnedMeshRenderer>();
                Debug.Log(skinnedMeshRenderer);
                skinnedMesh = child.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            }
        }
        
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

    public int IncreaseStat(string statName, int increaseLevel)
    {
        if(String.Compare(statName, "primal") == 0)
        {
            if(primalLevel + increaseLevel >=15)
            {
                Debug.Log("Max Primal Level Reached!");
                return 0;
            }
            primalLevel += increaseLevel;
        }
        else if(String.Compare(statName, "sentience") == 0)
        {
            if(sentienceLevel + increaseLevel >=15)
            {
                Debug.Log("Max Sentience Level Reached!");
                return 0;
            }
            sentienceLevel += increaseLevel;
        }
            
        else if(String.Compare(statName, "vitality") == 0)
        {
            if(vitalityLevel + increaseLevel >=15)
            {
                Debug.Log("Max Vitality Level Reached!");
                return 0;
            }
            vitalityLevel += increaseLevel;
        }
        else if(String.Compare(statName, "speed") == 0)
        {
            if(speedLevel + increaseLevel >=15)
            {
                Debug.Log("Max Speed Level Reached!");
                return 0;
            }
            speedLevel += increaseLevel;

        }
        else
        {
            Debug.Log("Stat Name does not exist in the current context. Stat options are \"primal\", \"sentience\", \"speed\", and \"vitality\"");
            return 0;
        }

        UpdateBlendshape();
        return 1;

    }

    void UpdateBlendshape()
    {
        float levelWeight = 0;
        int totalPointsSpent = sentienceLevel + primalLevel + vitalityLevel + speedLevel;
        //map the weight for Sentience
        levelWeight = sentienceLevel/15f * totalPointsSpent/15f * 100;
        statLevelWeights.Add(levelWeight);
        //map the weight for Primal
        levelWeight = primalLevel/15f * totalPointsSpent/15f * 100;
        statLevelWeights.Add(levelWeight);
        //map the weight for Vitality
        levelWeight = vitalityLevel/15f * totalPointsSpent/15f * 100;
        statLevelWeights.Add(levelWeight);
        //map the weight for Speed
        levelWeight = speedLevel/15f * totalPointsSpent/15f * 100;
        statLevelWeights.Add(levelWeight);

        for(int i=0;i<blendShapeCount;i++)
            {
            skinnedMeshRenderer.SetBlendShapeWeight(i,statLevelWeights[i]);
            }
    }
}
