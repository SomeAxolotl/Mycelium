using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillAttributes : AttributeBase
{
    protected bool canTriggerOnHit = true;
    protected Skill skillInstance;
    protected GameObject skillPrefab;
    protected SkillManager skillManager;
    private Transform skillLoadout;
    private float triggerChance = 0.15f;
    protected abstract string SkillName { get; }
    protected abstract string AttributeName { get; }
    protected abstract string BaseAttributeDescription { get; }

    public string GetSkillName()
    {
        return SkillName;
    }
    public override void Initialize()
    {
        attName = AttributeName;
        attDesc = BaseAttributeDescription;
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

        // Find the skill prefab in the SkillManager's skillList
        skillPrefab = skillManager.skillList.Find(skill => skill.name == SkillName);

        if (skillPrefab != null)
        {
            // Instantiate the skill directly under SkillLoadout
            var skillObject = Instantiate(skillPrefab, skillLoadout);
            skillObject.name = SkillName;
            skillInstance = skillObject.GetComponent<Skill>();
        }
        else
        {
            Debug.LogError(SkillName + " skill prefab not found.");
        }

        if (skillInstance == null)
        {
            Debug.LogError(SkillName + " skill not found.");
        }
        else
        {
            // Check for multiple instances of the same skill and adjust trigger chance
            AttributeBase[] attributes = GetComponents<AttributeBase>();
            int sameSkillCount = 0;
            foreach (var attribute in attributes)
            {
                if (attribute.GetType().Name == this.GetType().Name)
                {
                    sameSkillCount++;
                }
            }
            if (sameSkillCount > 1)
            {
                UpdateChanceAndDescription();
            }
        }
    }
    public void UpdateChanceAndDescription()
    {
        attName = AttributeName + " " + AttributeName;
        triggerChance = 0.30f; // Increase chance to 30% if there are two instances of the same skill
        attDesc = BaseAttributeDescription.Replace("15%", "30%");
    }
    public override void Unequipped()
    {
        base.Unequipped();

        CleanUpSkillInstance();
    }

    public void CleanUpSkillInstance()
    {
        // Destroy the skill instance when unequipped
        if (skillInstance != null)
        {
            Destroy(skillInstance.gameObject);
            skillInstance = null;
        }
    }

    public override void Hit(GameObject target, float damage)
    {
        if (canTriggerOnHit && skillInstance != null && skillInstance.canSkill)
        {
            // Check if a random value between 0 and 1 is less than or equal to 0.15 (15% chance)
            if (Random.value <= triggerChance)
            {
                StartCoroutine(ActivateSkillWithDelay());
            }
        }
    }

    /*private void ActivateSkill()
    {
        if (skillInstance == null)
        {
            Debug.LogError(SkillName + " skill is null in ActivateSkill.");
            return;
        }

        canTriggerOnHit = false;

        // Call the DoSkill method on the skill instance
        skillInstance.DoSkill();
        //skillInstance.StartCooldown(skillInstance.GetFinalCooldown());

        StartCoroutine(ResetSkillUsage());
    }*/
    private IEnumerator ActivateSkillWithDelay()
    {
        Animator animator = playerParent.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            // Wait until the attack animation has finished
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"));

            // Ensure that the skill instance is still valid
            if (skillInstance != null)
            {
                canTriggerOnHit = false;

                // Call the DoSkill method on the skill instance
                skillInstance.DoSkill();
                //skillInstance.StartCooldown(skillInstance.GetFinalCooldown());

                StartCoroutine(ResetSkillUsage());
            }
        }
        else
        {
            Debug.LogError("Animator not found on the PlayerParent.");
        }
    }

    private IEnumerator ResetSkillUsage()
    {
        //yield return new WaitForSeconds(skillInstance.GetFinalCooldown());
        yield return new WaitForSeconds(0);
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
