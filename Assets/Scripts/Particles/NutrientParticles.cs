using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientParticles : MonoBehaviour
{
    [SerializeField] private float absorbTime = 1.5f;
    [SerializeField] private float timeBeforeAbsorb = 1.5f;
    private GameObject targetObject;

    void Start()
    {
        targetObject = GameObject.FindWithTag("currentPlayer");
        StartCoroutine(LerpParticlePositions());
    }

    IEnumerator LerpParticlePositions()
    {   
        yield return new WaitForSeconds(timeBeforeAbsorb);

        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[100];

        int particleCount = particleSystem.GetParticles(particles);

        float absorbCounter = 0f;
        while (absorbCounter < absorbTime)
        {
            for (int i = 0; i <= particleCount - 1; i++)
            {
                float t = EaseInQuart(absorbCounter / absorbTime);
                particles[i].position = Vector3.Lerp(particles[i].position, targetObject.transform.position, t);
                particleSystem.SetParticles(particles, particleCount);

                Debug.Log("ParticlePosition: " + particles[0].position);
                Debug.Log("PlayerPosition: " + targetObject.transform.position);

                if (Vector3.Distance(particles[i].position, targetObject.transform.position) < 0.1f)
                {
                    particles[i].remainingLifetime = 0f;
                }
            }

            absorbCounter += Time.deltaTime;

            yield return null;
        }
    }

    float EaseInQuart(float x)
    {
        return Mathf.Pow(x, 4);
    }
}
