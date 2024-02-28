using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    [SerializeField] float percentHealthTaken = 0.2f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" || other.gameObject.tag == "Player")
        {
            RespawnObject(other.gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {
            KillObject(other.gameObject);
        }
    }

    void RespawnObject(GameObject other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        other.transform.position = respawnPoint.position;

        if (rb!= null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if (other.tag == "currentPlayer")
        {
            PlayerHealth playerHealth = other.transform.parent.GetComponent<PlayerHealth>();

            float healthFraction = playerHealth.currentHealth * percentHealthTaken;
            float clampedDamageValue = Mathf.Clamp(healthFraction, 1f, playerHealth.currentHealth);

            playerHealth.PlayerTakeDamage(clampedDamageValue);
        }
    }

    void KillObject(GameObject other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.EnemyTakeDamage(enemyHealth.maxHealth);
        }
    }
}
