using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth2 : EnemyHealth
{
    void OnTriggerEnter(Collider other)
    {
        BossHealthBar bossHealthBar = transform.Find("BossHealthCanvas").gameObject.GetComponent<BossHealthBar>();

        if (other.gameObject.CompareTag("currentPlayer") && !bossHealthBar.hasPopped)
        {
            bossHealthBar.EncounterEnemy();
        }
    }
}
