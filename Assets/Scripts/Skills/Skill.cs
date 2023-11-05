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

    public float fungalMightBonus = 0f;

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
        finalSkillValue = baseSkillValue + bonusSkillValue + fungalMightBonus;

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

        if (!this.name.Contains("FungalMight"))
        {
            ClearFungalMight();
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

    //Fungal Might Stuff

    public void ActivateFungalMight(float fungalMightValue)
    {
        fungalMightBonus += fungalMightValue;
    }

    public void DeactivateFungalMight()
    {
        fungalMightBonus = 0;
    }

    public void ClearFungalMight()
    {
        GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
        foreach (Transform child in skillLoadout.transform)
        {
            Skill skill = child.gameObject.GetComponent<Skill>();
            skill.DeactivateFungalMight();
        }

        GameObject[] fungalMightParticles = GameObject.FindGameObjectsWithTag("FungalMightParticles");
        foreach (GameObject particle in fungalMightParticles)
        {
            particle.GetComponent<ParticleSystem>().Stop();
        }
    }
}
