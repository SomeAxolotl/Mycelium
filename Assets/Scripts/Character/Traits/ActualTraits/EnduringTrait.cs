using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnduringTrait : TraitBase
{
    private string SkillName = "Sporeburst";
    //Only activates when under this health, 0.5f = 50%
    private float activateAmount = 0.5f;
    //How often skill can go off
    private float activateCooldown = 6f;

    private Transform skillLoadout;
    protected SkillManager skillManager;
    protected Skill skillInstance;
    protected GameObject skillPrefab;

    public override void Start(){
        base.Start();

        traitName = "Enduring";
        traitDesc = "\nSporeburst at low health";

        health.TakeDamage += TakeDamage;
        foreach(Transform child in transform){
            if(child.name == "SkillLoadout"){
                skillLoadout = child;
            }
        }
        skillManager = playerParent.GetComponent<SkillManager>();
        skillPrefab = skillManager.skillList.Find(skill => skill.name == SkillName);
        if(skillPrefab != null){
            // Instantiate the skill directly under SkillLoadout
            var skillObject = Instantiate(skillPrefab, skillLoadout);
            skillObject.name = SkillName;
            skillInstance = skillObject.GetComponent<Skill>();
        }
    }
    public void OnDisable(){
        health.TakeDamage += TakeDamage;
    }

    private void TakeDamage(float dmgTaken){
        if(cooldownTimer == null && (health.currentHealth - dmgTaken / health.maxHealth) <= activateAmount){
            //Activate the skill
            skillInstance.CalculateProperties();
            skillInstance.DoSkill();
            //Internal cooldown so it doesnt spam
            cooldownTimer = Cooldown(activateCooldown);
            StartCoroutine(cooldownTimer);
        }
    }

    IEnumerator cooldownTimer = null;
    IEnumerator Cooldown(float cooldown){
        yield return new WaitForSeconds(cooldown);
        cooldownTimer = null;
    }
}
