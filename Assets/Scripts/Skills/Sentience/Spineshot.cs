using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spineshot : Skill
{
    //Skill specific fields
    [SerializeField] private GameObject spineshotPrefab;
    public override void DoSkill()
    {
        //Skill specific stuff
        DoSpineshot();
        EndSkill();
    }

    public void DoSpineshot()
    {
        SoundEffectManager.Instance.PlaySound("Projectile", transform.position);
        
        Instantiate(spineshotPrefab, player.transform.Find("CenterPoint").position, transform.rotation);
    }
}
