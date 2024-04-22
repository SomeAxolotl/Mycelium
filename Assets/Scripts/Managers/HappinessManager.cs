using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HappinessManager : MonoBehaviour
{
    public static HappinessManager Instance;

    public float averageColonyHappiness {private set; get;}
    public int colonySporeCount {private set; get;}

    [Header("Energy")]
    [SerializeField][Tooltip("Max amount of energy a Spore can have")] public int maxEnergy = 3;
    [SerializeField][Tooltip("Min amount of energy a Spore can have")] public int minEnergy = -2;

    [Header("Individual Happiness")]
    [SerializeField][Range(-0.5f, 0.5f)][Tooltip("When a Spore has >0 energy, the happiness they gain on completing an area")] public float happinessOnSpendingEnergy = 0.1f;
    [SerializeField][Range(-0.5f, 0.5f)][Tooltip("When a Spore has <=0 energy, the happiness they gain on completing an area")] public float happinessOnExhaustingEnergy = -0.1f;
    [SerializeField][Range(-0.5f, 0.5f)][Tooltip("The happiness change when a Spore dies")] public float happinessOnDying = -0.15f;
    [SerializeField][Range(-0.5f, 0.5f)][Tooltip("The happiness change for all Spores when another Spore dies in hardcore mode")] public float happinessOnAFriendDyingForever = -0.1f;
    [SerializeField][Range(-0.5f, 0.5f)][Tooltip("The happiness change for all Sporse when a Spore is grown")] public float happinessOnSporeGrown = 0.1f;

    [Header("Colony Happiness")]
    [SerializeField][Tooltip("Whether colony happiness multiplies or adds to Spore stats")] public bool doesHappinessMultiply = false;

    [Header("Colony Happiness - For Increments")]
    [SerializeField][Tooltip("Increments the lerping bounds per Spore at minimum colony happiness -- ONLY USED WITH INCREMENT")] float minHappinessIncrementPerSpore = -0.5f;
    [SerializeField][Tooltip("Increments the lerping bounds per Spore at maximum colony happiness -- ONLY USED WITH INCREMENT")] float maxHappinessIncrementPerSpore = 0.5f;

    [SerializeField][Tooltip("The cap for final minimum happiness increment -- ONLY USED WITH INCREMENT")] int minFinalHappinessIncrement = -5;
    [SerializeField][Tooltip("The cap for final maximum happiness increment -- ONLY USED WITH INCREMENT")] int maxFinalHappinessIncrement = 10;


    [Header("Colony Happiness - For Multipliers")]
    [SerializeField][Tooltip("Increments the lerping bounds per Spore at minimum colony happiness -- ONLY USED WITH MULTIPLY")] float minHappinessMultiplierPerSpore = -0.2f;
    [SerializeField][Tooltip("Increments the lerping bounds per Spore at maximum colony happiness -- ONLY USED WITH MULTIPLY")] float maxHappinessMultiplierPerSpore = 0.2f;

    [SerializeField][Tooltip("The cap for final minimum happiness multiplier -- ONLY USED WITH MULTIPLY")] float minFinalHappinessMultiplier = 0.25f;
    [SerializeField][Tooltip("The cap for final maximum happiness multiplier -- ONLY USED WITH MULTIPLY")] float maxFinalHappinessMultiplier = 10f;

    private void Awake()
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

    public float GetHappinessStatMultiplier()
    {
        averageColonyHappiness = GetAverageColonyHappiness();

        float statMultiplier = Mathf.Clamp((Mathf.Lerp(minHappinessMultiplierPerSpore, maxHappinessMultiplierPerSpore, averageColonyHappiness) * colonySporeCount) + 1f, minFinalHappinessMultiplier, maxFinalHappinessMultiplier);
        return statMultiplier;
    }
    public int GetHappinessStatIncrement()
    {
        averageColonyHappiness = GetAverageColonyHappiness();

        //Debug.Log("Average Happiness: " + averageColonyHappiness);
        //Debug.Log("Lerp: " + Mathf.Lerp(minHappinessIncrementPerSpore, maxHappinessIncrementPerSpore, averageColonyHappiness) * colonySporeCount);

        int statIncrement = Mathf.RoundToInt(Mathf.Clamp(Mathf.Lerp(minHappinessIncrementPerSpore, maxHappinessIncrementPerSpore, averageColonyHappiness) * colonySporeCount, minFinalHappinessIncrement, maxFinalHappinessIncrement));
        return statIncrement;
    }

    public float GetAverageColonyHappiness()
    {
        List<GameObject> colonySpores = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>().characters;
        colonySporeCount = colonySpores.Count;

        float totalHappiness = 0f;
        foreach (GameObject spore in colonySpores)
        {
            totalHappiness += spore.GetComponent<CharacterStats>().sporeHappiness;
        }

        return totalHappiness / colonySporeCount;
    }

    public void FriendlySporePermaDied()
    {
        SwapCharacter swapCharacter = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();

        if (swapCharacter.characters.Count > 1)
        {
            foreach (GameObject character in swapCharacter.characters)
            {
                character.GetComponent<CharacterStats>().ModifyHappiness(happinessOnAFriendDyingForever);
            }
        }
    }

    public void SporeGrown(GameObject spore)
    {
        SwapCharacter swapCharacter = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();

        if (swapCharacter.characters.Count > 1)
        {
            foreach (GameObject character in swapCharacter.characters)
            {
                if (character != spore)
                {
                    character.GetComponent<CharacterStats>().ModifyHappiness(happinessOnSporeGrown);
                }
            }
        }
    }

    public void RestOtherSpores(GameObject currentCharacter)
    {
        SwapCharacter swapCharacter = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();

        foreach (GameObject character in swapCharacter.characters)
        {
            if (character != currentCharacter)
            {
                character.GetComponent<CharacterStats>().ModifyEnergy(1);
            }
        }
    }

    public void RestAllSpores()
    {
        SwapCharacter swapCharacter = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();

        foreach (GameObject character in swapCharacter.characters)
        {
            character.GetComponent<CharacterStats>().ModifyEnergy(1);
        }
    }
}
