using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Nutritious : AttributeBase
{
    public float nutrientIncrease = 0.5f; // 50% increase as a factor
    private float damageReduction = 0.25f; // 25% reduction as a factor

    public override void Initialize()
    {
        attName = "Nutritious";
        attDesc = "Gain 50% more nutrients, but deal 25% less damage";
        Debug.Log("[Nutritious] Initializing Nutritious attribute...");

        if (stats == null || hit == null)
        {
            Debug.LogWarning("[Nutritious] Stats or hit is null.");
            return;
        }

        // Apply damage reduction
        stats.statNums.advDamage.AddModifier(new StatModifier(-damageReduction, StatModType.PercentAdd, this));
        Debug.Log("[Nutritious] Applied damage reduction.");

        // Find all instances of NutrientParticles and apply the nutrient increase
        NutrientParticles[] nutrientParticles = GameObject.FindObjectsOfType<NutrientParticles>();
        Debug.Log($"[Nutritious] Found {nutrientParticles.Length} instances of NutrientParticles.");

        foreach (var particle in nutrientParticles)
        {
            Debug.Log($"[Nutritious] Applying nutrient increase to particle: {particle.gameObject.name}");
            ApplyNutrientIncrease(particle);
        }
    }

    private void ApplyNutrientIncrease(NutrientParticles particle)
    {
        // Access the amountPerParticle field using reflection
        FieldInfo fieldInfo = typeof(NutrientParticles).GetField("amountPerParticle", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            int currentAmount = (int)fieldInfo.GetValue(particle);
            int increasedAmount = Mathf.RoundToInt(currentAmount * (1 + nutrientIncrease));

            // Log the original and increased amounts
            Debug.Log($"[Nutritious] Original nutrient amount per particle: {currentAmount}");
            Debug.Log($"[Nutritious] Increased nutrient amount per particle: {increasedAmount}");

            // Set the new increased amount
            fieldInfo.SetValue(particle, increasedAmount);

            // Verify the change
            int updatedAmount = (int)fieldInfo.GetValue(particle);
            Debug.Log($"[Nutritious] Verified nutrient amount per particle after update: {updatedAmount}");
        }
        else
        {
            Debug.LogError("[Nutritious] amountPerParticle field not found in NutrientParticles.");
        }
    }
}

