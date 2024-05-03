using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TrophicCascade : Skill
{
    [SerializeField] private float vanishDuration = 2f;
    [SerializeField] private float cascadeRadius = 3f;
    [SerializeField] private float cameraBlendTimeScalar = 0.75f;

    public override void DoSkill()
    {
        //Skill specific stuff
        
        StartCoroutine(Vanish());
    }

    IEnumerator Vanish()
    {
        Renderer[] childRenderers = player.GetComponentsInChildren<Renderer>();
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", player.transform.position, Quaternion.Euler(-90,0,0));

        if (isPlayerCurrentPlayer())
        {
            playerController.isInvincible = true;
        }
        
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = false;
        }

        yield return StartCoroutine(Cascade());

        if (isPlayerCurrentPlayer())
        {
            playerController.isInvincible = false;
        }
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
            if (!enemies.Contains(collider.gameObject) && collider.gameObject.GetComponent<EnemyHealth>() != null)
            {
                enemies.Add(collider.gameObject);
            }
        }

        float cascadeDuration = vanishDuration / 2f;


        VCamRotator vcamRotator = GameObject.Find("VCamHolder").GetComponent<VCamRotator>();

        for (int i = 0; i < enemies.Count; i++)
        {
            Mark(enemies[i]);
            float blendTime = Mathf.Clamp(cascadeDuration / enemies.Count * cameraBlendTimeScalar, 0, cascadeDuration * cameraBlendTimeScalar * 0.5f);
            vcamRotator.DramaticCamera(enemies[i].transform, blendTime);
            yield return new WaitForSeconds(cascadeDuration / enemies.Count);
        }

        yield return new WaitForSeconds(vanishDuration / 4f);

        //omae wa mou shindeiru
        Extinguish(enemies);
    }

    void Mark(GameObject enemy)
    {
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", enemy.transform.position, Quaternion.identity);
        SoundEffectManager.Instance.PlaySound("Projectile", enemy.transform.position);
    }

    void Extinguish(List<GameObject> enemies)
    {
        float damagePerEnemy = finalSkillValue;// / enemies.Count;
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
