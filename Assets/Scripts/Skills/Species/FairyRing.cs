using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyRing : Skill
{
    [SerializeField] private GameObject fairyRingPrefab;
    [SerializeField] private GameObject fairyRingParticles;
    public override void DoSkill()
    {

        SpawnFairyRing();
        EndSkill();
    }
    void SpawnFairyRing()
    {
        GameObject fairyRingInstance = Instantiate(fairyRingPrefab, (player.transform.position + new Vector3(0f, 0.2f, 0f)), Quaternion.identity);
        GameObject ringParticles = Instantiate(fairyRingParticles, (player.transform.position + new Vector3(0f, 0.2f, 0f)), Quaternion.Euler(90f, 0f, 0f));
        Destroy(ringParticles, 7f);
        fairyRingInstance.GetComponent<FairyRingPlacement>().damage = finalSkillValue;

    }
}
