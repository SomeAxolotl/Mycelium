using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erupting : AttributeBase
{
    private bool canTriggerOnHit = true;
    private Skill eruptionSkill;
    private GameObject eruptionSkillInstance;
    private Transform skillLoadout;
    private GameObject eruptionSkillPrefab;
    private SkillManager skillManager;

    public override void Initialize()
    {
        attName = "Erupting";
        attDesc = "Unleash an eruption with a 15% chance when hitting an enemy.";
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

        // Find the Eruption skill prefab in the SkillManager's skillList
        eruptionSkillPrefab = skillManager.skillList.Find(skill => skill.name == "Eruption");

        if (eruptionSkillPrefab != null)
        {
            // Instantiate the Eruption skill directly under SkillLoadout
            eruptionSkillInstance = Instantiate(eruptionSkillPrefab, skillLoadout);
            eruptionSkillInstance.name = "Eruption";
            eruptionSkill = eruptionSkillInstance.GetComponent<Skill>();
        }
        else
        {
            Debug.LogError("Eruption skill prefab not found.");
        }

        if (eruptionSkill == null)
        {
            Debug.LogError("Eruption skill not found.");
        }
    }

    public override void Unequipped()
    {
        base.Unequipped();

        // Destroy the Eruption skill instance when unequipped
        if (eruptionSkillInstance != null)
        {
            Destroy(eruptionSkillInstance);
        }
    }

    public override void Hit(GameObject target, float damage)
    {
        if (canTriggerOnHit && eruptionSkill != null && eruptionSkill.canSkill)
        {
            // Check if a random value between 0 and 1 is less than or equal to 0.15 (15% chance)
            if (Random.value <= 0.15f)
            {
                ActivateEruptionSkill();
            }
        }
    }

    private void ActivateEruptionSkill()
    {
        if (eruptionSkill == null)
        {
            Debug.LogError("Eruption skill is null in ActivateEruptionSkill.");
            return;
        }

        canTriggerOnHit = false;

        // Call the DoSkill method on the eruptionSkill instance
        eruptionSkill.DoSkill();
        eruptionSkill.StartCooldown(eruptionSkill.GetFinalCooldown());

        StartCoroutine(ResetSkillUsage());
    }

    private IEnumerator ResetSkillUsage()
    {
        yield return new WaitForSeconds(eruptionSkill.GetFinalCooldown());
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

