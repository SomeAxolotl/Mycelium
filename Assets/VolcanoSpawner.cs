using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float maxInterval = 3f;
    [SerializeField] private float minLaunchForce = 10f;
    [SerializeField] private float maxLaunchForce = 30f;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private Vector3 gravity = new Vector3(0, -20f, 0);

    private void Start()
    {
        StartCoroutine(SpawnProjectiles());
    }

    private IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            LaunchProjectile(projectile);
        }
    }

    private void LaunchProjectile(GameObject projectile)
    {
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Projectile does not have a Rigidbody component.");
            return;
        }

        Vector3 launchDirection = Random.onUnitSphere;
        launchDirection.y = 0;
        launchDirection.Normalize();

        float launchForce = Random.Range(minLaunchForce, maxLaunchForce);
        float launchAngleRad = launchAngle * Mathf.Deg2Rad;

        Vector3 velocity = launchDirection * launchForce * Mathf.Cos(launchAngleRad);
        velocity.y = launchForce * Mathf.Sin(launchAngleRad);

        rb.AddForce(velocity, ForceMode.VelocityChange);
    }
}
