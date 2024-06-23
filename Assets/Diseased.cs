using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diseased : EnemyAttributeBase
{
    /*[SerializeField] private float radius = 5f; // The radius of the sphere
    [SerializeField] private LayerMask playerLayer; // Ensure this is set in the inspector

    private bool playerInRange = false;

    public override void Initialize()
    {
        // Ensure playerLayer is set to the Player layer
        playerLayer = LayerMask.GetMask("Player");
        StartCoroutine(CheckForPlayer());
    }

    private IEnumerator CheckForPlayer()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, playerLayer);
            bool playerFound = false;
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("currentPlayer"))
                {
                    PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        //playerHealth.DisableRegen();
                        playerFound = true;
                    }
                }
            }
            if (!playerFound && playerInRange)
            {
                // If the player was in range but is not anymore
                Collider[] players = Physics.OverlapSphere(transform.position, radius, playerLayer);
                foreach (Collider playerCollider in players)
                {
                    if (playerCollider.CompareTag("currentPlayer"))
                    {
                        PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
                        if (playerHealth != null)
                        {
                            //playerHealth.EnableRegen();
                        }
                    }
                }
            }
            playerInRange = playerFound;
            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }*/
}
