using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diseased : EnemyAttributeBase
{
    private GameObject diseaseColliderObject;
    private float healingReduction = 0.10f; // Healing reduction percentage

    protected override void OnInitialize()
    {
        // Find the DiseaseCollider child object
        diseaseColliderObject = transform.Find("DiseaseCollider").gameObject;

        if (diseaseColliderObject != null)
        {
            Debug.Log("Disease collider activated.");
            diseaseColliderObject.SetActive(true);
            diseaseColliderObject.GetComponent<DiseaseCollider>().SetHealingReduction(healingReduction);
        }
        else
        {
            Debug.LogError("Disease collider object is not found. Make sure the DiseaseCollider GameObject is a child of the miniboss.");
        }
    }
}
