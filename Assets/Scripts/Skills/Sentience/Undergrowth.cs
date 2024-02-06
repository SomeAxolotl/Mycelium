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
        InstantiateParticles();
        DoUndergrowth();
        EndSkill();
    }

    void InstantiateParticles()
    {
        Vector3 spawnPosition = transform.position;
        ParticleManager.Instance.SpawnParticles("UndergrowthShot", spawnPosition, transform.rotation);
    }

    public void DoUndergrowth()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");
        Instantiate(undergrowthPrefab, new Vector3(player.transform.position.x, player.transform.position.y + 0.7f, player.transform.position.z), transform.rotation);
    }
}
