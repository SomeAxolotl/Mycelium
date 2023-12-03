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
    [SerializeField] private float timeBeforeFall = 0.25f;
    [SerializeField] private float particleHeight = 0f;

    public override void DoSkill()
    { 
        StartCoroutine(BlitzParticles(player.transform.position, player.transform.forward));
        StartCoroutine(DoDash());
        DamageEnemies();
        playerController.isInvincible = false;
        player.GetComponent<Collider>().isTrigger = false;
        EndSkill();
    }

    IEnumerator DoDash()
    {
        playerController.rb.AddForce(player.transform.forward * dashSpeed, ForceMode.Impulse);
        playerController.rb.constraints |= RigidbodyConstraints.FreezePositionY;
        playerController.isInvincible = true;
        player.GetComponent<Collider>().isTrigger = true;

        yield return new WaitForSeconds(timeBeforeFall);
        playerController.rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
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

    IEnumerator BlitzParticles(Vector3 startPosition, Vector3 startingForwardVector)
    {
        for (int i = 0; i < particleCount; i++)
        {
            float t = (float)i / (float)(particleCount - 1);

            Vector3 spawnPosition = Vector3.Lerp(startPosition, startPosition + (startingForwardVector * maxDistance), t);
            Vector3 heightAdder = new Vector3(0f, particleHeight, 0f);
            Vector3 spawnPositionWithHeight = spawnPosition + heightAdder;
            ParticleManager.Instance.SpawnParticles("BlitzParticles", spawnPositionWithHeight, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenParticles);
        }
    }
}