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
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SpawnParticles(string particleName, Vector3 particleSpawnPosition, Quaternion particleRotation)
    {
        StartCoroutine(SpawnParticlesCoroutine(particleName, particleSpawnPosition, particleRotation));
    }

    public void SpawnParticles(string particleName, Vector3 particleSpawnPosition, Quaternion particleRotation, GameObject particleParent)
    {
        StartCoroutine(SpawnParticlesCoroutine(particleName, particleSpawnPosition, particleRotation, particleParent));
    }

    IEnumerator SpawnParticlesCoroutine(string particleName, Vector3 particleSpawnPosition, Quaternion particleRotation, GameObject particleParent = null)
    {
        foreach (GameObject particle in particlePrefabs)
        {
            if (particle.name == particleName)
            {
                GameObject spawnedParticle;
                if (particleParent != null)
                {
                    spawnedParticle = Instantiate(particle, particleSpawnPosition, particleRotation, particleParent.transform);
                }
                else
                {
                   spawnedParticle = Instantiate(particle, particleSpawnPosition, particleRotation);
                }

                while (spawnedParticle.GetComponent<ParticleSystem>().isPlaying)
                {
                    yield return null;
                }
                Destroy(spawnedParticle);
                yield break;
            }
        }
        Debug.Log("PARTICLE NAME NOT FOUND");
        yield break;
    }
}
