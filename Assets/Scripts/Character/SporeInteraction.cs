using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RonaldSunglassesEmoji.Interaction;
using System.Linq;

public class SporeInteraction : MonoBehaviour, IInteractable
{
    CharacterStats characterStats;
    DesignTracker designTracker;
    string coloredSporeName = "Gob";
    private int salvagedDeposits = 0;
    private const int totalDepositsNeeded = 1;
    private int salvagedWeapons = 0;
    private const int totalWeaponsNeeded = 5;
    NutrientTracker nutrientTracker;
    private bool depositGoalReached = false; // Flag to track if the deposit goal has been reached
    private bool weaponGoalReached = false;
    private HUDItem hudItem;
    [HideInInspector] public SwapWeapon swap;
    [HideInInspector] public GameObject player;
    [HideInInspector] public GameObject playerParent;
    private GameObject currentWeapon;
    // Lists to track the rarities of salvaged weapons
    private List<AttributeAssigner.Rarity> salvagedWeaponRarities = new List<AttributeAssigner.Rarity>();
    private bool isInteractingWithShroom = false; // New flag to track interaction with Giga or Forge Shroom
    private bool isForgeShroomTooltipVisible = false;

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        designTracker = GetComponent<DesignTracker>();
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        hudItem = GameObject.Find("HUD").GetComponent<HUDItem>();
        player = GameObject.FindWithTag("currentPlayer");
        currentWeapon = GameObject.FindWithTag("currentWeapon");
    }

    public void Interact(GameObject interactObject)
    {
        if (interactObject.CompareTag("GigaShroom")) // Check if interacting with Giga Shroom
        {
            isInteractingWithShroom = true;
            // Special case interaction with Giga Shroom
            HandleGigaShroomInteraction();
        }
        else if (interactObject.CompareTag("ForgeShroom")) // Check if interacting with Forge Shroom
        {
            isInteractingWithShroom = true;
            // Special case interaction with Forge Shroom
            HandleForgeShroomInteraction();
        }
        else
        {
            isInteractingWithShroom = false;
            // Regular interaction behavior
            if (!interactObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sprout"))
            {
                if (isInteractingWithShroom) // Check if not interacting with Giga or Forge Shroom
                {
                    SwapCharacter swapCharacter = GameObject.Find("PlayerParent").GetComponent<SwapCharacter>();
                    int characterIndex = swapCharacter.GetCharacterIndex(interactObject);
                    swapCharacter.SwitchCharacterGrowMenu(characterIndex);
                    DestroyTooltip(gameObject);
                }
            }
        }
    }
    private void HandleGigaShroomInteraction()
    {
        // Check if the player is holding a deposit
        if (nutrientTracker.heldItem != null)
        {
            // Transfer the deposit to the Giga Shroom
            string heldDepositName = nutrientTracker.heldItem.name;
            salvagedDeposits++;
            Debug.Log("Deposited " + heldDepositName + " to the Giga Shroom!");

            // Clear the held deposit
            RemoveHeldDeposit();

            // Check if all deposits have been salvaged
            if (salvagedDeposits >= totalDepositsNeeded)
            {
                // Set the deposit goal reached flag
                depositGoalReached = true;
                // Provide reward to the player
                ProvideReward();
            }
        }
        else
        {
            Debug.Log("No deposit held by the player.");
        }

        CreateGigaShroomTooltip();
        // Get the colored hint string for the interact button
        
    }
    private void HandleForgeShroomInteraction()
    {
        if (currentWeapon != null)
        {
            AttributeBase weaponAttribute = currentWeapon.GetComponent<AttributeBase>();
            if (weaponAttribute != null)
            {
                salvagedWeaponRarities.Add(weaponAttribute.rating);
            }
            RemoveHeldWeapon();
            salvagedWeapons++;
            Debug.Log("Deposited " + currentWeapon.name + " to the Forge Shroom!");

            // Check if all weapons have been salvaged
            if (salvagedWeapons >= totalWeaponsNeeded)
            {
                // Set the weapon goal reached flag
                weaponGoalReached = true;
                // Provide reward to the player
                ProvideWeaponReward();
            }
        }
        else
        {
            Debug.Log("No weapon held by the player.");
        }
            CreateForgeShroomTooltip();
        isForgeShroomTooltipVisible = true; // Forge Shroom tooltip is now visible
    }
    private void RemoveHeldDeposit()
    {
        if (nutrientTracker.heldItem != null)
        {
            string heldItemName = nutrientTracker.heldItem.name;

            // Deactivate the held item
            nutrientTracker.heldItem.SetActive(false);
            nutrientTracker.heldItem = null;

            // Update the HUD to remove the held item
            nutrientTracker.LoseMaterials();
            hudItem.LostItem();
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
            AttributeAssigner.Instance.AssignCommonAttribute(randomWeapon); // Ensure the weapon is always common rarity
            randomWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            randomWeapon.GetComponent<Collider>().enabled = false;
            gameObject.tag = "Weapon";
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
            var enigmaticComponent = randomWeapon.GetComponent<Enigmatic>();
            if (enigmaticComponent != null)
            {
                enigmaticComponent.Equipped();
            }
        }
    }
    private string RandomWeapon()
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

    private void CreateGigaShroomTooltip()
    {
        if (depositGoalReached)
        {
            // If the deposit goal has been reached, do not show the tooltip
            return;
        }
        // Get the colored hint string for the interact button
        string interactText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();

        // Create the tooltip message
        string tooltipTitle = "Giga Shroom";
        string tooltipMessage = "Deposits needed: " + salvagedDeposits + "/" + totalDepositsNeeded + "\n";
        tooltipMessage += "Provide deposits to the Giga Shroom.\n";
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
    private void CreateForgeShroomTooltip()
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
    private void ProvideReward()
    {
        // Provide reward to the player for salvaging all deposits
        Debug.Log("All deposits salvaged! Providing reward...");
        // Add your reward logic here
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
        GameObject rewardWeapon = Instantiate(Resources.Load(RandomWeapon()), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
        if (rewardWeapon.GetComponent<WeaponStats>().wpnName != "Stick")
        {
            rewardWeapon.transform.localScale = rewardWeapon.transform.localScale / 0.03f / 100f / 1.2563f;
        }
        rewardWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
        AttributeAssigner.Instance.AssignAttributeOfRarity(rewardWeapon, rewardRarity);
        rewardWeapon.layer = LayerMask.NameToLayer("currentWeapon");
        rewardWeapon.GetComponent<Collider>().enabled = false;
        gameObject.tag = "Weapon";
        rewardWeapon.tag = "currentWeapon";
        StartCoroutine(SetPreviousWeaponStats(rewardWeapon));

        Debug.Log("All weapons salvaged! Providing reward of rarity: " + rewardRarity);
    }
    public void Salvage(GameObject interactObject)
    {
        //buh
    }

    public void CreateTooltip(GameObject interactObject)
    {
        if (interactObject.CompareTag("GigaShroom"))
        {
            // Handle the Giga Shroom tooltip separately
            CreateGigaShroomTooltip();
             // Exit early to avoid creating the spore tooltip
        }
        if (interactObject.CompareTag("ForgeShroom") && salvagedWeapons != 5)
        {
            // Handle the Giga Shroom tooltip separately
            CreateForgeShroomTooltip();
             // Exit early to avoid creating the spore tooltip
        }
        Color bodyColor = designTracker.bodyColor;
        //coloredSporeName = "<color=#" + ColorUtility.ToHtmlStringRGB(bodyColor) + ">"+characterStats.sporeName+"</color>";

        coloredSporeName = characterStats.GetColoredSporeName();

        string buttonText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        int subspeciesSkillIconIndex = 8;
        switch (characterStats.equippedSkills[0])
        {
            case "FungalMight":
                subspeciesSkillIconIndex = 8;
                break;
            case "DeathBlossom":
                subspeciesSkillIconIndex = 9;
                break;
            case "FairyRing":
                subspeciesSkillIconIndex = 10;
                break;
            case "Zombify":
                subspeciesSkillIconIndex = 11;
                break;
        }
        
        Tooltip sporeTooltip = TooltipManager.Instance.CreateTooltip
        (
            gameObject, 
            "<sprite=" + subspeciesSkillIconIndex + ">  " + coloredSporeName + "  <sprite=" + subspeciesSkillIconIndex + ">", 
            "<sprite=0> " + characterStats.primalLevel + 
            "   <sprite=1> " + characterStats.speedLevel + 
            "   <sprite=2> " + characterStats.sentienceLevel + 
            "   <sprite=3> " + characterStats.vitalityLevel, 
            "Press " + buttonText + " to Swap",
            "",
            true,
            1f,
            false
        );
        if (sporeTooltip != null)
        {  
            sporeTooltip.ShowHappiness(characterStats.sporeHappiness);
        }
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        TooltipManager.Instance.DestroyTooltip();
    }
}
