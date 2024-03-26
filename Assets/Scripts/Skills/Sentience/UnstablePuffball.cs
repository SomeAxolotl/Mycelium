using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstablePuffball : Skill
{
    //Skill specific fields
    [SerializeField] private GameObject puffballPrefab;
    public override void DoSkill()
    {
        //Skill specific stuff
        DoPuffball();
        EndSkill();
    }

    public void DoPuffball()
    {
        SoundEffectManager.Instance.PlaySound("Projectile", transform.position);
        Instantiate(puffballPrefab, player.transform.Find("CenterPoint").position, transform.rotation);
    }
}
