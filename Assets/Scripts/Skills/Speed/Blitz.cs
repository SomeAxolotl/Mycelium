using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Blitz : Skill
{
    //Skill specific fields
    private float activeBlitz = .3f;
    private float raycastDistance = 10f;
    private int particleCount = 10;
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
        Rigidbody rb = player.GetComponent<Rigidbody>();
        playerController.activeDodge = true;
        playerController.isInvincible = true;
        playerController.looking = false;
        Vector3 blitzDirection = rb.transform.forward * 40f;
        float elapsedTime = 0f;
        float storedAnimSpeed = currentAnimator.speed;
        if(curWeapon != null)
        {
            currentAnimator.speed = 2f;
            currentAnimator.Play("Stab");
        }
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
            blitzDirection.y = rb.velocity.y;
            rb.velocity = blitzDirection;
            yield return null;
        }
        currentAnimator.speed = storedAnimSpeed;
        playerController.activeDodge = false;
        playerController.isInvincible = false;
        playerController.looking = true;
        enemiesHit.Clear();
    }
    IEnumerator BlitzParticles(Vector3 startPosition, Vector3 startingForwardVector)
    {
        for (int i = 0; i < particleCount; i++)
        {
            float t = (float)i / (float)(particleCount - 1);

            Vector3 spawnPosition = Vector3.Lerp(startPosition, startPosition + (startingForwardVector * raycastDistance), t);
            ParticleManager.Instance.SpawnParticles("BlitzParticles", player.transform.Find("CenterPoint").position, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenParticles);
        }
    }
}