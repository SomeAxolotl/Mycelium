using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechingSpore : Skill
{
    //Skill specific fields
    /* damagee = 5
    Duration = 5 sec
    Range = 5
    nearest enemy im 5m range spore gets attached
    drains enemy for 5 seconds at a damage of 5 per second
    heals player for 5 seonds at 5 HP per second*/
    [SerializeField] private float smallRadius = 5f;

    public override void DoSkill()
    {
        //Skill specific stuff

        DoLeech();

        EndSkill();
    }

    public void DoLeech()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, smallRadius, enemyLayerMask);
        
        finalSkillValue = 5;
        foreach (Collider collider in colliders)
        {
            float distanceToCollider = Vector3.Distance(transform.position, collider.transform.position);

            if (distanceToCollider >= smallRadius)
            {
                
            }
        }
    }
}
