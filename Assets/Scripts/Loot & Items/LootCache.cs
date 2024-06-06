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
    [SerializeField][Tooltip("Loot table of all weapons")] private LootTable weaponTable;
    [SerializeField][Tooltip("Chance to pull a weapon from cache")] private float weaponChance;
    [SerializeField][Tooltip("Loot table of all stats")] private LootTable statTable;
    [SerializeField][Tooltip("Chance to pull a stat from cache")] private float statChance;
    [SerializeField][Tooltip("Loot table of all resources")] private LootTable resourceTable;
    [SerializeField][Tooltip("Chance to pull a resource from cache")] private float resourceChance;
    [HideInInspector][Tooltip("Legacy. Used only for reference.")] private List<GameObject> possibleDrops;
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
        SoundEffectManager.Instance.PlaySound("Cache", transform.position);
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

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        TooltipManager.Instance.DestroyTooltip();
    }

    public void GetLoot()
    {
        float totalChance = (weaponChance + statChance + resourceChance) + (RandomManager.bonusWeaponChance + RandomManager.bonusStatChance + RandomManager.bonusResourceChance);
        float randomValue = UnityEngine.Random.Range(0f, totalChance);
        Loot droppedItem;
        bool isWeapon = false;
        if(randomValue <= weaponChance + RandomManager.bonusWeaponChance){
            droppedItem = PickByWeight(weaponTable.loots);
            isWeapon = true;
            RandomManager.bonusWeaponChance = 0;
            RandomManager.bonusStatChance += statChance * 0.1f;
            RandomManager.bonusResourceChance += resourceChance * 0.1f;
        }else if(randomValue - weaponChance <= statChance + RandomManager.bonusStatChance){
            droppedItem = PickByWeight(statTable.loots);
            RandomManager.bonusWeaponChance += weaponChance * 0.1f;
            RandomManager.bonusStatChance = 0;
            RandomManager.bonusResourceChance += resourceChance * 0.1f;
        }else{
            droppedItem = PickByWeight(resourceTable.loots);
            RandomManager.bonusWeaponChance += weaponChance * 10.1f;
            RandomManager.bonusStatChance += statChance * 0.1f;
            RandomManager.bonusResourceChance = 0;
        }
        GameObject droppedObject = Instantiate(droppedItem.LootPrefab, new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
        if (isWeapon) 
        {
            AttributeManager attributManager = droppedObject.GetComponent<AttributeManager>();

            if (attributManager != null)
            {
                attributManager.doesSpawnRarityParticles = true;
            }
        }
        
        int randomNutrientValue = UnityEngine.Random.Range(nutrientMin, nutrientMax);
        if (GlobalData.currentLoop >= 2)
        {
            randomNutrientValue = (randomNutrientValue * (GlobalData.currentLoop / 2));
        }
        //GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(randomNutrientValue);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", randomNutrientValue, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
    }

    private Loot PickByWeight(List<Loot> loots){
        float totalChance = 0;
        foreach(Loot currLoot in loots){
            totalChance += currLoot.DropChance;
            //Debug.Log(currLoot + ":  " + currLoot.DropChance);
        }
        float randomValue = UnityEngine.Random.Range(0f, totalChance);
        foreach(Loot currLoot in loots){
            randomValue -= currLoot.DropChance;
            if(randomValue <= 0){
                return currLoot;
            }
        }
        return loots[UnityEngine.Random.Range(0, loots.Count)];
    }
}