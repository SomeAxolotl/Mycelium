using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth2 : EnemyHealth
{
    bool hudPopup = false;
   [SerializeField] private List<GameObject> enemySpawners = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        HUDBoss hudBoss = GameObject.Find("HUD").GetComponent<HUDBoss>();

        if (other.gameObject.CompareTag("currentPlayer") && !hudPopup)
        {
            hudPopup = true;
            if (GameObject.Find("Rival Colony Leader") != null)
            {
                GameObject.Find("Rival Colony Leader").GetComponent<MonsterBossAttack>().DoRandomAttack();
            }
            hudBoss.EncounterBoss(gameObject.name, currentHealth, maxHealth);
        }
    }

    public override void EnemyTakeDamage(float dmgTaken)
    {
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
            hudBoss.UpdateBossHealthUI(0f, maxHealth);
            if(gameObject.name == "Rival Colony Leader")
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
            if (enemyObj.name != "Rival Colony Leader")
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