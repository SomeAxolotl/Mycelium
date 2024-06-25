using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributeManager : MonoBehaviour
{
    private List<EnemyAttributeBase> attributes;

    private void Awake()
    {
        attributes = new List<EnemyAttributeBase>(GetComponents<EnemyAttributeBase>());
    }

    public void RefreshData()
    {
        foreach (var attribute in attributes)
        {
            attribute.Initialize();
        }
    }

    public void OnEnemyTakeDamage(float damage)
    {
        foreach (var attribute in attributes)
        {
            attribute.OnTakeDamage(damage);
        }
    }

    public void OnEnemyDeath()
    {
        foreach (var attribute in attributes)
        {
            attribute.OnDeath();
        }
    }

    public void OnEnemySpawn()
    {
        foreach (var attribute in attributes)
        {
            attribute.OnSpawn();
        }
    }

    public void ClearAttributes()
    {
        foreach (var attribute in attributes)
        {
            Destroy(attribute);
        }
        attributes.Clear();
    }

    public static void AssignAttributesToExistingEnemies()
    {
        var enemies = FindObjectsOfType<EnemyAttributeManager>();
        foreach (var enemy in enemies)
        {
            EnemyAttributeAssigner.Instance.AssignAttributes(enemy.gameObject);
        }
    }
}
