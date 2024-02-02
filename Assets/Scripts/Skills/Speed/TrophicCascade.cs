using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophicCascade : Skill
{
    [SerializeField] private float vanishDuration = 2f;
    [SerializeField] private float cascadeRadius = 3f;

    public override void DoSkill()
    {
        //Skill specific stuff

        StartCoroutine(Vanish());
    }

    IEnumerator Vanish()
    {
        Renderer[] childRenderers = player.GetComponentsInChildren<Renderer>();
        PlayerHealth playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>();

        playerController.isInvincible = true;
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = false;
        }

        yield return StartCoroutine(Cascade());

        playerController.isInvincible = false;
        foreach (Renderer renderer in childRenderers)
        {
            if (!(renderer is ParticleSystemRenderer))
            {
                renderer.enabled = true;
            }
        }
    }

    IEnumerator Cascade()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, cascadeRadius, enemyLayerMask);

        yield return new WaitForSeconds(vanishDuration / 4f);

        List<GameObject> enemies = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            enemies.Add(collider.gameObject);
        }

        float cascadeDuration = vanishDuration / 2f;

        foreach (GameObject enemy in enemies)
        {
            Mark(enemy);
            yield return new WaitForSeconds(cascadeDuration / enemies.Count);
        }

        yield return new WaitForSeconds(vanishDuration / 4f);

        Extinguish(enemies);
    }

    void Mark(GameObject enemy)
    {
        ParticleManager.Instance.SpawnParticles("Dust", enemy.transform.position, Quaternion.identity);
        SoundEffectManager.Instance.PlaySound("Stab", enemy.transform.position);
    }

    void Extinguish(List<GameObject> enemies)
    {
        float damagePerEnemy = finalSkillValue / enemies.Count;
        foreach (GameObject enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.EnemyTakeDamage(damagePerEnemy);
        }

        if (enemies.Count > 0)
        {
            SoundEffectManager.Instance.PlaySound("Impact", player.transform.position);
        }

        EndSkill();
    }
}
