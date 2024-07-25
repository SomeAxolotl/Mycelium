using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenSpore : MonoBehaviour
{
    public Animator sproutAnimator; // Animator for the sprout animation
    public float activationYPositionOffset = 1f; // Offset to move the enemy up

    void Start()
    {
        // Ensure the hidden spore is inactive at the start
        gameObject.SetActive(false);

        // Disable the hidden spore's navigation script at the start
        ReworkedEnemyNavigation enemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        if (enemyNavigation != null)
        {
            enemyNavigation.enabled = false;
        }

        MushyAttack mushyAttack = GetComponent<MushyAttack>();
        if (mushyAttack != null)
        {
            mushyAttack.enabled = false;
        }
        else
        {
            Debug.LogError("Object does not have MushyAttack", gameObject);
        }
    }

    public void ActivateHiddenSpore()
    {
        Debug.Log("HiddenSpore activated"); // Debug statement

        // Move the hidden spore to the activation position
        Vector3 newPosition = transform.position;
        newPosition.y += activationYPositionOffset; // Adjust the y-position of the hidden spore by adding the offset
        transform.position = newPosition;

        Debug.Log("New HiddenSpore position: " + newPosition); // Debug statement

        // Activate the hidden spore
        gameObject.SetActive(true);

        // Play the sprout animation
        sproutAnimator.SetTrigger("Sprout");

        // Start a coroutine to enable its navigation after the animation delay
        StartCoroutine(EnableNavigationAfterDelay(sproutAnimator.GetCurrentAnimatorStateInfo(0).length));
    }

    private IEnumerator EnableNavigationAfterDelay(float delay)
    {
        Debug.Log("Delay Seconds: " + delay, gameObject);

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        float originalMass = rigidbody.mass;
        rigidbody.mass = 1000f;

        yield return new WaitForSeconds(delay);

        rigidbody.mass = originalMass;

        // Enable the hidden spore's navigation script
        ReworkedEnemyNavigation enemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        if (enemyNavigation != null)
        {
            enemyNavigation.enabled = true;
        }

        MushyAttack mushyAttack = GetComponent<MushyAttack>();
        if (mushyAttack != null)
        {
            mushyAttack.enabled = true;
        }
        else
        {
            Debug.LogError("Object does not have MushyAttack", gameObject);
        }
    }
}