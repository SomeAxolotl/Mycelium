using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [HideInInspector] public bool canSkill = true;

    [SerializeField] private float valueBase = 10f;
    [SerializeField] private float valueIncrement = 2f;
    public float finalSkillValue {get; private set;}

    [SerializeField] private float cooldownBase = 6f;
    [SerializeField] private float cooldownIncrement = -0.5f;
    public float finalSkillCooldown;
    public Coroutine cooldownCoroutine = null;
    public Coroutine hudCooldownCoroutine = null;

    protected bool fungalActive = false;
    private float fungalMightBonusChungus = 1f;

    public GameObject player;
    public CharacterStats characterStats;
    public PlayerController playerController;
    public PlayerAttack playerAttack;
    public PlayerHealth playerHealth;
    public GameObject curWeapon;

    public HUDSkills hudSkills;
    public int skillSlot;
    
    public Animator currentAnimator;

    void Start()
    {
        player = transform.parent.parent.gameObject;
        characterStats = player.GetComponent<CharacterStats>();
        currentAnimator = player.GetComponent<Animator>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerAttack = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerAttack>();
        playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>();
    }

    //this is called at the start of all the subclass skills. all stat math for skills can be done here and it should b fine
    public virtual void CalculateProperties()
    {
        player = transform.parent.parent.gameObject;
        characterStats = player.GetComponent<CharacterStats>();
        currentAnimator = player.GetComponent<Animator>();
        if(GameObject.FindWithTag("currentWeapon") != null)
        {
            curWeapon = GameObject.FindWithTag("currentWeapon");
        }
        //int t = characterStats.sentienceLevel;
        //float lerpValue = (-0.081365f) + (0.08f*t) + (Mathf.Pow(0.0015f*t, 2)) - (Mathf.Pow(0.000135f*t, 3));

        //Tentative value math -- Lerps between ValueAt1Sentience and ValueAt15Sentience depending on what ur sentience lvl is
        //finalSkillValue = Mathf.RoundToInt(Mathf.Lerp(ValueAt1Sentience, ValueAt15Sentience, lerpValue));
        finalSkillValue = (valueBase + ((characterStats.sentienceLevel - 1) * valueIncrement)) * fungalMightBonusChungus;

        //Tentative cooldown math -- Lerps between CooldownAt1Sentience and CooldownAt15Sentience depending on what ur sentience lvl is
        //finalSkillCooldown = Mathf.Lerp(CooldownAt1Sentience, CooldownAt15Sentience, lerpValue);
        finalSkillCooldown = Mathf.Clamp(cooldownBase + ((characterStats.sentienceLevel - 1) * cooldownIncrement), cooldownBase * 0.2f, cooldownBase);
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

    public void ActivateSkill(int slot, bool isTriggered = false)
    {
        if (isPlayerCurrentPlayer())
        {
            playerController.DisableController();
            playerAttack.DisableAttack();
        }

        skillSlot = slot;

        CalculateProperties();
        Actions.ActivatedSkill?.Invoke(this);
        if (!isTriggered)
        {   
            StartCooldown(finalSkillCooldown);
        }

        DoSkill();

        //prevents the initial cast from clearing all fungal mights
        if (this is not FungalMight)
        {
            ClearAllFungalMights();
        }
    }

    public virtual void DoSkill()
    {
        //Overrided by subclasses
    }

    public virtual void StartCooldown(float skillCooldown)
    {
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
        if (hudCooldownCoroutine != null)
        {
            hudSkills.StopHUDCoroutine(hudCooldownCoroutine);
        }

        cooldownCoroutine = StartCoroutine(Cooldown(skillCooldown));
    }

    public IEnumerator Cooldown(float skillCooldown)
    {
        if (isPlayerCurrentPlayer())
        {
            hudCooldownCoroutine = hudSkills.StartCooldownUI(skillSlot, skillCooldown);
        }

        canSkill = false;
        yield return new WaitForSeconds(skillCooldown);
        canSkill = true;

        cooldownCoroutine = null;
        hudCooldownCoroutine = null;
        
    }

    protected virtual void ActualCooldownStart()
    {
        //buh
    }

    //Fungal Might for Skills
    public virtual void ActivateFungalMight(float fungalMightValue)
    {
        if (this is FungalMight)
        {
            Debug.Log("setting fungalActive to true");
        }
        fungalActive = true;
        fungalMightBonusChungus = fungalMightValue;
    }
    public virtual void DeactivateFungalMight()
    {
        if (fungalActive)
        {
            fungalActive = false;
            fungalMightBonusChungus = 1f;
        }
    }

    //Clears Fungal Might for attacking and skills
    public virtual void ClearAllFungalMights()
    {
        GameObject skillLoadout = player.transform.Find("SkillLoadout").gameObject;
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
            if (particle.transform.IsChildOf(player.transform))
            {
                particle.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public void EndSkill()
    {
        if (isPlayerCurrentPlayer())
        {
            playerController.EnableController();
            playerAttack.EnableAttack();
        }
    }

    protected bool isPlayerCurrentPlayer()
    {
        if (player == GameObject.FindWithTag("currentPlayer"))
        {   
            return true;
        }
        else
        {
            return false;
        }
    }
}
