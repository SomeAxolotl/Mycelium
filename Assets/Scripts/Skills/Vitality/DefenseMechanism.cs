using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseMechanism : Skill
{
    //Skill specific fields
    [SerializeField] private float skillDuration = 1f;
    [SerializeField] private float knockbackRadius = 3f;
    public override void DoSkill()
    {
        if (isPlayerCurrentPlayer())
        {
            DefenseChange defenseChangeEffect = playerHealth.gameObject.AddComponent<DefenseChange>();
            defenseChangeEffect.InitializeDefenseChange(skillDuration, 150);

            Sturdy sturdyEffect = playerHealth.gameObject.AddComponent<Sturdy>();
            sturdyEffect.InitializeSturdy(skillDuration);

            RefreshTimer();
            StartCoroutine(Knockback());
        }

        EndSkill();
    }

    float savedCooldown;
    public override void StartCooldown(float skillCooldown){
        savedCooldown = skillCooldown;
        //Does not do cooldown normally
    }

    private IEnumerator Knockback(){
        showRadius = true;
        yield return new WaitForSeconds(skillDuration);
        //ParticleManager.Instance.SpawnParticles("SporeburstParticles", transform.position, Quaternion.identity);
        //Knocks back enemies
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, knockbackRadius, enemyLayerMask);
        List<GameObject> enemies = new List<GameObject>();
        foreach(Collider collider in colliders){
            if(collider.gameObject.GetComponent<EnemyKnockback>() != null && !enemies.Contains(collider.gameObject)){
                float distanceToCollider = Vector3.Distance(transform.position, collider.transform.position);
                enemies.Add(collider.gameObject);
                EnemyKnockback enemyKnockback = collider.gameObject.GetComponent<EnemyKnockback>();
                enemyKnockback.Knockback(12 * (distanceToCollider / knockbackRadius), transform, collider.transform, false);
            }
        }
        //Reflects any projectiles in a slightly larger area
        enemyLayerMask = 1 << LayerMask.NameToLayer("EnemyProjectile");
        colliders = Physics.OverlapSphere(transform.position, knockbackRadius * 1.2f, enemyLayerMask);
        foreach(Collider collider in colliders){
            Debug.Log("Projectile hit!");
            RangedEnemyProjectile proj = collider.GetComponent<RangedEnemyProjectile>();
            if(proj != null){
                proj.ReverseProjectile();
            }
        }
        showRadius = false;
        ActualCooldownStart();
    }

    private void ActualCooldownStart(){
        hudSkills.ToggleActiveBorder(skillSlot, false);
        
        if(cooldownCoroutine != null){
            StopCoroutine(cooldownCoroutine);
        }
        if(hudCooldownCoroutine != null){
            hudSkills.StopHUDCoroutine(hudCooldownCoroutine);
        }

        cooldownCoroutine = StartCoroutine(Cooldown(savedCooldown));
    }

    private void RefreshTimer(){
        hudSkills.ToggleActiveBorder(skillSlot, true);
        if(hudCooldownCoroutine != null){
            hudSkills.StopHUDEffectCoroutine(hudCooldownCoroutine);
        }
        hudCooldownCoroutine = hudSkills.StartEffectUI(skillSlot, skillDuration);
    }

    private bool showRadius = false;
    void OnDrawGizmos(){
        if(showRadius){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, knockbackRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, knockbackRadius * 1.2f);
        }
    }
}
