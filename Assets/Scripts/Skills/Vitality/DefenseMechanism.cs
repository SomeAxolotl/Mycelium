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
        canSkill = false;
    }

    private IEnumerator Knockback(){
        showRadius = true;
        ParticleManager.Instance.SpawnParticles("DefenseBuildup", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity, gameObject);
        yield return new WaitForSeconds(skillDuration);
        ParticleManager.Instance.SpawnParticles("DefenseBurst", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity, gameObject);
        //Knocks back enemies
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, knockbackRadius, enemyLayerMask);
        List<GameObject> enemies = new List<GameObject>();
        foreach(Collider collider in colliders){
            if(collider.gameObject.GetComponent<EnemyKnockback>() != null && !enemies.Contains(collider.gameObject)){
                //Knock enemies away, further if they are closer
                float distanceToCollider = Vector3.Distance(transform.position, collider.transform.position);
                enemies.Add(collider.gameObject);
                EnemyKnockback enemyKnockback = collider.gameObject.GetComponent<EnemyKnockback>();
                enemyKnockback.Knockback(12 * (distanceToCollider / knockbackRadius), transform, collider.transform, false);
                if(collider.gameObject.GetComponent<EnemyHealth>() != null){
                    collider.gameObject.GetComponent<EnemyHealth>().EnemyTakeDamage(finalSkillValue);
                }
            }
        }
        //Reflects any projectiles in a slightly larger area
        enemyLayerMask = 1 << LayerMask.NameToLayer("EnemyProjectile");
        colliders = Physics.OverlapSphere(transform.position, knockbackRadius * 1.2f, enemyLayerMask);
        foreach(Collider collider in colliders){
            RangedEnemyProjectile proj = collider.GetComponent<RangedEnemyProjectile>();
            if(proj != null){
                proj.ReverseProjectile();
            }
        }
        showRadius = false;
        ActualCooldownStart();
    }

    protected override void ActualCooldownStart(){
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
