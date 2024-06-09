using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cycloning : AttributeBase
{
    private bool canTriggerOnHit = true;
    private Skill livingCycloneSkill;
    private GameObject livingCycloneSkillInstance;
    private Transform skillLoadout;
    private GameObject livingCycloneSkillPrefab;
    private SkillManager skillManager;
    private Animator playerAnimator;
    private static readonly int InActionState = Animator.StringToHash("InActionState");

    public override void Initialize()
    {
        attName = "Cycloning";
        attDesc = "Living Cyclone with a 15% chance when hitting an enemy.";
    }

    public override void Equipped()
    {
        base.Equipped();

        // Find the SkillLoadout by name
        skillLoadout = FindSkillLoadout();
        if (skillLoadout == null)
        {
            Debug.LogError("SkillLoadout not found on the PlayerParent.");
            return;
        }

        // Get the SkillManager component
        skillManager = playerParent.GetComponent<SkillManager>();
        if (skillManager == null)
        {
            Debug.LogError("SkillManager not found on the PlayerParent.");
            return;
        }

        // Find the Living Cyclone skill prefab in the SkillManager's skillList
        livingCycloneSkillPrefab = skillManager.skillList.Find(skill => skill.name == "LivingCyclone");

        if (livingCycloneSkillPrefab != null)
        {
            // Instantiate the Living Cyclone skill directly under SkillLoadout
            livingCycloneSkillInstance = Instantiate(livingCycloneSkillPrefab, skillLoadout);
            livingCycloneSkillInstance.name = "Living Cyclone";
            livingCycloneSkill = livingCycloneSkillInstance.GetComponent<Skill>();
        }
        else
        {
            Debug.LogError("Living Cyclone skill prefab not found.");
        }

        if (livingCycloneSkill == null)
        {
            Debug.LogError("Living Cyclone skill not found.");
        }

        // Get the Animator component from the player or its children
        playerAnimator = playerParent.GetComponentInChildren<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("Animator not found on the PlayerParent.");
        }
    }

    public override void Unequipped()
    {
        base.Unequipped();

        // Destroy the Living Cyclone skill instance when unequipped
        if (livingCycloneSkillInstance != null)
        {
            Destroy(livingCycloneSkillInstance);
        }
    }

    public override void Hit(GameObject target, float damage)
    {
        if (canTriggerOnHit && livingCycloneSkill != null && livingCycloneSkill.canSkill)
        {
            StartCoroutine(ActivateLivingCycloneSkillWithDelay());
            // Check if a random value between 0 and 1 is less than or equal to 0.15 (15% chance)
            /*if (Random.value <= 0.15f)
            {
                StartCoroutine(ActivateLivingCycloneSkillWithDelay());
            }*/
        }
    }
    private IEnumerator ActivateLivingCycloneSkillWithDelay()
    {
        // Wait for the player to finish their attack animation
        yield return new WaitUntil(() => !IsPlayerInActionState());

        if (livingCycloneSkill == null)
        {
            Debug.LogError("Living Cyclone skill is null in ActivateLivingCycloneSkill.");
            yield break;
        }

        canTriggerOnHit = false;

        // Call the DoSkill method on the livingCycloneSkill instance
        livingCycloneSkill.DoSkill();
        livingCycloneSkill.StartCooldown(livingCycloneSkill.GetFinalCooldown());

        StartCoroutine(ResetSkillUsage());
    }
    private bool IsPlayerInActionState()
    {
        return playerAnimator != null && playerAnimator.GetBool(InActionState);
    }
    private IEnumerator ResetSkillUsage()
    {
        yield return new WaitForSeconds(livingCycloneSkill.GetFinalCooldown());
        canTriggerOnHit = true;
    }

    private Transform FindSkillLoadout()
    {
        Transform[] children = playerParent.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.name == "SkillLoadout")
            {
                return child;
            }
        }
        return null;
    }
}
