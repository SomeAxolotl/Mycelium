using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth2 : EnemyHealth
{
    void OnTriggerEnter(Collider other)
    {
        HUDBoss hudBoss = GameObject.Find("HUD").GetComponent<HUDBoss>();

        if (other.gameObject.CompareTag("currentPlayer") && !hudBoss.fightingBoss)
        {
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

        if (currentHealth <= 0 && !alreadyDead)
        {
            hudBoss.UpdateBossHealthUI(0f, maxHealth);
            StartCoroutine(Death());
        }

        hasTakenDamage = true;
    }
}
