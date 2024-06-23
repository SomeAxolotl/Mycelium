using RonaldSunglassesEmoji.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretCache : MonoBehaviour, IInteractable
{
    [SerializeField][Tooltip("Custom loot table for secret caches")] private LootTable secretLootTable;
    [SerializeField] private int nutrientMin = 50;
    [SerializeField] private int nutrientMax = 200;
    private bool canInteract = false;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");

        //THIS IS A TEMPORARY FIX FOR CACHES!!! PLEASE GO INTO THE CHUNKS AND MANUALLY FIX THE NUTRIENT VALUES.
        if (nutrientMin >= 60)
        {
            nutrientMin = nutrientMin / 20;
        }

        if (nutrientMax >= 60)
        {
            nutrientMax = nutrientMax / 20;
        }
    }

    public void Interact(GameObject interactObject)
    {
        if (!canInteract)
            return;

        SoundEffectManager.Instance.PlaySound("Cache", transform.position);
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", transform.position, Quaternion.Euler(-90, 0, 0));

        TooltipManager.Instance.DestroyTooltip();
        GetLoot();
        Destroy(this.gameObject);
    }

    public void Salvage(GameObject interactObject)
    {
        // Salvage logic here
    }

    public void CreateTooltip(GameObject interactObject)
    {
        if (!canInteract)
            return;

        string buttonText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        TooltipManager.Instance.CreateTooltip
        (
            this.gameObject,
            "Secret Cache",
            "Contains Special Rewards!",
            "Press " + buttonText + " to Open"
        );
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        TooltipManager.Instance.DestroyTooltip();
    }

    public void GetLoot()
    {
        Loot droppedItem = PickByWeight(secretLootTable.loots);

        GameObject droppedObject = Instantiate(droppedItem.LootPrefab, new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);

        int randomNutrientValue = UnityEngine.Random.Range(nutrientMin, nutrientMax);
        if (GlobalData.currentLoop >= 2)
        {
            randomNutrientValue = (randomNutrientValue * (GlobalData.currentLoop / 2));
        }
        //GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(randomNutrientValue);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", randomNutrientValue, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
    }

    private Loot PickByWeight(List<Loot> loots)
    {
        float totalChance = 0;
        foreach (Loot currLoot in loots)
        {
            totalChance += currLoot.DropChance;
        }
        float randomValue = UnityEngine.Random.Range(0f, totalChance);
        foreach (Loot currLoot in loots)
        {
            randomValue -= currLoot.DropChance;
            if (randomValue <= 0)
            {
                return currLoot;
            }
        }
        return loots[UnityEngine.Random.Range(0, loots.Count)];
    }

    public void EnableInteraction()
    {
        canInteract = true;
    }
}
