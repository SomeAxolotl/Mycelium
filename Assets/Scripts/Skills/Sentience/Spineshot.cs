using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spineshot : Skill
{
    //Skill specific fields
    [SerializeField] private GameObject spineshotPrefab;
    [SerializeField] private Transform spineshotLaunch;
    [SerializeField] private TempSpineshot tempSpineshot;
    [SerializeField] private float spineshotDmg;

    public override void DoSkill()
    {
        //Skill specific stuff
        DoSpineshot();

        EndSkill();
    }

    public void DoSpineshot()
    {
        finalSkillCooldown = 3;
        spineshotLaunch = GameObject.FindWithTag("spineshotLaunch").transform;
        Instantiate(spineshotPrefab, spineshotLaunch.position, transform.rotation);
    }
}
