using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseMechanism : Skill
{
    //Skill specific fields
    [SerializeField] private float defenseDuration = 5f;
    public override void DoSkill()
    {
        if (isPlayerCurrentPlayer())
        {
            DefenseChange defenseChangeEffect = playerHealth.gameObject.AddComponent<DefenseChange>();
            defenseChangeEffect.InitializeDefenseChange(1, 150);
        }

        EndSkill();
    }
}
