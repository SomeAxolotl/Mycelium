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
        if (GlobalData.currentLoop > 1)
        {
            EnemyAttributeManager.AssignAttributesToExistingEnemies();
        }
        else
        {
            ClearAllAttributes();
        }
    }

    public void AssignAttributes(GameObject enemy)
    {
        var manager = enemy.GetComponent<EnemyAttributeManager>();
        if (manager != null)
        {
            for (int i = 1; i < GlobalData.currentLoop; i++)
            {
                AssignSingleAttribute(enemy);
            }
            manager.RefreshData();
        }
    }

    private void AssignSingleAttribute(GameObject enemy)
    {
        // Ensure only one attribute is added per loop
        List<EnemyAttributeBase> possibleAttributes = new List<EnemyAttributeBase>(allAttributes);
        foreach (EnemyAttributeBase attribute in enemy.GetComponents<EnemyAttributeBase>())
        {
            possibleAttributes.RemoveAll(attr => attr.GetType() == attribute.GetType());
        }

        if (possibleAttributes.Count > 0)
        {
            EnemyAttributeBase randomAttribute = possibleAttributes[UnityEngine.Random.Range(0, possibleAttributes.Count)];
            Component newComponent = enemy.AddComponent(randomAttribute.GetType());
            EnemyAttributeBase newAttribute = newComponent as EnemyAttributeBase;
            newAttribute.Initialize();
        }
    }

    private void ClearAllAttributes()
    {
        var enemies = FindObjectsOfType<EnemyAttributeManager>();
        foreach (var enemy in enemies)
        {
            enemy.ClearAttributes();
        }
    }
}
