using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributeBase : MonoBehaviour
{
    [HideInInspector] public EnemyHealth health;
    [HideInInspector] public ReworkedEnemyNavigation ai;
    [HideInInspector] public EnemyAttack attack;
    [HideInInspector] public GameObject enemy;
    [HideInInspector] public EnemyAttributeManager manager;

    private void Start()
    {
        enemy = gameObject;
        health = enemy.GetComponent<EnemyHealth>();
        ai = enemy.GetComponent<ReworkedEnemyNavigation>();
        attack = enemy.GetComponent<EnemyAttack>();
    }

    private void Awake()
    {
        manager = GetComponent<EnemyAttributeManager>();
        Initialize();
        if (manager != null)
        {
            manager.RefreshData();
        }
    }

    public virtual void Initialize()
    {
        // Add initialization code here
    }

    public virtual void OnTakeDamage(float damage)
    {
        // Modify behavior when the enemy takes damage
    }

    public virtual void OnDeath()
    {
        // Modify behavior when the enemy dies
    }

    public virtual void OnSpawn()
    {
        // Modify behavior when the enemy spawns
    }
}
