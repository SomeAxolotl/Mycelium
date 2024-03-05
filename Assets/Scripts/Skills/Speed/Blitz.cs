using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Blitz : Skill
{
    //Skill specific fields
    private float activeBlitz = .4f;
    private float raycastDistance = 10f;
    private int particleCount = 12;
    private float timeBetweenParticles = 0.03f;
    [SerializeField] private LayerMask enemyLayer;
    private Collider[] enemyColliders;
    List<GameObject> enemiesHit = new List<GameObject>();
    public override void DoSkill()
    { 
        StartCoroutine(BlitzParticles(player.transform.Find("CenterPoint").position, player.transform.forward));
        StartCoroutine(Blitzing());
        EndSkill();
    }
    IEnumerator Blitzing()
    {
        playerController.activeDodge = true;
        playerController.isInvincible = true;
        Vector3 blitzForce = playerController.inputDirection * 15f;
        blitzForce += Vector3.up * 3f;
        playerController.rb.AddForce(blitzForce, ForceMode.Impulse);
        float elapsedTime = 0f;
        while (elapsedTime < activeBlitz)
        {
            enemyColliders = Physics.OverlapSphere(player.transform.Find("CenterPoint").position, 3f, enemyLayer);
            foreach (var enemyCollider in enemyColliders)
            {
                if (enemyCollider.GetComponent<EnemyHealth>() != null && !enemiesHit.Contains(enemyCollider.gameObject))
                {
                    enemiesHit.Add(enemyCollider.gameObject);
                    enemyCollider.GetComponent<EnemyHealth>().EnemyTakeDamage(finalSkillValue);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerController.activeDodge = false;
        playerController.isInvincible = false;
        enemiesHit.Clear();
    }
    IEnumerator BlitzParticles(Vector3 startPosition, Vector3 startingForwardVector)
    {
        for (int i = 0; i < particleCount; i++)
        {
            float t = (float)i / (float)(particleCount - 1);

            Vector3 spawnPosition = Vector3.Lerp(startPosition, startPosition + (startingForwardVector * raycastDistance), t);
            ParticleManager.Instance.SpawnParticles("BlitzParticles", spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenParticles);
        }
    }
}