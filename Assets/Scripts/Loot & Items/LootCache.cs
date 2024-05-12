using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using RonaldSunglassesEmoji.Interaction;
using System;
//using static UnityEditor.Progress;


public class LootCache : MonoBehaviour, IInteractable
{
    float distance;
    [SerializeField][Tooltip("The Loot Table Object used for this Cache;")] private LootTable lootTable;
    [SerializeField][Tooltip("Legacy. Used only for reference.")] private List<GameObject> possibleDrops;
    [SerializeField] private int nutrientMin = 50;
    [SerializeField] private int nutrientMax = 200;
    GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");

        //THIS IS A TEMPORARY FIX FOR CACHES!!! PLEASE GO INTO THE CHUNKS AND MANUALLY FIX THE NUTRIENT VALUES.
        if(nutrientMin >= 60)
        {
            nutrientMin = nutrientMin / 20;
        }

        if(nutrientMax >= 60)
        {
            nutrientMax = nutrientMax / 20;
        }
    }

    public void Interact(GameObject interactObject)
    {
        SoundEffectManager.Instance.PlaySound("Pickup", transform.position);
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", transform.position, Quaternion.Euler(-90,0,0));

        TooltipManager.Instance.DestroyTooltip();
        GetLoot();
        Destroy(this.gameObject);
    }

    public void Salvage(GameObject interactObject)
    {
        //buh
    }

    public void CreateTooltip(GameObject interactObject)
    {
        string buttonText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        TooltipManager.Instance.CreateTooltip
        (
            this.gameObject, 
            "Loot Cache", 
            "Contains Rewards!", 
            "Press "+buttonText+" to Open"
        );
    }

    public void DestroyTooltip(GameObject interactObject)
    {
        TooltipManager.Instance.DestroyTooltip();
    }

    public void GetLoot()
    {
        List<Loot> possibleItems = new List<Loot>();
        do
        {
            int randomIndex = UnityEngine.Random.Range(1,101);
            foreach(Loot item in lootTable.loots)
            {
                if(randomIndex <= item.DropChance)
                {
                    possibleItems.Add(item);
                }
            }
        } while (possibleItems.Count == 0);
        Loot droppedItem = possibleItems[UnityEngine.Random.Range(0,possibleItems.Count)];
        Instantiate(droppedItem.LootPrefab, new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
        
        int randomNutrientValue = UnityEngine.Random.Range(nutrientMin, nutrientMax);
        if (GlobalData.currentLoop >= 2)
        {
            randomNutrientValue = (randomNutrientValue * (GlobalData.currentLoop / 2));
        }
        //GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(randomNutrientValue);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", randomNutrientValue, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
    }
}