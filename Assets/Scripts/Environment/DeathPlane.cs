using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    [SerializeField] float percentHealthTaken = 0.2f;
    [SerializeField][Tooltip("The range at which an enemy falling will drop nutrients")] float rangeForEnemyDropNutrients = 20f;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "currentPlayer" || other.gameObject.tag == "Player")
        {
            RespawnObject(other.gameObject);
        }
        if (other.gameObject.tag == "Enemy" && !other.gameObject.name.Contains("Crab"))
        {
            if (Vector3.Distance(other.gameObject.transform.position, GameObject.FindWithTag("currentPlayer").transform.position) < rangeForEnemyDropNutrients)
            {
                KillObject(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
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
            GameManager.Instance.NavigateCamera();
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
