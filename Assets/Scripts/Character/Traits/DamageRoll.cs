using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRoll : MonoBehaviour
{
    public GameObject player;
    public CharacterStats characterStats;

    [SerializeField] private float damageAmount = 1;
    [SerializeField] private float knockbackRadius = 1f;
    [SerializeField] private float knockbackAmount = 12f;
    private List<GameObject> targets = new List<GameObject>();

    private void OnTriggerEnter(Collider other){
        if(!targets.Contains(other.gameObject)){
            Debug.Log("Deal roll damage to: " + other.gameObject);
            targets.Add(other.gameObject);
            EnemyHealth targetHealth = other.GetComponent<EnemyHealth>();
            EnemyKnockback enemyKnockback = other.gameObject.GetComponent<EnemyKnockback>();
            if(targetHealth != null && targetHealth.currentHealth > 0){
                targetHealth.EnemyTakeDamage(damageAmount + characterStats.speedLevel);
            }
            if(enemyKnockback != null){
                float distanceToCollider = Vector3.Distance(transform.position, other.transform.position);
                enemyKnockback.Knockback(knockbackAmount * (distanceToCollider / knockbackRadius), transform, other.transform, false);
            }
        }
    }

    void OnDisable(){
        targets.Clear();
    }
}
