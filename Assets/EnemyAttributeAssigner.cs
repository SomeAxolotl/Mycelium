using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAttributeAssigner : MonoBehaviour
{
    public static EnemyAttributeAssigner Instance;

    [SerializeField] private GameObject attributesParent;
    private EnemyAttributeBase[] allAttributes;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        allAttributes = attributesParent.GetComponentsInChildren<EnemyAttributeBase>();
    }

    void Start()
    {
        AssignAttributesToExistingEnemies();
    }

    private void AssignAttributesToExistingEnemies()
    {
        EnemyAttributeManager[] enemies = FindObjectsOfType<EnemyAttributeManager>();
        foreach (EnemyAttributeManager enemy in enemies)
        {
            AssignRandomAttribute(enemy.gameObject);
        }
    }

    public void AssignRandomAttribute(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyAttributeManager>() == null)
        {
            enemy.AddComponent<EnemyAttributeManager>();
        }

        EnemyAttributeBase randomAttribute = allAttributes[UnityEngine.Random.Range(0, allAttributes.Length)];
        Component newComponent = enemy.AddComponent(randomAttribute.GetType());
        EnemyAttributeBase newAttribute = newComponent as EnemyAttributeBase;
        newAttribute.Initialize();
    }
}
