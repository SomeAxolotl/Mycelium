using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeburstParticles : MonoBehaviour
{
    /*[SerializeField] private float expandTime = 0.75f;
    [HideInInspector] public float travelRange = 5f;

    void Start()
    {
        StartCoroutine(LerpParticlePositions());
    }

    IEnumerator LerpParticlePositions()
    {   
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[100];

        int particleCount = particleSystem.GetParticles(particles);

        List<Vector3> endingPositions = new List<Vector3>();
        for (int i = 0; i <= particleCount - 1; i++)
        {
            endingPositions.Add(particles[i].velocity * travelRange);
        }

        float expandCounter = 0f;
        while (expandCounter < expandTime)
        {
            for (int i = 0; i <= particleCount - 1; i++)
            {
                float t = EaseOutQuart(expandCounter / expandTime);
                particles[i].position = Vector3.Lerp(particles[i].position, endingPositions[i], t);
                particleSystem.SetParticles(particles, particleCount);
            }

            expandCounter += Time.deltaTime;
            yield return null;
        }
    }

    float EaseOutQuart(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4);
    }*/
}
