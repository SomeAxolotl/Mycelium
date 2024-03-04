using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [HideInInspector] public bool canSkill = true;

    [SerializeField] private float valueBase = 10f;
    [SerializeField] private float valueIncrement = 2f;
    public float finalSkillValue;

    [SerializeField] private float cooldownBase = 6f;
    [SerializeField] private float cooldownIncrement = -0.5f;
    public float finalSkillCooldown;

    private float fungalMightBonus = 1f;

    public GameObject player;
    public CharacterStats characterStats;
    public PlayerController playerController;
    public PlayerHealth playerHealth;

    private HUDSkills hudSkills;
    private int skillSlot;
    
    public Animator currentAnimator;

    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        characterStats = player.GetComponent<CharacterStats>();
        currentAnimator = player.GetComponent<Animator>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>();
    }

    //this is called at the start of all the subclass skills. all stat math for skills can be done here and it should b fine
    public void CalculateProperties()
    {
        player = GameObject.FindWithTag("currentPlayer");
        characterStats = player.GetComponent<CharacterStats>();
        currentAnimator = player.GetComponent<Animator>();

        //int t = characterStats.sentienceLevel;
        //float lerpValue = (-0.081365f) + (0.08f*t) + (Mathf.Pow(0.0015f*t, 2)) - (Mathf.Pow(0.000135f*t, 3));

        //Tentative value math -- Lerps between ValueAt1Sentience and ValueAt15Sentience depending on what ur sentience lvl is
        //finalSkillValue = Mathf.RoundToInt(Mathf.Lerp(ValueAt1Sentience, ValueAt15Sentience, lerpValue));
        finalSkillValue = valueBase + ((characterStats.sentienceLevel - 1) * valueIncrement);

        //Tentative cooldown math -- Lerps between CooldownAt1Sentience and CooldownAt15Sentience depending on what ur sentience lvl is
        //finalSkillCooldown = Mathf.Lerp(CooldownAt1Sentience, CooldownAt15Sentience, lerpValue);
        finalSkillCooldown = cooldownBase + ((characterStats.sentienceLevel - 1) * cooldownIncrement);
    }

    public float GetFinalValue()
    {
        CalculateProperties();
        return finalSkillValue;
    }

    public float GetFinalCooldown()
    {
        CalculateProperties();
        return finalSkillCooldown;
    }

    public void ActivateSkill(int slot)
    {
        playerController.DisableController();

        skillSlot = slot;

        CalculateProperties();
        Debug.Log(this.name + " " + finalSkillValue);
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

        PlayerAttack playerAttack = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerAttack>();
        playerAttack.DeactivateFungalMight();

        GameObject[] fungalMightParticles = GameObject.FindGameObjectsWithTag("FungalMightParticles");
        foreach (GameObject particle in fungalMightParticles)
        {
            particle.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void EndSkill()
    {
        playerController.EnableController();
    }
}
