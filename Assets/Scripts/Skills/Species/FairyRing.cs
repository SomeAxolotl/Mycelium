using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyRing : Skill
{
    [SerializeField] private GameObject fairyRingPrefab;
    public override void DoSkill()
    {

        SpawnFairyRing();
        EndSkill();
    }
    void SpawnFairyRing()
    {
        Vector3 playerPosition = transform.position;
        Vector3 spawnPosition = playerPosition;
        GameObject fairyRingInstance = Instantiate(fairyRingPrefab, spawnPosition, Quaternion.identity);

    }
}
