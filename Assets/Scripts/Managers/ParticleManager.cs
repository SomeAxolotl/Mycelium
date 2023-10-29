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

    public void SpawnParticle(string particleName, Vector3 particleSpawnPosition, Vector3 particleRotation)
    {
        StartCoroutine(SpawnParticleCoroutine(particleName, particleSpawnPosition, particleRotation));
    }

    IEnumerator SpawnParticleCoroutine(string particleName, Vector3 particleSpawnPosition, Vector3 particleRotation)
    {
        foreach (GameObject particle in particlePrefabs)
        {
            if (particle.name == particleName)
            {
                GameObject spawnedParticle = Instantiate(particle, particleSpawnPosition, Quaternion.Euler(particleRotation));
                while (spawnedParticle.GetComponent<ParticleSystem>().isPlaying)
                {
                    yield return null;
                }
                Destroy(spawnedParticle);
                yield break;
            }
            else
            {
                Debug.Log("PARTICLE NAME NOT FOUND");
                yield break;
            }
        }

    }
}
