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
        GameObject player = GameObject.FindWithTag("currentPlayer");
        Instantiate(undergrowthPrefab, player.transform.Find("CenterPoint").position, transform.rotation);
    }
    void InstantiateParticles()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");
        ParticleManager.Instance.SpawnParticles("UndergrowthShot", player.transform.Find("CenterPoint").position, transform.rotation);
    }
}
