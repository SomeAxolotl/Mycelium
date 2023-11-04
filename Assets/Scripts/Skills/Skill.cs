using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [HideInInspector] public bool canSkill = true;

    [SerializeField] private float baseSkillDamage = 10f;
    [SerializeField] private float sentienceDamageScalar = 1f; //How much the skill's damage scales off Sentience (is multiplied with bonus)
    private float bonusSkillDamage;
    public float finalSkillDamage;

    [SerializeField] private float baseSkillCooldown = 5f;
    private float decreasingSkillCooldown;
    public float finalSkillCooldown;

    private CharacterStats characterStats;

    private HUDSkills hudSkills;
    private int skillSlot;

    void Start()
    {
        characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
    }

    //this is called at the start of all the subclass skills. all stat math for skills can be done here and it should b fine
    public void CalculateProperties()
    {
        characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();

        int sentienceLevel = characterStats.sentienceLevel;

        //Tentative damage math
        bonusSkillDamage = sentienceLevel * sentienceDamageScalar;
        finalSkillDamage = baseSkillDamage + bonusSkillDamage;

        //Tentative cooldown math -- Lerps between -10% and -75% depending on what ur sentience lvl is
        //at 0 it's -10% cdr
        //at 5 it's -42.5% cdr
        //at 8 it's 62% cdr
        //at 10 it's 75% cdr
        decreasingSkillCooldown = 1 - Mathf.Lerp(0.1f, 0.75f, sentienceLevel / 15f);
        finalSkillCooldown = baseSkillCooldown * decreasingSkillCooldown;
    }

    public void ActivateSkill(int slot)
    {
        skillSlot = slot;

        CalculateProperties();
        StartCooldown();
        DoSkill();
    }

    public virtual void DoSkill()
    {
        //Overrided by subclasses
    }

    public void StartCooldown()
    {
        StartCoroutine("Cooldown");
    }

    IEnumerator Cooldown()
    {
        hudSkills.StartCooldownUI(skillSlot, finalSkillCooldown);

        canSkill = false;
        yield return new WaitForSeconds(finalSkillCooldown);
        canSkill = true;
    }
}
