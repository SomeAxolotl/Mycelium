using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class HUDSkills : MonoBehaviour
{
    //Hit

    private MeleeAttack meleeAttack;
    private float hitCooldown;
    private float hitCooldownCounter;

    private Image hitCooldownBackground;
    private TMP_Text hitText;
    private TMP_Text hitButtonText;

    //Skill

    private float skill1Cooldown;
    private float skill1CooldownCounter;

    private Image skill1CooldownBackground;
    private TMP_Text skill1Text;
    private TMP_Text skill1ButtonText;

    void Start()
    {
        hitCooldownBackground = GameObject.Find("HitCooldownBackground").GetComponent<Image>();
        hitText = GameObject.Find("HitText").GetComponent<TMP_Text>();
        hitButtonText = GameObject.Find("HitButton").GetComponent<TMP_Text>();

        skill1CooldownBackground = GameObject.Find("Skill1CooldownBackground").GetComponent<Image>();
        skill1Text = GameObject.Find("Skill1Text").GetComponent<TMP_Text>();
        skill1ButtonText = GameObject.Find("Skill1Button").GetComponent<TMP_Text>();
    }

    public void StartHitCooldownUI()
    {
        meleeAttack = GameObject.FindWithTag("currentPlayer").GetComponent<MeleeAttack>();
        hitCooldown = meleeAttack.finalAtkCooldown;
        hitCooldownCounter = hitCooldown;
    }

    public void StartSkill1CooldownUI(float cooldown)
    {
        skill1Cooldown = cooldown;
        skill1CooldownCounter = cooldown;
    }

    void Update()
    {
        if (hitCooldownCounter > 0)
        {
            hitCooldownCounter -= Time.deltaTime;
            hitCooldownBackground.fillAmount = hitCooldownCounter / hitCooldown;
        }

        if (skill1CooldownCounter > 0)
        {
            skill1CooldownCounter -= Time.deltaTime;
            skill1CooldownBackground.fillAmount = skill1CooldownCounter / skill1Cooldown;
        }
    }

    /*
    public void UpdateSkillButtons()
    {
        hitAction = inputAction.bindings

        hitButtonText
        skillButtonText
    }
    */
}
