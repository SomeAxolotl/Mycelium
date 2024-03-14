using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyRing : Skill
{
    [SerializeField] private GameObject fairyRingPrefab;
    [SerializeField] private float distanceInFront = 2.0f;
    public override void DoSkill()
    {

        SpawnFairyRing();
        EndSkill();
    }
    void SpawnFairyRing()
    {
        Vector3 playerPosition = transform.position;
        Vector3 spawnPosition = playerPosition + transform.forward * distanceInFront;
        float yOffSet = 1.2f;
        spawnPosition.y += yOffSet;
        GameObject fairyRingInstance = Instantiate(fairyRingPrefab, spawnPosition, Quaternion.identity);

        fairyRingInstance.GetComponent<FairyRingPlacement>().damage = finalSkillValue;

    }
}
