using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientParticles : MonoBehaviour
{
    [SerializeField] private int amountPerParticle = 20;
    [SerializeField] private float absorbTime = 1.5f;
    [SerializeField] private float timeBeforeAbsorb = 1.5f;
    private GameObject targetObject;
    private DesignTracker designTrackerScript;
    private NutrientTracker nutrientTrackerScript;

    [SerializeField] float nutrientHealAmount = 0.25f;

    void Start()
    {
        targetObject = GameObject.FindWithTag("currentPlayer");
        designTrackerScript = GameObject.FindWithTag("currentPlayer").GetComponent<DesignTracker>();
        nutrientTrackerScript = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        StartCoroutine(LerpParticlePositions());

        // Add initial debug log
        //Debug.Log($"Initial nutrient amount per particle: {amountPerParticle}");
    }

    IEnumerator LerpParticlePositions()
    {
        yield return new WaitForSeconds(timeBeforeAbsorb);

        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];

        int particleCount = particleSystem.GetParticles(particles);

        float absorbCounter = 0f;
        while (absorbCounter < absorbTime)
        {
            for (int i = 0; i <= particleCount - 1; i++)
            {
                float t = EaseInQuart(absorbCounter / absorbTime);
                particles[i].position = Vector3.Lerp(particles[i].position, targetObject.transform.position + new Vector3(0, 1, 0), t);
                particleSystem.SetParticles(particles, particleCount);

                if (Vector3.Distance(particles[i].position, targetObject.transform.position + new Vector3(0, 1, 0)) < 0.1f)
                {
                    SoundEffectManager.Instance.PlaySound("Pop", targetObject.transform);
                    particleSystem.Clear();
                    absorbCounter += absorbCounter;
                }
            }

            absorbCounter += Time.deltaTime;

            yield return null;
        }
        
        nutrientTrackerScript.AddNutrients(amountPerParticle);
        designTrackerScript.StartNutrientGlow();
        // Log the nutrient amount per particle when nutrients are gained
        //Debug.Log($"Nutrient amount per particle on gain: {amountPerParticle}");
        GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>().PlayerHeal(nutrientHealAmount);
    }

    float EaseInQuart(float x)
    {
        return Mathf.Pow(x, 4);
    }
}
