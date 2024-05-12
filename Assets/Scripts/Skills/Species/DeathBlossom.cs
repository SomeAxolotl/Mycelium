using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlossom : Skill
{
    [SerializeField] private GameObject deathBlossomPrefab;
    [SerializeField] private float distanceInFront = 2.0f;
    public override void DoSkill()
    {
        SpawnDeathBlossom();
        EndSkill();
    }
    void SpawnDeathBlossom()
    {
        Vector3 playerPosition = transform.position;
        Vector3 spawnPosition = playerPosition + transform.forward * distanceInFront;
        GameObject deathBlossomInstance = Instantiate(deathBlossomPrefab, spawnPosition, Quaternion.identity);
        deathBlossomInstance.GetComponent<DeathBlossomPlant>().finalDamageValue = finalSkillValue;
    }
}

