using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField] [Tooltip("Prefabs of each particle -- In other scripts, call this when the particle should spawn: SoundEffectManager.Instance.SpawnParticle(particleName, particleSpawnPosition, particleRotation)")]
    private List<GameObject> particlePrefabs = new List<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void SpawnParticles(string particleName, Vector3 particleSpawnPosition, Quaternion particleRotation, GameObject particleParent = null, Vector3? particleScale = null)
    {
        StartCoroutine(SpawnParticlesCoroutine(particleName, particleSpawnPosition, particleRotation, particleParent, particleScale));
    }

    public ParticleSystem SpawnParticlesAndGetParticleSystem(string particleName, Vector3 particleSpawnPosition, Quaternion particleRotation, GameObject particleParent = null)
    {
        GameObject spawnedParticle;
        if (particleParent != null)
        {
            spawnedParticle = Instantiate(FindParticlePrefab(particleName), particleSpawnPosition, particleRotation, particleParent.transform);
        }
        else
        {
           spawnedParticle = Instantiate(FindParticlePrefab(particleName), particleSpawnPosition, particleRotation);
        }
        StartCoroutine(DestroyParticlesWhenDone(spawnedParticle));

        ParticleSystem returnParticleSystem = spawnedParticle.GetComponent<ParticleSystem>();

        return returnParticleSystem;
    }

    IEnumerator SpawnParticlesCoroutine(string particleName, Vector3 particleSpawnPosition, Quaternion particleRotation, GameObject particleParent = null, Vector3? particleScale = null)
    {
        GameObject spawnedParticle = Instantiate(FindParticlePrefab(particleName), particleSpawnPosition, particleRotation);

        if (particleParent != null)
        {
            spawnedParticle.transform.SetParent(particleParent.transform);
        }
        if(particleScale.HasValue)
        {
            spawnedParticle.transform.localScale = particleScale.Value;
        }
        StartCoroutine(DestroyParticlesWhenDone(spawnedParticle));

        yield return null;
    }

    public void SpawnParticleFlurry(string particleName, int flurryCount, float flurryInterval, Vector3 particleSpawnPosition, Quaternion particleRotation)
    {
        StartCoroutine(SpawnParticleFlurryCoroutine(particleName, flurryCount, flurryInterval, particleSpawnPosition, particleRotation));
    }

    public void SpawnParticleFlurry(string particleName, int flurryCount, float flurryInterval, Vector3 particleSpawnPosition, Quaternion particleRotation, GameObject particleParent)
    {
        StartCoroutine(SpawnParticleFlurryCoroutine(particleName, flurryCount, flurryInterval, particleSpawnPosition, particleRotation, particleParent));
    }

    IEnumerator SpawnParticleFlurryCoroutine(string particleName, int flurryCount, float flurryInterval, Vector3 particleSpawnPosition, Quaternion particleRotation, GameObject particleParent = null)
    {
        float minisculeRandomTime = Random.Range(0, 0.25f);
        yield return new WaitForSeconds(minisculeRandomTime);

        float initialFlurryInterval = flurryInterval;

        float flurryCounter = 0f;
        while (flurryCounter < flurryCount)
        {
            GameObject spawnedParticle;
            if (particleParent != null)
            {
                spawnedParticle = Instantiate(FindParticlePrefab(particleName), particleSpawnPosition, particleRotation, particleParent.transform);
            }
            else
            {
               spawnedParticle = Instantiate(FindParticlePrefab(particleName), particleSpawnPosition, particleRotation);
            }
            StartCoroutine(DestroyParticlesWhenDone(spawnedParticle));

            flurryCounter++;

            float t = flurryCounter / flurryCount;
            float easeOutFactor = 1f - Mathf.Pow(1f - t, 3f);
            flurryInterval = initialFlurryInterval * easeOutFactor;

            yield return new WaitForSeconds(flurryInterval);
        }
    }

    GameObject FindParticlePrefab(string particleName)
    {
        foreach (GameObject particle in particlePrefabs)
        {
            if (particle.name.Contains(particleName))
            {
                return particle;
            }
        }

        Debug.Log("PARTICLE NAME NOT FOUND");
        return null;
    }

    IEnumerator DestroyParticlesWhenDone(GameObject particles)
    {
        while (particles != null)
        {
            if (particles.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return null;
            }
            else
            {
                Destroy(particles);
                yield return null;
            }
        }
    }
}
