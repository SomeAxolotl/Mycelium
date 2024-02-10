using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechingSpore : Skill
{
    //Skill specific fields
    [SerializeField] private float attachRange = 5f;
    [SerializeField] private float sporeDuration = 5f;
    [SerializeField] private GameObject sporeObj;
    private GameObject currentSpore;
    [SerializeField] private float percentOfDamageHealed = 0.5f;

    public override void DoSkill()
    {
        //Skill specific stuff

        DoLeech();
        EndSkill();
    }

    public void DoLeech()
    {
        // get closest enemy with 5m
        Transform closestEnemyObj = ClosestEnemy();

        if (closestEnemyObj != null)
        {
            GameObject enemyAttach = GameObject.FindWithTag("EnemySporeAttach");
            currentSpore = Instantiate(sporeObj, enemyAttach.transform);
            StartCoroutine(DrainEnemy(closestEnemyObj.gameObject));
            StartCoroutine(HealingPlayer(closestEnemyObj.gameObject));
            StartCoroutine(DestroySpore(currentSpore));
        }
    }

    Transform ClosestEnemy()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, attachRange, enemyLayerMask);

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
            if (enemy != null && enemy.GetComponent<EnemyHealth>().currentHealth > 0) 
                GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>().PlayerHeal(damage * percentOfDamageHealed);
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
