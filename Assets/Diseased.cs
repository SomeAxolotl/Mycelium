using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diseased : EnemyAttributeBase
{
    private EnemyHealth enemyHealth;
    private GameObject diseaseColliderObject;

    public override void Initialize()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth != null && enemyHealth.isMiniBoss)
        {
            Debug.Log("Applying Diseased attribute to miniboss: " + enemyHealth.miniBossName);
            enemyHealth.AddAttributePrefix("Diseased"); // Add the attribute prefix to the miniboss name

            // Find the DiseaseCollider child object
            diseaseColliderObject = transform.Find("DiseaseCollider").gameObject;

            if (diseaseColliderObject != null)
            {
                Debug.Log("Disease collider activated.");
                diseaseColliderObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Disease collider object is not found. Make sure the DiseaseCollider GameObject is a child of the miniboss.");
            }
        }
    }
}
