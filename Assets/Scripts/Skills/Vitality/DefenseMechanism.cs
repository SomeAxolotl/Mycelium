using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseMechanism : Skill
{
    //Skill specific fields
    [SerializeField] private float defenseDuration = 5f;
    public override void DoSkill()
    {
        //Skill specific stuff
        DefenseParticles();

        if (isPlayerCurrentPlayer())
        {
            DefenseChange defenseChangeEffect = playerHealth.gameObject.AddComponent<DefenseChange>();
            defenseChangeEffect.InitializeDefenseChange(10, 50);
        }

        EndSkill();
    }
    void DefenseParticles()
    {
        ParticleManager.Instance.SpawnParticles("DefenseParticles", new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(-90f, 0f, 0f), player);
    }
}
