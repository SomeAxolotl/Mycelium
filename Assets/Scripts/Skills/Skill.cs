using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [HideInInspector] public bool canSkill = true;

    [SerializeField] private float baseSkillValue = 10f;
    [SerializeField] private float sentienceScalar = 1f; //How much the skill's damage scales off Sentience (is multiplied with bonus)
    private float bonusSkillValue;
    public float finalSkillValue;

    [SerializeField] private float baseSkillCooldown = 5f;
    private float decreasingSkillCooldown;
    public float finalSkillCooldown;

    private float fungalMightBonus = 1f;

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

        //Tentative value math
        bonusSkillValue = (sentienceLevel - 1) * sentienceScalar;
        finalSkillValue = (baseSkillValue + bonusSkillValue) * fungalMightBonus;

        //Tentative cooldown math -- Lerps between -10% and -75% depending on what ur sentience lvl is
        decreasingSkillCooldown = 1 - Mathf.Lerp(0.1f, 0.75f, sentienceLevel / 15f);
        finalSkillCooldown = baseSkillCooldown * decreasingSkillCooldown;
    }

    public float GetFinalCooldown()
    {
        CalculateProperties();
        return finalSkillCooldown;
    }

    public void ActivateSkill(int slot)
    {
        skillSlot = slot;

        CalculateProperties();
        StartCooldown();
        DoSkill();

        if (!this.name.Contains("FungalMight"))
        {
            ClearAllFungalMights();
        }
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

    //Fungal Might for Skills
    public void ActivateFungalMight(float fungalMightValue)
    {
        fungalMightBonus = fungalMightValue;
    }
    public void DeactivateFungalMight()
    {
        fungalMightBonus = 1f;
    }

    //Clears Fungal Might for attacking and skills
    public void ClearAllFungalMights()
    {
        GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
        foreach (Transform child in skillLoadout.transform)
        {
            Skill skill = child.gameObject.GetComponent<Skill>();
            skill.DeactivateFungalMight();
        }

        NewPlayerAttack playerAttack = GameObject.FindWithTag("PlayerParent").GetComponent<NewPlayerAttack>();
        playerAttack.DeactivateFungalMight();

        GameObject[] fungalMightParticles = GameObject.FindGameObjectsWithTag("FungalMightParticles");
        foreach (GameObject particle in fungalMightParticles)
        {
            particle.GetComponent<ParticleSystem>().Stop();
        }
    }
}
