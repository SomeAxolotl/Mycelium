using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cascading : AttributeBase
{
    private bool canTriggerOnHit = true;
    private Skill trophicCascadeSkill;
    private GameObject trophicCascadeSkillInstance;
    private Transform skillLoadout;
    private GameObject trophicCascadeSkillPrefab;
    private SkillManager skillManager;

    public override void Initialize()
    {
        attName = "Cascading";
        attDesc = "Unleash Trophic Cascade with a 15% chance when hitting an enemy.";
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

        // Find the Trophic Cascade skill prefab in the SkillManager's skillList
        trophicCascadeSkillPrefab = skillManager.skillList.Find(skill => skill.name == "TrophicCascade");

        if (trophicCascadeSkillPrefab != null)
        {
            // Instantiate the Trophic Cascade skill directly under SkillLoadout
            trophicCascadeSkillInstance = Instantiate(trophicCascadeSkillPrefab, skillLoadout);
            trophicCascadeSkillInstance.name = "TrophicCascade";
            trophicCascadeSkill = trophicCascadeSkillInstance.GetComponent<Skill>();
        }
        else
        {
            Debug.LogError("Trophic Cascade skill prefab not found.");
        }

        if (trophicCascadeSkill == null)
        {
            Debug.LogError("Trophic Cascade skill not found.");
        }
    }

    public override void Unequipped()
    {
        base.Unequipped();

        // Destroy the Trophic Cascade skill instance when unequipped
        if (trophicCascadeSkillInstance != null)
        {
            Destroy(trophicCascadeSkillInstance);
        }
    }

    public override void Hit(GameObject target, float damage)
    {
        if (canTriggerOnHit && trophicCascadeSkill != null && trophicCascadeSkill.canSkill)
        {
            // Check if a random value between 0 and 1 is less than or equal to 0.15 (15% chance)
            if (Random.value <= 0.15f)
            {
                ActivateTrophicCascadeSkill();
            }
        }
    }

    private void ActivateTrophicCascadeSkill()
    {
        if (trophicCascadeSkill == null)
        {
            Debug.LogError("Trophic Cascade skill is null in ActivateTrophicCascadeSkill.");
            return;
        }

        canTriggerOnHit = false;

        // Call the DoSkill method on the trophicCascadeSkill instance
        trophicCascadeSkill.DoSkill();
        trophicCascadeSkill.StartCooldown(trophicCascadeSkill.GetFinalCooldown());

        StartCoroutine(ResetSkillUsage());
    }

    private IEnumerator ResetSkillUsage()
    {
        yield return new WaitForSeconds(trophicCascadeSkill.GetFinalCooldown());
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
