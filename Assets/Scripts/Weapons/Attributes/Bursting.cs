using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bursting : AttributeBase
{
    private bool canTriggerOnHit = true;
    private Skill sporeburstSkill;
    private GameObject sporeburstSkillInstance;
    private Transform skillLoadout;
    private GameObject sporeburstSkillPrefab;
    private SkillManager skillManager;

    public override void Initialize()
    {
        attName = "Bursting";
        attDesc = "Unleash Sporeburst with a 15% chance when hitting an enemy.";
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

        // Find the Sporeburst skill prefab in the SkillManager's skillList
        sporeburstSkillPrefab = skillManager.skillList.Find(skill => skill.name == "Sporeburst");

        if (sporeburstSkillPrefab != null)
        {
            // Instantiate the Sporeburst skill directly under SkillLoadout
            sporeburstSkillInstance = Instantiate(sporeburstSkillPrefab, skillLoadout);
            sporeburstSkillInstance.name = "Sporeburst";
            sporeburstSkill = sporeburstSkillInstance.GetComponent<Skill>();
        }
        else
        {
            Debug.LogError("Sporeburst skill prefab not found.");
        }

        if (sporeburstSkill == null)
        {
            Debug.LogError("Sporeburst skill not found.");
        }
    }

    public override void Unequipped()
    {
        base.Unequipped();

        // Destroy the Sporeburst skill instance when unequipped
        if (sporeburstSkillInstance != null)
        {
            Destroy(sporeburstSkillInstance);
        }
    }

    public override void Hit(GameObject target, float damage)
    {
        if (canTriggerOnHit && sporeburstSkill != null && sporeburstSkill.canSkill)
        {
            // Check if a random value between 0 and 1 is less than or equal to 0.15 (15% chance)
            if (Random.value <= 0.15f)
            {
                ActivateSporeburstSkill();
            }
        }
    }

    private void ActivateSporeburstSkill()
    {
        if (sporeburstSkill == null)
        {
            Debug.LogError("Sporeburst skill is null in ActivateSporeburstSkill.");
            return;
        }

        canTriggerOnHit = false;

        // Call the DoSkill method on the sporeburstSkill instance
        sporeburstSkill.DoSkill();
        sporeburstSkill.StartCooldown(sporeburstSkill.GetFinalCooldown());

        StartCoroutine(ResetSkillUsage());
    }

    private IEnumerator ResetSkillUsage()
    {
        yield return new WaitForSeconds(sporeburstSkill.GetFinalCooldown());
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
