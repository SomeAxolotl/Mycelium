using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RonaldSunglassesEmoji.Interaction;

public class SporeInteraction : MonoBehaviour, IInteractable
{
    CharacterStats characterStats;
    DesignTracker designTracker;
    string coloredSporeName = "Gob";
    private int salvagedDeposits = 0;
    private const int totalDepositsNeeded = 1;
    private int salvagedWeapons = 0;
    private const int totalWeaponsNeeded = 1;
    NutrientTracker nutrientTracker;
    private bool depositGoalReached = false; // Flag to track if the deposit goal has been reached
    private bool weaponGoalReached = false;
    public GameObject curWeapon;
    private HUDItem hudItem;
    SwapWeapon swapWeapon;

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        designTracker = GetComponent<DesignTracker>();
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        hudItem = GameObject.Find("HUD").GetComponent<HUDItem>();
    }

    public void Interact(GameObject interactObject)
    {
        if (interactObject.CompareTag("GigaShroom")) // Check if interacting with Giga Shroom
        {
            // Special case interaction with Giga Shroom
            HandleGigaShroomInteraction();
        }
        else if (interactObject.CompareTag("ForgeShroom")) // Check if interacting with Forge Shroom
        {
            // Special case interaction with Forge Shroom
            HandleForgeShroomInteraction();
        }
        else
        {
            // Regular interaction behavior
            if (!interactObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sprout"))
            {
                SwapCharacter swapCharacter = GameObject.Find("PlayerParent").GetComponent<SwapCharacter>();
                int characterIndex = swapCharacter.GetCharacterIndex(interactObject);
                swapCharacter.SwitchCharacterGrowMenu(characterIndex);
                DestroyTooltip(gameObject);
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
        GameObject currentWeapon = GameObject.FindWithTag("currentWeapon");
        if (currentWeapon != null)
        {
            //RemoveHeldWeapon();
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
        if (GameObject.FindWithTag("currentWeapon") != null)
        {
            // Instantiate a clone of the current weapon
            GameObject currentWeapon = GameObject.FindWithTag("currentWeapon");
            GameObject clonedWeapon = Instantiate(currentWeapon, currentWeapon.transform.position, currentWeapon.transform.rotation);
            clonedWeapon.name = currentWeapon.name + "_Clone"; // Rename the clone to differentiate it
            curWeapon = clonedWeapon;

            // Deactivate the original weapon
            currentWeapon.SetActive(false);

            // Set the clone as the current weapon
            swapWeapon.curWeapon = clonedWeapon;
        }
        else
        {
            Debug.Log("No weapon held by the player.");
        }

        // Instantiate the starting weapon
        
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
        // Provide reward to the player for salvaging all weapons
        Debug.Log("All weapons salvaged! Providing reward...");
        // Add your reward logic here
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
            return; // Exit early to avoid creating the spore tooltip
        }
        if (interactObject.CompareTag("ForgeShroom"))
        {
            // Handle the Giga Shroom tooltip separately
            CreateForgeShroomTooltip();
            return; // Exit early to avoid creating the spore tooltip
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
