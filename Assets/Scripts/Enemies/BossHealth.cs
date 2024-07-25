using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class BossHealth : EnemyHealth
{
    bool hudPopup = false;
    [SerializeField] Collider hudCollider;
    [SerializeField] private List<GameObject> enemySpawners = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        HUDBoss hudBoss = GameObject.Find("HUD").GetComponent<HUDBoss>();

        if (other.gameObject.CompareTag("currentPlayer") && !hudPopup)
        {
            hudPopup = true;
            hudCollider.enabled = false;
            hudBoss.EncounterBoss(gameObject.name, currentHealth, maxHealth);
            if(SceneManager.GetActiveScene().name == "Impact Barrens")
            {
                StartCoroutine(GameObject.Find("Rival Sporemother").GetComponent<MonsterBossAttack>().WaitForIntroToEnd());
            }
        }
    }
    public override void EnemyTakeDamage(float damage, bool wasFromDeathPlane = false, bool wasFromWeapon = true)
    {
        // Check for ArmoredAttribute and apply damage reduction
        Armored armoredAttribute = GetComponent<Armored>();
        if (armoredAttribute != null)
        {
            damage = armoredAttribute.ApplyDamageReduction(damage);
        }
        //Save current damage taken
        dmgTaken = damage;
        //Call action to modify damage
        TakeDamage?.Invoke(dmgTaken);

        currentHealth -= dmgTaken;
        ParticleManager.Instance.SpawnParticles("Blood", centerPoint.position, Quaternion.identity);

        HUDBoss hudBoss = GameObject.Find("HUD").GetComponent<HUDBoss>();
        if (currentHealth + dmgTaken > 0)
        {
            hudBoss.UpdateBossHealthUI(currentHealth, maxHealth);
        }

        if ((currentHealth <= maxHealth / 2) && (!alreadyDead) && (currentHealth >= 1))
        {
            // activate all enemy spawners when health is less than 50%
            foreach (var enemy in enemySpawners)
            {
                enemy.SetActive(true);
            }
        }

        if (currentHealth <= 0 && !alreadyDead)
        {
            Died?.Invoke();
            Actions.EnemyKilled?.Invoke(this);

            hudBoss.UpdateBossHealthUI(0f, maxHealth);
            if(gameObject.name == "Rival Sporemother")
            {
                DestroyNonBossEnemies();
                StartCoroutine(BossDeath());
                gameObject.GetComponent<MonsterBossAttack>().enabled = false;
                gameObject.GetComponent<TempMovement>().enabled = false;
            }
            else if(gameObject.name == "Giga Beetle")
            {
                StartCoroutine(Death());
            }
        }

        hasTakenDamage = true;
    }

    public void DestroyNonBossEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemyObj in enemies)
        {
            if (enemyObj.name != "Rival Sporemother")
            {
                Debug.Log("Enemy Spawner: " + enemyObj.name);
                enemyObj.SetActive(false);
            }
        }

        EnemySpawner[] enemySpawners = GameObject.FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            Destroy(enemySpawner.gameObject);
        }
    }
}