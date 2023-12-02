using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Blitz : Skill
{
    //Skill specific fields
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float dashSpeed;
    [SerializeField] private int particleCount = 10;
    [SerializeField] private float timeBetweenParticles = 0.05f;

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
        playerController.isInvincible = true;
        player.GetComponent<Collider>().isTrigger = true;
    }

    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Vector3 forward = player.transform.TransformDirection(Vector3.forward);

        RaycastHit raycastHit;
        if(Physics.Raycast(player.transform.position, forward, out raycastHit, maxDistance))
		{
			if(raycastHit.collider.tag == "Enemy" || raycastHit.collider.tag == "Boss")
			{
                NewEnemyHealth enemyHealth = raycastHit.collider.GetComponent<NewEnemyHealth>();
                enemyHealth.EnemyTakeDamage(finalSkillValue);
			}
		}
    }

    IEnumerator BlitzParticles()
    {
        for (int i = 0; i < particleCount; i++)
        {
            float t = i / (particleCount - 1);

            Vector3 spawnPosition = Vector3.Lerp(player.transform.position, (player.transform.position + player.transform.forward) * maxDistance, t);
            ParticleManager.Instance.SpawnParticles("BlitzParticles", spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenParticles);
        }
    }
}