using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechingSpore : Skill
{
    //Skill specific fields
    [SerializeField] private float attachRange = 10f;
    [SerializeField] private float sporeDuration = 5f;
    [SerializeField] private GameObject sporeObj;
    [SerializeField] private GameObject healObj;
    private GameObject currentSpore;
    [SerializeField] private float percentOfDamageHealed = 0.5f;
    [SerializeField] private float reducedCooldownPercentL = .6f;


    public override void DoSkill()
    {
        //Skill specific stuff

        DoLeech();
        EndSkill();
    }

    public void DoLeech()
    {
        SoundEffectManager.Instance.PlaySound("Projectile", player.transform.position);

        //Get closest enemy within distance
        Transform closestEnemyObj = ClosestEnemy();

        if (closestEnemyObj != null)
        {
            GameObject enemyAttach = closestEnemyObj.Find("Attach")?.gameObject;
            if (enemyAttach != null)
            {
                currentSpore = Instantiate(sporeObj, enemyAttach.transform);
                StartCoroutine(DrainEnemy(closestEnemyObj.gameObject));
                StartCoroutine(HealingPlayer(closestEnemyObj.gameObject));
                StartCoroutine(DestroySpore(currentSpore));
            }
        }
       else
        {
            StartCooldown(GetFinalCooldown() * reducedCooldownPercentL);
        }
    }

    Transform ClosestEnemy()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, attachRange, enemyLayerMask);

        Debug.Log("Number of colliders detected: " + colliders.Length);

        Transform theTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider collider in colliders)
        {            Transform enemyTargets = collider.transform;

            Vector3 directionTarget = enemyTargets.position - currentPosition;
            float dSqrTarget = directionTarget.sqrMagnitude;

            if (dSqrTarget < closestDistance)
            {
                closestDistance = dSqrTarget;
                theTarget = enemyTargets;
            }
        }
        return theTarget;
    }

    IEnumerator DrainEnemy(GameObject enemy)
    {
        float timer = 0f;

        while (timer < sporeDuration)
        {  
            Debug.Log("Draining enemy");
            float damage = finalSkillValue;  
            if (enemy != null && enemy.GetComponent<EnemyHealth>().currentHealth > 0)
                enemy.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
            else if (enemy != null && enemy.GetComponent<EnemyHealth>().currentHealth <= 0)
                Destroy(currentSpore);
            yield return new WaitForSeconds(1f);;
            timer++;
        }
    }

    IEnumerator HealingPlayer(GameObject enemy)
    {
        float timer = 0f;

        while (timer < sporeDuration)
        {  
            float damage = finalSkillValue;
            if (enemy != null && enemy.GetComponent<EnemyHealth>().currentHealth > 0){
                GameObject healing = Instantiate(healObj, enemy.transform.position, Quaternion.identity);
                healing.GetComponent<HealerScript>().healAmount = (damage * percentOfDamageHealed);
                //GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>().PlayerHeal(damage * percentOfDamageHealed);
            }
            else if (enemy != null && enemy.GetComponent<EnemyHealth>().currentHealth <= 0)
                Destroy(currentSpore);
            yield return new WaitForSeconds(1f);
            timer++;
        }
    }

    IEnumerator DestroySpore(GameObject theSpore)
    {
        yield return new WaitForSeconds(sporeDuration);
        Destroy(theSpore);
    }
}
