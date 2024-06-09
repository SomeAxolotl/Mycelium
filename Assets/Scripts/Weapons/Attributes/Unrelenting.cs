using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unrelenting : AttributeBase
{
    private bool canTriggerOnHit = true;
    private Skill relentlessFurySkill;
    private GameObject relentlessFurySkillInstance;
    private Transform skillLoadout;
    private GameObject relentlessFurySkillPrefab;
    private SkillManager skillManager;

    public override void Initialize()
    {
        attName = "Unrelenting";
        attDesc = "Release your Relentless Fury with a 15% chance when hitting an enemy.";
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

        // Find the Relentless Fury skill prefab in the SkillManager's skillList
        relentlessFurySkillPrefab = skillManager.skillList.Find(skill => skill.name == "RelentlessFury");

        if (relentlessFurySkillPrefab != null)
        {
            // Instantiate the Relentless Fury skill directly under SkillLoadout
            relentlessFurySkillInstance = Instantiate(relentlessFurySkillPrefab, skillLoadout);
            relentlessFurySkillInstance.name = "Relentless Fury";
            relentlessFurySkill = relentlessFurySkillInstance.GetComponent<Skill>();
        }
        else
        {
            Debug.LogError("Relentless Fury skill prefab not found.");
        }

        if (relentlessFurySkill == null)
        {
            Debug.LogError("Relentless Fury skill not found.");
        }
    }

    public override void Unequipped()
    {
        base.Unequipped();

        // Destroy the Relentless Fury skill instance when unequipped
        if (relentlessFurySkillInstance != null)
        {
            Destroy(relentlessFurySkillInstance);
        }
    }

    public override void Hit(GameObject target, float damage)
    {
        if (canTriggerOnHit && relentlessFurySkill != null && relentlessFurySkill.canSkill)
        {
            // Check if a random value between 0 and 1 is less than or equal to 0.15 (15% chance)
            if (Random.value <= 0.15f)
            {
                ActivateRelentlessFurySkill();
            }
        }
    }

    private void ActivateRelentlessFurySkill()
    {
        if (relentlessFurySkill == null)
        {
            Debug.LogError("Relentless Fury skill is null in ActivateRelentlessFurySkill.");
            return;
        }

        canTriggerOnHit = false;

        // Call the DoSkill method on the relentlessFurySkill instance
        relentlessFurySkill.DoSkill();
        relentlessFurySkill.StartCooldown(relentlessFurySkill.GetFinalCooldown());

        StartCoroutine(ResetSkillUsage());
    }

    private IEnumerator ResetSkillUsage()
    {
        yield return new WaitForSeconds(relentlessFurySkill.GetFinalCooldown());
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
