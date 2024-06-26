using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseCollider : MonoBehaviour
{
    private float healingReduction;

    public void SetHealingReduction(float reduction)
    {
        healingReduction = reduction;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called.");
        if (other.CompareTag("currentPlayer"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.DisableRegen(); // Disable health regeneration
                Debug.Log("Player entered disease collider. Regen disabled");
                playerHealth.SetHealingReduction(healingReduction); // Apply healing reduction
                Debug.Log($"Player entered disease radius. Healing reduced by {healingReduction * 100}%");
            }
            else
            {
                Debug.Log("NOT WORKING");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit called.");
        if (other.CompareTag("currentPlayer"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.EnableRegen(); // Enable health regeneration
                Debug.Log("Player exited disease collider. Regen enabled");
                playerHealth.SetHealingReduction(0f); // Remove healing reduction
                playerHealth = null;
                Debug.Log("Player exited disease radius. Healing reduction removed");
            }
            else
            {
                Debug.Log("NOT WORKING");
            }
        }
    }
}