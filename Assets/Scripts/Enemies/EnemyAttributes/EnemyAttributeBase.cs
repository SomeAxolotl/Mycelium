using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttributeBase : MonoBehaviour
{
    protected EnemyHealth enemyHealth;
    

    public virtual void Initialize()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null && enemyHealth.isMiniBoss)
        {
            Debug.Log($"Applying {GetAttributeName()} attribute to miniboss: {enemyHealth.miniBossName}");
            enemyHealth.AddAttributePrefix(GetAttributeName());
            OnInitialize();
        }
    }

    protected abstract void OnInitialize();

    protected string GetAttributeName()
    {
        return $"{GetType().Name}";
    }

    public virtual void OnTakeDamage(float damage) { }
    public virtual void OnEnemyDealDamage(float damageDealt) { }

    public virtual void OnDeath() { }

    public virtual void OnSpawn() { }
}
