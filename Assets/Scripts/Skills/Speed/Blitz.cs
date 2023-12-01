using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Blitz : Skill
{
    //Skill specific fields
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float dashSpeed;
    [SerializeField] private GameObject BlitzParticle; 

    public override void DoSkill()
    { 
        DoDash();
        DamageEnemies();
        playerController.isInvincible = false;
        player.GetComponent<Collider>().isTrigger = false;
        EndSkill();
    }

    void DoDash()
    {
        playerController.rb.AddForce(player.transform.forward * dashSpeed, ForceMode.Impulse);
        ParticleManager.Instance.SpawnParticles("Dust", GameObject.FindWithTag("currentPlayer").transform.position, Quaternion.identity);
        playerController.isInvincible = true;
        player.GetComponent<Collider>().isTrigger = true;
    }

    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, forward * maxDistance, Color.green);

        RaycastHit raycastHit;
        if(Physics.Raycast(transform.position, forward, out raycastHit, maxDistance))
		{
			if(raycastHit.collider.tag == "Enemy" || raycastHit.collider.tag == "Boss")
			{
                NewEnemyHealth enemyHealth = raycastHit.collider.GetComponent<NewEnemyHealth>();
                enemyHealth.EnemyTakeDamage(finalSkillValue);
			}
		}
    }
}