using RonaldSunglassesEmoji.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    public GameObject baseWeaponPrefab; // Prefab of the base weapon
    private List<GameObject> depositedWeapons = new List<GameObject>();
    // Public GameObject for the cache location
    public GameObject cacheLocation;
    private Dictionary<string, string> weaponResourcePaths = new Dictionary<string, string>
    {
        { "AvocadoFlamberge", "Slash/AvocadoFlamberge" },
        { "ObsidianScimitar", "Slash/ObsidianScimitar" },
        { "MandibleSickle", "Slash/MandibleSickle" },
        { "RoseMace", "Smash/RoseMace" },
        { "GeodeHammer", "Smash/GeodeHammer" },
        { "FemurClub", "Smash/FemurClub" },
        { "BambooPartisan", "Stab/BambooPartisan" },
        { "OpalRapier", "Stab/OpalRapier" },
        { "CarpalSais", "Stab/CarpalSais" }
    };
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
        //Also return if it's a forged weapon (or no attribute)
        if (currentWeapon != null && (currentWeapon.GetComponent<AttributeManager>().GetHighestAttributeRarity() == AttributeAssigner.Rarity.Forged || currentWeapon.GetComponent<AttributeManager>().GetHighestAttributeRarity() == AttributeAssigner.Rarity.None))
        {
            return;
        }

        if (currentWeapon != null)
        {
            // Clean up the skill instance
            foreach (var attribute in currentWeapon.GetComponents<SkillAttributes>())
            {
                attribute.CleanUpSkillInstance();
            }
            salvagedWeapons++;
            Debug.Log("Deposited " + currentWeapon.name + " to the Forge Shroom!");

            if (salvagedWeapons == 1)
            {
                // Set the base weapon type for the reward
                SetBaseRewardWeapon(currentWeapon);
            }
            else if (salvagedWeapons == 2)
            {
                // Add the first attribute of the second weapon to the reward
                AddAttributeToRewardWeapon(currentWeapon, 0);
            }
            else if (salvagedWeapons == 3)
            {
                // Add the second attribute of the third weapon to the reward
                AddAttributeToRewardWeapon(currentWeapon, 0);
                weaponGoalReached = true;
                ProvideWeaponReward();
            }

            if (salvagedWeapons <= totalWeaponsNeeded)
            {
                ParentHeldWeapon();
                // Provide a stick weapon to the player
                GiveStickWeapon();
            }
        }
        else
        {
            Debug.Log("No weapon held by the player.");
        }
    }
    private void SetBaseRewardWeapon(GameObject weapon)
    {
        string weaponName = weapon.name.Replace("(Clone)", "").Trim();
        if (weaponResourcePaths.TryGetValue(weaponName, out string resourcePath))
        {
            baseWeaponPrefab = Resources.Load<GameObject>(resourcePath);

            if (baseWeaponPrefab == null)
            {
                Debug.LogError("Base weapon prefab not found in Resources: " + resourcePath);
            }
            else
            {
                depositedWeapons.Add(weapon);
                Debug.Log("Base weapon set: " + baseWeaponPrefab.name);
            }
        }
        else
        {
            Debug.LogError("Weapon name not found in resource paths: " + weaponName);
        }
    }
    private void AddAttributeToRewardWeapon(GameObject weapon, int attributeIndex)
    {
        // Get the attribute at the specified index
        AttributeBase[] attributes = weapon.GetComponents<AttributeBase>();
        if (attributes.Length > attributeIndex)
        {
            string attributeName = attributes[attributeIndex].GetType().Name;
            depositedWeapons.Add(weapon);
            Debug.Log("Attribute added: " + attributeName);
        }
    }
    
    IEnumerator SetPreviousWeaponStats(GameObject rewardWeapon)
    {
        yield return 0;
        Destroy(currentWeapon);
        if (rewardWeapon != null)
        {
            var weaponInteraction = rewardWeapon.GetComponent<WeaponInteraction>();
            if (weaponInteraction != null)
            {
                weaponInteraction.ApplyWeaponPositionAndRotation();
            }
        }

        // Swap the current weapon with the reward weapon
       /* if (playerParent != null && swap != null)
        {
            swap.SwapCurrentWeapon(rewardWeapon);
        }*/
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

        // Create the tooltip message
        string tooltipTitle = "Blacksmith Ironspore";
        string tooltipMessage = "Combine 3 Weapons into 1:\n";


        tooltipMessage += $"Base {(salvagedWeapons >= 1 ? 1 : 0)}/1\n";
        tooltipMessage += $"Attribute {(salvagedWeapons >= 2 ? 1 : 0)}/1\n";
        tooltipMessage += $"Attribute {(salvagedWeapons >= 3 ? 1 : 0)}/1\n";

        // Get the colored hint string for the interact button
        string interactText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        string interactTooltiptext = "Press " + interactText + " to provide weapon.";
        // Call CreateTooltip with all required parameters
        TooltipManager.Instance.CreateTooltip(
            gameObject,
            tooltipTitle,
            tooltipMessage,
            interactTooltiptext, // interactString
            "", // salvageString
            true, // showBackground
            1f, // tooltipDuration
            false, // persist
            null // rarity
        );
    }
    private void ParentHeldWeapon()
    {
        if (currentWeapon != null)
        {
            // Parent the current weapon to the ForgeShroom
            currentWeapon.transform.SetParent(this.transform);
            currentWeapon.tag = "Untagged"; // Remove the currentWeapon tag
            currentWeapon.SetActive(false); // Optionally deactivate the weapon
            currentWeapon = null; // Clear the currentWeapon reference
        }
        else
        {
            Debug.Log("No weapon held by the player.");
        }
    }
    private void GiveStickWeapon()
    {
        // Instantiate a stick weapon and give it to the player
        GameObject stickWeapon = Instantiate(Resources.Load("Slash/Stick"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
        stickWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
        stickWeapon.layer = LayerMask.NameToLayer("currentWeapon");
        stickWeapon.GetComponent<Collider>().enabled = false;
        stickWeapon.tag = "currentWeapon";
        StartCoroutine(SetStickWeaponStats(stickWeapon));
    }
    IEnumerator SetStickWeaponStats(GameObject stickWeapon)
    {
        yield return 0; // Wait for one frame to ensure the object is properly initialized
        if (stickWeapon != null)
        {
            var weaponInteraction = stickWeapon.GetComponent<WeaponInteraction>();
            if (weaponInteraction != null)
            {
                weaponInteraction.ApplyWeaponPositionAndRotation();
            }
        }
    }
    private void ProvideWeaponReward()
    {
        if (baseWeaponPrefab != null && cacheLocation != null)
        {
            GameObject rewardWeapon = Instantiate(baseWeaponPrefab, cacheLocation.transform.position, Quaternion.identity);

            // Ensure no extra attributes are present on instantiation
            AttributeBase[] existingAttributes = rewardWeapon.GetComponents<AttributeBase>();
            foreach (AttributeBase attribute in existingAttributes)
            {
                Destroy(attribute);
            }
            // Track added skill names to avoid duplicates
            HashSet<string> addedSkills = new HashSet<string>();
            // Add the collected attributes to the reward weapon
            if (depositedWeapons.Count > 1)
            {
                AddCollectedAttributeToWeapon(rewardWeapon, depositedWeapons[1], 0, addedSkills);
            }

            if (depositedWeapons.Count > 2)
            {
                AddCollectedAttributeToWeapon(rewardWeapon, depositedWeapons[2], 0, addedSkills);
            }

            ChangeAttributesToForged(rewardWeapon);

            rewardWeapon.GetComponent<WeaponStats>().acceptingAttribute = false; // Enable accepting new attributes if needed

            Debug.Log("Reward weapon instantiated at the cache location.");
        }
        else
        {
            Debug.LogError("Base weapon prefab or cache location is not set.");
        }
    }
    private void AddCollectedAttributeToWeapon(GameObject rewardWeapon, GameObject sourceWeapon, int attributeIndex, HashSet<string> addedSkills)
    {
        AttributeBase[] attributes = sourceWeapon.GetComponents<AttributeBase>();
        if (attributes.Length > attributeIndex)
        {
            string attributeName = attributes[attributeIndex].GetType().Name;

            // Check if the attribute is a skill attribute
            if (attributes[attributeIndex] is SkillAttributes skillAttribute)
            {
                // Check if the skill has already been added
                /*
                if (!addedSkills.Contains(skillAttribute.GetSkillName()))
                {
                    addedSkills.Add(skillAttribute.GetSkillName());
                    AttributeAssigner.Instance.PickAttFromString(rewardWeapon, attributeName);
                }
                else
                {
                    // If the skill already exists, update the existing skill to increase chance and description
                    var existingSkill = rewardWeapon.GetComponent(skillAttribute.GetType()) as SkillAttributes;
                    if (existingSkill != null)
                    {
                        existingSkill.UpdateChanceAndDescription();
                    }
                }
                */
                //Griffin change for skill management
                addedSkills.Add(skillAttribute.GetSkillName());
                AttributeAssigner.Instance.PickAttFromString(rewardWeapon, attributeName);
                //Griffin just moved this
            }
            else
            {
                AttributeAssigner.Instance.PickAttFromString(rewardWeapon, attributeName);
            }
        }
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        TooltipManager.Instance.DestroyTooltip();
    }

    void ChangeAttributesToForged(GameObject rewardWeapon)
    {
        AttributeBase[] attributes = rewardWeapon.GetComponents<AttributeBase>();
        foreach (AttributeBase attributeBase in attributes)
        {
            attributeBase.rating = AttributeAssigner.Rarity.Forged;
        }
    }
}
