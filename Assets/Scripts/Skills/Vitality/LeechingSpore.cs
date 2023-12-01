using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechingSpore : Skill
{
    //Skill specific fields
    /* damagee = 5
    Duration = 5 sec
    Range = 5
    nearest enemy im 5m range spore gets attached
    drains enemy for 5 seconds at a damage of 5 per second
    heals player for 5 seonds at 5 HP per second*/
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float durationTime = 5f;
    [SerializeField] private GameObject sporeObj;

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
            GameObject theSpore = Instantiate(sporeObj, enemyAttach.transform);

            StartCoroutine(DrainEnemy(closestEnemyObj.gameObject));
            StartCoroutine(HealingPlayer());
            StartCoroutine(DestroySpore(theSpore));
            
            Debug.Log("Enemy within range.");
        }
        else
        {
            // No enemies within range
            Debug.Log("No enemies within range.");
        }

        // destroy spore here
        // sporeObj = GameObject.FindWithTag("Spore");
        // Destroy(sporeObj);
    }

    Transform ClosestEnemy()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRange, enemyLayerMask);

        Transform theTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider collider in colliders)
        {
            Transform enemyTargets = collider.transform;

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

        while (timer < durationTime)
        {  
            float damageThisFrame = 5.0f * Time.deltaTime;
            enemy.GetComponent<NewEnemyHealth>().EnemyTakeDamage(damageThisFrame);

            Debug.Log($"Draining enemy: {damageThisFrame} damage | Time remaining: {durationTime - timer}");

            yield return null;
            timer += Time.deltaTime;
        }

        Debug.Log("Damage complete!");
    }

    IEnumerator HealingPlayer()
    {
        float timer = 0f;

        while (timer < durationTime)
        {  
            float damage = finalSkillValue;
            Debug.Log("Final Damage: " + damage);
            float healthThisFrame = damage * Time.deltaTime;

            //playerParent = GameObject.FindWithTag("PlayerParent");
            GameObject.FindWithTag("PlayerParent").GetComponent<NewPlayerHealth>().PlayerHeal(healthThisFrame);

            Debug.Log($"Healing player: {healthThisFrame} health | Time remaining: {durationTime - timer}");

            yield return null;
            timer += Time.deltaTime;
        }

        Debug.Log("Healing complete!");
    }

    IEnumerator DestroySpore(GameObject theSpore)
    {
        yield return new WaitForSeconds(durationTime);
        Destroy(theSpore);
    }
}
