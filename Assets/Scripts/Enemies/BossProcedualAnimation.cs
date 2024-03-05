using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProcedualAnimation : MonoBehaviour
{
    MonsterBossMovement monsterBossMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        monsterBossMovement = GetComponent<MonsterBossMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (monsterBossMovement.turningRight == true)
        {
            // Move headfirst as indicator
            // Move right arm
            // Move left arm
            // Move body
        }

        if (monsterBossMovement.turningLeft == true)
        {
            // Move headfirst as indicator
            // Move left arm
            // Move right arm
            // Move body
        }
        
    }
}
