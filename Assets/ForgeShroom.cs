using RonaldSunglassesEmoji.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RonaldSunglassesEmoji.Interaction;
using System.Linq;
//using static UnityEditor.Timeline.Actions.MenuPriority;

public class ForgeShroom : MonoBehaviour, IInteractable
{  
    DesignTracker designTracker;
    private int salvagedWeapons = 0;
    private const int totalWeaponsNeeded = 3;
    private bool weaponGoalReached = false;
   
    [HideInInspector] public SwapWeapon swap;
    [HideInInspector] public GameObject player;
    [HideInInspector] public GameObject playerParent;
    private GameObject currentWeapon;
    // Lists to track the rarities of salvaged weapons
    private List<AttributeAssigner.Rarity> salvagedWeaponRarities = new List<AttributeAssigner.Rarity>();

    void Start()
    {
   
        designTracker = GetComponent<DesignTracker>();
        swap = GameObject.Find("PlayerParent").GetComponent<SwapWeapon>();
        player = GameObject.FindWithTag("currentPlayer");
        
    }
    public void Interact(GameObject interactObject)
    {
        if (weaponGoalReached)
        {
            DestroyTooltip(interactObject);
            return;
        }

        // Special case interaction with Forge Shroom
        HandleForgeShroomInteraction();
            if (currentWeapon == null)
            {
                Debug.Log("Weapon error");
            }
        
    }
    private void HandleForgeShroomInteraction()
    {
        currentWeapon = GameObject.FindWithTag("currentWeapon");
        if (currentWeapon != null && currentWeapon.GetComponent<WeaponStats>().wpnName == "Stick")
        {
            Debug.Log(currentWeapon.name);
            return;
        }

        if (currentWeapon != null)
        {
            AttributeBase weaponAttribute = currentWeapon.GetComponent<AttributeBase>();
            if (weaponAttribute != null)
            {
                salvagedWeaponRarities.Add(weaponAttribute.rating);
            }
            
            salvagedWeapons++;
            Debug.Log("Deposited " + currentWeapon.name + " to the Forge Shroom!");

            // Check if all weapons have been salvaged
            if (salvagedWeapons < totalWeaponsNeeded)
            {
                RemoveHeldWeapon();
            }
            else if (salvagedWeapons == totalWeaponsNeeded)
            {
                weaponGoalReached = true;
                ProvideWeaponReward();
            }

        }
        else
        {
            Debug.Log("No weapon held by the player.");
        }
             
    }
    private void RemoveHeldWeapon()
    {
        // Check if the player is holding a weapon
        if (currentWeapon != null)
        {
            playerParent = player.transform.parent.gameObject;
            swap = playerParent.GetComponent<SwapWeapon>();

            // Instantiate a new weapon of common rarity
            GameObject randomWeapon = Instantiate(Resources.Load(RandomWeapon()), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            if (randomWeapon.GetComponent<WeaponStats>().wpnName != "Stick")
            {
                randomWeapon.transform.localScale = randomWeapon.transform.localScale / 0.03f / 100f / 1.2563f;
            }
            randomWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
            //AttributeAssigner.Instance.AssignCommonAttribute(randomWeapon); // Ensure the weapon is always common rarity
            randomWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            randomWeapon.GetComponent<Collider>().enabled = false;
            currentWeapon.gameObject.tag = "Weapon";
            randomWeapon.tag = "currentWeapon";
            StartCoroutine(SetPreviousWeaponStats(randomWeapon));
        }
        else
        {
            Debug.Log("No weapon held by the player.");
        }
    }
    IEnumerator SetPreviousWeaponStats(GameObject randomWeapon)
    {
        yield return 0; // Wait for one frame to ensure the object is properly initialized
        Destroy(currentWeapon);
        if (randomWeapon != null)
        {
            var weaponInteraction = randomWeapon.GetComponent<WeaponInteraction>();
            if (weaponInteraction != null)
            {
                weaponInteraction.ApplyWeaponPositionAndRotation();
            }
        }
    }
    IEnumerator SetRewardWeaponStats(GameObject rewardWeapon)
    {
        yield return 0; // Wait for one frame to ensure the object is properly initialized
        Destroy(currentWeapon);
        if (rewardWeapon != null)
        {
            var weaponInteraction = rewardWeapon.GetComponent<WeaponInteraction>();
            if (weaponInteraction != null)
            {
                weaponInteraction.ApplyWeaponPositionAndRotation();
            }
        }
    }
    private string RandomWeapon()
    {
        
                return "Slash/Stick";
        
    }
    private string RandomRewardWeapon()
    {
        switch (Random.Range(0, 8))
        {
            case 0:
                return "Slash/AvocadoFlamberge";
            case 1:
                return "Slash/ObsidianScimitar";
            case 2:
                return "Slash/MandibleSickle";
            case 3:
                return "Smash/RoseMace";
            case 4:
                return "Smash/GeodeHammer";
            case 5:
                return "Smash/FemurClub";
            case 6:
                return "Stab/BambooPartisan";
            case 7:
                return "Stab/OpalRapier";
            case 8:
                return "Stab/CarpalSais";
            default:
                return "Slash/Stick";
        }
    }
    public void Salvage(GameObject interactObject)
    {
        //adw
    }
    public void CreateTooltip(GameObject interactObject)
    {
        if (weaponGoalReached)
        {
            // If the weapon goal has been reached, do not show the tooltip
            return;
        }

        // Get the colored hint string for the interact button
        string interactText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();

        // Create the tooltip message
        string tooltipTitle = "Forge Shroom";
        string tooltipMessage = "Weapons needed: " + salvagedWeapons + "/" + totalWeaponsNeeded + "\n";
        tooltipMessage += "Provide weapons to the Forge Shroom.\n";
        tooltipMessage += "Press " + interactText + " to provide.";

        // Call CreateTooltip with all required parameters
        TooltipManager.Instance.CreateTooltip(
            gameObject,
            tooltipTitle,
            tooltipMessage,
            "", // interactString
            "", // salvageString
            true, // showBackground
            1f, // tooltipDuration
            false, // persist
            null // rarity
        );
    }
    private void ProvideWeaponReward()
    {
        // Calculate the probabilities based on the provided rarities
        float commonCount = salvagedWeaponRarities.Count(r => r == AttributeAssigner.Rarity.Common);
        float rareCount = salvagedWeaponRarities.Count(r => r == AttributeAssigner.Rarity.Rare);
        float legendaryCount = salvagedWeaponRarities.Count(r => r == AttributeAssigner.Rarity.Legendary);

        float totalWeapons = salvagedWeaponRarities.Count;
        float commonChance = (commonCount / totalWeapons) * 10f; // 10%
        float rareChance = (rareCount / totalWeapons) * 20f; // 20%
        float legendaryChance = (legendaryCount / totalWeapons) * 50f; // 50%

        // Normalize the chances
        float totalChance = commonChance + rareChance + legendaryChance;
        commonChance /= totalChance;
        rareChance /= totalChance;
        legendaryChance /= totalChance;

        // Determine the rarity of the reward
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        AttributeAssigner.Rarity rewardRarity;
        if (randomValue <= commonChance)
        {
            rewardRarity = AttributeAssigner.Rarity.Common;
        }
        else if (randomValue - commonChance <= rareChance)
        {
            rewardRarity = AttributeAssigner.Rarity.Rare;
        }
        else
        {
            rewardRarity = AttributeAssigner.Rarity.Legendary;
        }

        // Instantiate a new weapon of the determined rarity
        GameObject rewardWeapon = Instantiate(Resources.Load(RandomRewardWeapon()), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
        if (rewardWeapon.GetComponent<WeaponStats>().wpnName != "Stick")
        {
            rewardWeapon.transform.localScale = rewardWeapon.transform.localScale / 0.03f / 100f / 1.2563f;
        }
        rewardWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
        AttributeAssigner.Instance.AssignAttributeOfRarity(rewardWeapon, rewardRarity);
        rewardWeapon.layer = LayerMask.NameToLayer("currentWeapon");
        rewardWeapon.GetComponent<Collider>().enabled = false;
        currentWeapon.gameObject.tag = "Weapon";
        rewardWeapon.tag = "currentWeapon";
        StartCoroutine(SetRewardWeaponStats(rewardWeapon));

        Debug.Log("All weapons salvaged! Providing reward of rarity: " + rewardRarity);
    }
    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        TooltipManager.Instance.DestroyTooltip();
    }
}
