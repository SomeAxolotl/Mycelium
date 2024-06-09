using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undergrown : AttributeBase
{
    private bool canTriggerOnHit = true;
    private Skill undergrowthSkill;
    private GameObject undergrowthSkillInstance;
    private Transform skillLoadout;
    private GameObject undergrowthSkillPrefab;
    private SkillManager skillManager;

    public override void Initialize()
    {
        attName = "Undergrown";
        attDesc = "Entangle enemies in Undergrowth with a 15% chance when hitting an enemy.";
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

        // Find the Undergrowth skill prefab in the SkillManager's skillList
        undergrowthSkillPrefab = skillManager.skillList.Find(skill => skill.name == "Undergrowth");

        if (undergrowthSkillPrefab != null)
        {
            // Instantiate the Undergrowth skill directly under SkillLoadout
            undergrowthSkillInstance = Instantiate(undergrowthSkillPrefab, skillLoadout);
            undergrowthSkillInstance.name = "Undergrowth";
            undergrowthSkill = undergrowthSkillInstance.GetComponent<Skill>();
        }
        else
        {
            Debug.LogError("Undergrowth skill prefab not found.");
        }

        if (undergrowthSkill == null)
        {
            Debug.LogError("Undergrowth skill not found.");
        }
    }

    public override void Unequipped()
    {
        base.Unequipped();

        // Destroy the Undergrowth skill instance when unequipped
        if (undergrowthSkillInstance != null)
        {
            Destroy(undergrowthSkillInstance);
        }
    }

    public override void Hit(GameObject target, float damage)
    {
        if (canTriggerOnHit && undergrowthSkill != null && undergrowthSkill.canSkill)
        {
            // Check if a random value between 0 and 1 is less than or equal to 0.15 (15% chance)
            if (Random.value <= 0.15f)
            {
                ActivateUndergrowthSkill();
            }
        }
    }

    private void ActivateUndergrowthSkill()
    {
        if (undergrowthSkill == null)
        {
            Debug.LogError("Undergrowth skill is null in ActivateUndergrowthSkill.");
            return;
        }

        canTriggerOnHit = false;

        // Call the DoSkill method on the undergrowthSkill instance
        undergrowthSkill.DoSkill();
        undergrowthSkill.StartCooldown(undergrowthSkill.GetFinalCooldown());

        StartCoroutine(ResetSkillUsage());
    }

    private IEnumerator ResetSkillUsage()
    {
        yield return new WaitForSeconds(undergrowthSkill.GetFinalCooldown());
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
