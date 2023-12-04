using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Blitz : Skill
{
    //Skill specific fields
    private float blitzForce = 8f;
    private float blitzTime = .3f;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private int particleCount = 10;
    [SerializeField] private float timeBetweenParticles = 0.05f;
    [SerializeField] private float particleHeight = 0f;
    [SerializeField] private LayerMask enemyLayer;
    private Collider[] enemyColliders;
    List<GameObject> enemiesHit = new List<GameObject>();
    public override void DoSkill()
    { 
        StartCoroutine(BlitzParticles(player.transform.position, player.transform.forward));
        StartCoroutine(Blitzing());
        EndSkill();
    }
    IEnumerator Blitzing()
    {
        playerController.activeDodge = true;
        playerController.rb.AddForce(playerController.forceDirection * blitzForce, ForceMode.Impulse);
        playerController.isInvincible = true;
        enemyColliders = Physics.OverlapSphere(transform.position, 5f, enemyLayer);
        foreach (var enemyCollider in enemyColliders)
        {
            Vector3 dirToEnemy = (enemyCollider.gameObject.transform.position - playerController.forceDirection).normalized;
            float angleToEnemy = Vector3.Angle(playerController.forceDirection, enemyCollider.gameObject.transform.position);
            float distanceToEnemy = Vector3.Distance(transform.position, enemyCollider.gameObject.transform.position);
            if (enemyCollider.GetComponent<NewEnemyHealth>() != null && !enemiesHit.Contains(enemyCollider.gameObject) && angleToEnemy <= 25f && distanceToEnemy <= 5f)
            {
                yield return new WaitForSeconds(blitzTime / 2f);
                enemiesHit.Add(enemyCollider.gameObject);
                enemyCollider.GetComponent<NewEnemyHealth>().EnemyTakeDamage(finalSkillValue);
            }
        }
        yield return new WaitForSeconds(blitzTime/2f);
        playerController.activeDodge = false;
        playerController.isInvincible = false;
        ClearEnemyList();
    }
    IEnumerator DamageEnemies()
    {
        enemyColliders = Physics.OverlapSphere(transform.position, 2f, enemyLayer);
        foreach (var enemyCollider in enemyColliders)
        {
            if (enemyCollider.GetComponent<NewEnemyHealth>() != null && !enemiesHit.Contains(enemyCollider.gameObject))
            {
                enemiesHit.Add(enemyCollider.gameObject);
                enemyCollider.GetComponent<NewEnemyHealth>().EnemyTakeDamage(finalSkillValue);
            }
        }
        yield return new WaitForSeconds(blitzTime + .05f);
    }
    private void ClearEnemyList()
    {
        enemiesHit.Clear();
    }

    IEnumerator BlitzParticles(Vector3 startPosition, Vector3 startingForwardVector)
    {
        for (int i = 0; i < particleCount; i++)
        {
            float t = (float)i / (float)(particleCount - 1);

            Vector3 spawnPosition = Vector3.Lerp(startPosition, startPosition + (startingForwardVector * raycastDistance), t);
            Vector3 heightAdder = new Vector3(0f, particleHeight, 0f);
            Vector3 spawnPositionWithHeight = spawnPosition + heightAdder;
            ParticleManager.Instance.SpawnParticles("BlitzParticles", spawnPositionWithHeight, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenParticles);
        }
    }
}