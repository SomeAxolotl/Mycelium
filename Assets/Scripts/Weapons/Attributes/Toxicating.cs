using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxicating : AttributeBase
{
    private bool canTriggerOnHit = true;
    private Skill mycotoxinsSkill;
    private GameObject mycotoxinsSkillInstance;
    private Transform skillLoadout;
    private GameObject mycotoxinsSkillPrefab;
    private SkillManager skillManager;

    public override void Initialize()
    {
        attName = "Toxicating";
        attDesc = "Gain Mycotixins with a 15% chance when hitting an enemy.";
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

        // Find the Mycotoxins skill prefab in the SkillManager's skillList
        mycotoxinsSkillPrefab = skillManager.skillList.Find(skill => skill.name == "Mycotoxins");

        if (mycotoxinsSkillPrefab != null)
        {
            // Instantiate the Mycotoxins skill directly under SkillLoadout
            mycotoxinsSkillInstance = Instantiate(mycotoxinsSkillPrefab, skillLoadout);
            mycotoxinsSkillInstance.name = "Mycotoxins";
            mycotoxinsSkill = mycotoxinsSkillInstance.GetComponent<Skill>();
        }
        else
        {
            Debug.LogError("Mycotoxins skill prefab not found.");
        }

        if (mycotoxinsSkill == null)
        {
            Debug.LogError("Mycootxins skill not found.");
        }
    }

    public override void Unequipped()
    {
        base.Unequipped();

        // Destroy the Myocotoxins skill instance when unequipped
        if (mycotoxinsSkillInstance != null)
        {
            Destroy(mycotoxinsSkillInstance);
        }
    }

    public override void Hit(GameObject target, float damage)
    {
        if (canTriggerOnHit && mycotoxinsSkill != null && mycotoxinsSkill.canSkill)
        {
            // Check if a random value between 0 and 1 is less than or equal to 0.15 (15% chance)
            if (Random.value <= 0.15f)
            {
                ActivateMycotoxinsSkill();
            }
        }
    }

    private void ActivateMycotoxinsSkill()
    {
        if (mycotoxinsSkill == null)
        {
            Debug.LogError("Mycotoxins skill is null in ActivateMycotoxinsSkill.");
            return;
        }

        canTriggerOnHit = false;

        // Call the DoSkill method on the mycotoxinsSkill instance
        mycotoxinsSkill.DoSkill();
        mycotoxinsSkill.StartCooldown(mycotoxinsSkill.GetFinalCooldown());

        StartCoroutine(ResetSkillUsage());
    }

    private IEnumerator ResetSkillUsage()
    {
        yield return new WaitForSeconds(mycotoxinsSkill.GetFinalCooldown());
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
