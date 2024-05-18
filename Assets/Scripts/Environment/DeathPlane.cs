using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] float percentHealthTaken = 0.2f;
    [SerializeField][Tooltip("The range at which an enemy falling will drop nutrients")] float rangeForEnemyDropNutrients = 20f;
    [SerializeField][Tooltip("How far ahead of the player you jump to a checkpoint")] float checkpointSkipRange = 20f;

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "currentPlayer" || other.gameObject.tag == "Player") && other.GetType() == typeof(CapsuleCollider))
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
        // Find all respawn points with tags "Checkpoint" and "PlayerSpawn"
        GameObject[] checkpointRespawnPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        GameObject[] playerSpawnRespawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawn");
        
        // Combine the arrays into a single list
        List<GameObject> respawnPoints = new List<GameObject>(checkpointRespawnPoints);
        respawnPoints.AddRange(playerSpawnRespawnPoints);

        if (respawnPoints.Count == 0)
        {
            Debug.LogError($"No respawn points set for {gameObject}");
            return;
        }

        Rigidbody rb = other.GetComponent<Rigidbody>();

        //Teleports and constraints rigidbody
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        //Gets possible respawn points
        List<GameObject> possibleRespawnPoints = new List<GameObject>();
        foreach (GameObject respawnPoint in respawnPoints)
        {
            if (respawnPoint.transform.position.z + checkpointSkipRange >= other.transform.position.z)
            {
                possibleRespawnPoints.Add(respawnPoint);
            }
        }

        //Calculates the final respawn point
        GameObject closestPossibleRespawnPoint = null;
        float shortestDistance = float.MaxValue;
        foreach (GameObject possibleRespawnPoint in possibleRespawnPoints)
        {
            float distance = Vector3.Distance(other.transform.position, possibleRespawnPoint.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPossibleRespawnPoint = possibleRespawnPoint;
            }
        }

        if (closestPossibleRespawnPoint != null)
        {
            other.transform.position = closestPossibleRespawnPoint.transform.position;
        }
        else
        {
            other.transform.position = respawnPoints[0].transform.position;
            Debug.LogWarning($"No closest respawn point found from death plane {gameObject}");
        }

        if (rb!= null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        //Does damage to player
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
