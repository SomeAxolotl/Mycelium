using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] float percentHealthTaken = 0.2f;
    [SerializeField][Tooltip("The range at which an enemy falling will drop nutrients")] float rangeForEnemyDropNutrients = 20f;
    [SerializeField][Tooltip("How far ahead of the player you jump to a checkpoint")] float checkpointSkipRange = 20f;

    float timeUntilRespawn = 0.2f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" && other.GetType() == typeof(CapsuleCollider))
        {
            //Only respawn them if the player can pause. This ensures this can't be called twice when hitting two overlapping deathplanes
            if (GlobalData.isAbleToPause)
            {
                StartCoroutine(RespawnPlayer());
            }
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

    IEnumerator RespawnPlayer()
    {
        GlobalData.isAbleToPause = false;

        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");

        //Does damage to player
        PlayerHealth playerHealth = currentPlayer.transform.parent.GetComponent<PlayerHealth>();

        float healthFraction = playerHealth.currentHealth * percentHealthTaken;
        float clampedDamageValue = Mathf.Clamp(healthFraction, 1f, playerHealth.currentHealth);

        playerHealth.PlayerTakeDamage(clampedDamageValue);

        // Find all respawn points with tags "Checkpoint" and "PlayerSpawn"
        GameObject[] checkpointRespawnPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        GameObject[] playerSpawnRespawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawn");
        
        // Combine the arrays into a single list
        List<GameObject> respawnPoints = new List<GameObject>(checkpointRespawnPoints);
        respawnPoints.AddRange(playerSpawnRespawnPoints);

        if (respawnPoints.Count == 0)
        {
            Debug.LogError($"No respawn points set for {gameObject}");
            yield break;
        }

        Rigidbody rb = currentPlayer.GetComponent<Rigidbody>();

        //Gets possible respawn points
        List<GameObject> possibleRespawnPoints = new List<GameObject>();
        foreach (GameObject respawnPoint in respawnPoints)
        {
            if (respawnPoint.transform.position.z + checkpointSkipRange >= currentPlayer.transform.position.z)
            {
                possibleRespawnPoints.Add(respawnPoint);
            }
        }

        //Calculates the final respawn point
        GameObject closestPossibleRespawnPoint = null;
        float shortestDistance = float.MaxValue;
        foreach (GameObject possibleRespawnPoint in possibleRespawnPoints)
        {
            float distance = Vector3.Distance(currentPlayer.transform.position, possibleRespawnPoint.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPossibleRespawnPoint = possibleRespawnPoint;
            }
        }

        HUDController hudController = GameObject.Find("HUD").GetComponent<HUDController>();
        yield return hudController.StartCoroutine(hudController.BlackFade(true));
        yield return new WaitForSeconds(timeUntilRespawn / 2);

        //Teleports the player (and navigates camera immediately)
        if (closestPossibleRespawnPoint != null)
        {
            currentPlayer.transform.position = closestPossibleRespawnPoint.transform.position;
        }
        else
        {
            currentPlayer.transform.position = respawnPoints[0].transform.position;
            Debug.LogWarning($"No closest respawn point found from death plane {gameObject}");
        }
        GameManager.Instance.NavigateCamera();

        //Constraints rigidbody for a frame to prevent clipping
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        yield return null;

        if (rb!= null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        yield return new WaitForSeconds(timeUntilRespawn);
        yield return hudController.StartCoroutine(hudController.BlackFade(false));

        GlobalData.isAbleToPause = true;
    }

    void KillObject(GameObject other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.EnemyTakeDamage(enemyHealth.maxHealth, true);
        }
    }
}
