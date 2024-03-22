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
        ParticleManager.Instance.SpawnParticles("FairyRingBackUp", player.transform.position, Quaternion.Euler(-90,0,0));
        fairyRingInstance.GetComponent<FairyRingPlacement>().damage = finalSkillValue;
    }
}
