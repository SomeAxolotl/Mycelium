using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sporeburst : Skill
{
    [SerializeField] private float burstRadius = 3f;
    [SerializeField] private float stunDuration = 2f;
    List<GameObject> enemiesHit = new List<GameObject>();
    public override void DoSkill()
    {
        DoSporeburst();
        EndSkill();
    }

    void DoSporeburst()
    {
        SoundEffectManager.Instance.PlaySound("Explosion", player.transform.position);
        ParticleManager.Instance.SpawnParticles("SporeBurstPart", transform.position, Quaternion.Euler(-90,0,0));

        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] enemies = Physics.OverlapSphere(transform.position, burstRadius, enemyLayerMask);
        float damage = finalSkillValue;
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.GetComponent<EnemyHealth>() != null && !enemiesHit.Contains(enemy.gameObject))
            {
                enemiesHit.Add(enemy.gameObject);
                enemy.gameObject.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
                HealPlayer(enemy.gameObject);
                Debug.Log("Sporeburst hit!");
            }

            if (enemy.gameObject.GetComponent<EnemyAttack>() != null)
            {
                enemy.gameObject.GetComponent<EnemyAttack>().CancelAttack();
                StartCoroutine(ReactivateAttack(enemy.gameObject.GetComponent<EnemyAttack>()));
                enemy.gameObject.GetComponent<EnemyAttack>().enabled = false;
            }

            if (enemy.gameObject.GetComponent<ReworkedEnemyNavigation>() != null)
            {
                StartCoroutine(ReactivateNavigation(enemy.gameObject.GetComponent<ReworkedEnemyNavigation>()));
                enemy.gameObject.GetComponent<ReworkedEnemyNavigation>().enabled = false;
                enemy.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
            }
        }
    }
    void HealPlayer(GameObject enemy)
    {
        float damage = finalSkillValue;
        float healingAmount = 0.5f * damage;
        if (enemy != null)
        {
            GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>().SpawnHealingOrb(enemy.transform.position, healingAmount);
        }
    }
    IEnumerator ReactivateAttack(EnemyAttack enemyAttack)
    {
        yield return new WaitForSeconds(stunDuration);
        enemyAttack.enabled = true;
        enemiesHit.Clear();
    }
    IEnumerator ReactivateNavigation(ReworkedEnemyNavigation reworkedEnemyNavigation)
    {
        yield return new WaitForSeconds(stunDuration);
        reworkedEnemyNavigation.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
        reworkedEnemyNavigation.enabled = true;
        enemiesHit.Clear();
    }
}

