using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerativeHealth : MonoBehaviour
{
    [SerializeField][Tooltip("How many times healed a second")] float regenRate = 20;
    [SerializeField] EnemyHealth enemyHealth;

    IEnumerator Start()
    {
        while (true)
        {
            enemyHealth.Heal(enemyHealth.maxHealth / 1000f);

            yield return new WaitForSeconds(1f / regenRate);
        }
    }
}
