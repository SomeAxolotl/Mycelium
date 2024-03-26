using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undergrowth : Skill
{
    //Skill specific fields
    [SerializeField] private GameObject undergrowthPrefab;
    public override void DoSkill()
    {
        //Skill specific stuff
        DoUndergrowth();
        InstantiateParticles();
        EndSkill();
    }
    void DoUndergrowth()
    {
        SoundEffectManager.Instance.PlaySound("Projectile", transform.position);

        Instantiate(undergrowthPrefab, player.transform.Find("CenterPoint").position, transform.rotation);
    }
    void InstantiateParticles()
    {
        ParticleManager.Instance.SpawnParticles("UndergrowthShot", player.transform.Find("CenterPoint").position, transform.rotation);
    }
}
