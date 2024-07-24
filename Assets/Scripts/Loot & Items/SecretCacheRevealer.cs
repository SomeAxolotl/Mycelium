using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretCacheRevealer : MonoBehaviour
{
    [SerializeField][Tooltip("The cache that will be revealed")] private GameObject cacheObject;
    [SerializeField][Tooltip("The y-value to which the cache will rise relative to its initial position")] private float revealYValue = 10f;
    [SerializeField][Tooltip("The initial upward force to apply to the cache")] private float upwardForce = 30f;
    [SerializeField][Tooltip("The collider that will trigger the reveal")] private Collider triggerCollider;

    private bool isRevealed = false;
    private Rigidbody cacheRigidbody;

    private void Start()
    {
        if (cacheObject != null)
        {
            // Ensure the cache object has a Rigidbody component
            cacheRigidbody = cacheObject.GetComponent<Rigidbody>();
            if (cacheRigidbody == null)
            {
                cacheRigidbody = cacheObject.AddComponent<Rigidbody>();
            }
            cacheRigidbody.useGravity = false; // Disable gravity initially
            cacheRigidbody.isKinematic = true; // Disable physics simulation initially
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        WeaponStats weaponStats = other.GetComponent<WeaponStats>();
        if (!isRevealed && weaponStats != null && weaponStats.weaponType == WeaponStats.WeaponTypes.Smash)
        {
            StartCoroutine(RevealCache());
        }
    }

    private IEnumerator RevealCache()
    {
        isRevealed = true;
        Vector3 startPosition = cacheObject.transform.position;

        // Set the Rigidbody to use physics simulation
        cacheRigidbody.isKinematic = false;
        cacheRigidbody.useGravity = true;

        // Apply an upward force to the cache
        cacheRigidbody.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);

        // Wait until the cache reaches its peak and starts falling
        yield return new WaitUntil(() => cacheRigidbody.velocity.y <= 0);

        // Constrain the x-axis movement
        cacheRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;

        // Enable interaction with the cache once it starts falling
        var secretCache = cacheObject.GetComponent<SecretCache>();
        if (secretCache != null)
        {
            secretCache.EnableInteraction();
        }

        // Remove the trigger collider
        if (triggerCollider != null)
        {
            Destroy(triggerCollider);
        }

        // Remove the Rigidbody component after a short delay to ensure the cache starts falling
        yield return new WaitForSeconds(0.5f);
        if (cacheRigidbody != null)
        {
            Destroy(cacheRigidbody);
        }
    }
}
