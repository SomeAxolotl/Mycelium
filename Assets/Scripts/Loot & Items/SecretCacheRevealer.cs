using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretCacheRevealer : MonoBehaviour
{
    [SerializeField][Tooltip("The cache that will be revealed")] private GameObject cacheObject;
    [SerializeField][Tooltip("The y-value to which the cache will rise")] private float revealYValue = 1f;
    [SerializeField][Tooltip("The tag of the heavy weapon")] private string heavyWeaponTag = "Slam";
    [SerializeField][Tooltip("The speed at which the cache rises")] private float riseSpeed = 2f;
    [SerializeField][Tooltip("Delay before the cache becomes accessible")] private float accessibilityDelay = 1f;

    private bool isRevealed = false;
    private Vector3 targetPosition;

    private void Start()
    {
        if (cacheObject != null)
        {
            targetPosition = new Vector3(cacheObject.transform.position.x, revealYValue, cacheObject.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isRevealed && other.CompareTag(heavyWeaponTag))
        {
            WeaponStats weaponStats = other.GetComponent<WeaponStats>();
            if (weaponStats != null && weaponStats.weaponType == WeaponStats.WeaponTypes.Smash)
            {
                StartCoroutine(RevealCache());
            }
        }
    }

    private IEnumerator RevealCache()
    {
        isRevealed = true;
        float elapsedTime = 0f;
        Vector3 startPosition = cacheObject.transform.position;

        while (elapsedTime < 1f)
        {
            cacheObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * riseSpeed;
            yield return null;
        }

        cacheObject.transform.position = targetPosition;
        yield return new WaitForSeconds(accessibilityDelay);

        // Enable interaction with the cache
        var secretCache = cacheObject.GetComponent<SecretCache>();
        if (secretCache != null)
        {
            secretCache.enabled = true;
        }
    }
}
