using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstablePuffball : Skill
{
    //Skill specific fields
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileLaunch;
    [SerializeField] private float launchVel = 6.5f;
    UPProjectile uPProjectile;

    public override void DoSkill()
    {
        //Skill specific stuff
        DoProjectile();

        EndSkill();
    }

    public void DoProjectile()
    {
        projectileLaunch = GameObject.FindWithTag("spineshotLaunch").transform;
        uPProjectile = projectilePrefab.GetComponent<UPProjectile>();
        var shoot = Instantiate(projectilePrefab, projectileLaunch.position, transform.rotation);
        shoot.GetComponent<Rigidbody>().velocity = projectileLaunch.up * launchVel;
        uPProjectile.damage = finalSkillValue;
    }
}
