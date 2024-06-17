using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyShroom : MonoBehaviour
{
    public Animator sproutAnimator; // Animator for the sprout animation
    public GameObject enemy; // Reference to the enemy GameObject
    public float activationYPosition = 3.9f; // Y position to move the enemy to
    private bool isActive = false;

    void Start()
    {
        // Ensure the enemy is inactive at the start
        enemy.SetActive(false);

        // Ensure the enemy's Rigidbody is kinematic initially
        Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
        if (enemyRigidbody != null)
        {
            enemyRigidbody.isKinematic = true;
        }

        // Disable the enemy's navigation script at the start
        ReworkedEnemyNavigation enemyNavigation = enemy.GetComponent<ReworkedEnemyNavigation>();
        if (enemyNavigation != null)
        {
            enemyNavigation.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player hits the environmental object
        if (other.CompareTag("currentWeapon") && !isActive)
        {
            ActivateEnvironmentalObject();
        }
    }

    void ActivateEnvironmentalObject()
    {
        isActive = true;

        // Play the sprout animation
        sproutAnimator.SetTrigger("Sprout");

        // Disable colliders on the mushroom
        Collider[] mushroomColliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in mushroomColliders)
        {
            collider.enabled = false;
        }

        // Start a coroutine to activate the enemy after a delay
        StartCoroutine(ActivateEnemyAfterDelay(sproutAnimator.GetCurrentAnimatorStateInfo(0).length));
    }

    IEnumerator ActivateEnemyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Move the enemy to the activation position
        Vector3 newPosition = enemy.transform.position;
        newPosition.y += activationYPosition; // Adjust the y-position of the enemy by adding the offset
        enemy.transform.position = newPosition;

        // Activate the enemy
        enemy.SetActive(true);

        // Set the enemy's Rigidbody to non-kinematic to enable physics interactions
        Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
        if (enemyRigidbody != null)
        {
            enemyRigidbody.isKinematic = false;
        }

        // Enable the enemy's navigation script
        ReworkedEnemyNavigation enemyNavigation = enemy.GetComponent<ReworkedEnemyNavigation>();
        if (enemyNavigation != null)
        {
            enemyNavigation.enabled = true;
        }
    }
}