using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class HUDSkills : MonoBehaviour
{
    //Hit

    private float hitCooldown;
    private float hitCooldownCounter;

    private Image hitCooldownBackground;
    private TMP_Text hitText;
    private TMP_Text hitButtonText;

    //Skill 1

    private float skill1Cooldown;
    private float skill1CooldownCounter;
    private Image skill1CooldownBackground;
    private TMP_Text skill1Text;
    private TMP_Text skill1ButtonText;

    //Skill 2

    private float skill2Cooldown;
    private float skill2CooldownCounter;
    private Image skill2CooldownBackground;
    private TMP_Text skill2Text;
    private TMP_Text skill2ButtonText;

    //Skill 3

    private float skill3Cooldown;
    private float skill3CooldownCounter;
    private Image skill3CooldownBackground;
    private TMP_Text skill3Text;
    private TMP_Text skill3ButtonText;

    void Start()
    {
        hitCooldownBackground = GameObject.Find("HitCooldownBackground").GetComponent<Image>();
        hitText = GameObject.Find("HitText").GetComponent<TMP_Text>();
        hitButtonText = GameObject.Find("HitButton").GetComponent<TMP_Text>();

        skill1CooldownBackground = GameObject.Find("Skill1CooldownBackground").GetComponent<Image>();
        skill1Text = GameObject.Find("Skill1Text").GetComponent<TMP_Text>();
        skill1ButtonText = GameObject.Find("Skill1Button").GetComponent<TMP_Text>();

        skill2CooldownBackground = GameObject.Find("Skill2CooldownBackground").GetComponent<Image>();
        skill2Text = GameObject.Find("Skill2Text").GetComponent<TMP_Text>();
        skill2ButtonText = GameObject.Find("Skill2Button").GetComponent<TMP_Text>();

        skill3CooldownBackground = GameObject.Find("Skill3CooldownBackground").GetComponent<Image>();
        skill3Text = GameObject.Find("Skill3Text").GetComponent<TMP_Text>();
        skill3ButtonText = GameObject.Find("Skill3Button").GetComponent<TMP_Text>();
    }

    public void StartHitCooldownUI(float cooldown)
    {
        hitCooldown = cooldown;
        hitCooldownCounter = cooldown;
    }

    public void StartSkillCooldownUI(int slot, float cooldown)
    {
        Debug.Log("Cooldown: " + cooldown);

        switch (slot)
        {
            case 0:
                skill1Cooldown = cooldown;
                skill1CooldownCounter = cooldown;
                break;
            case 1:
                skill2Cooldown = cooldown;
                skill2CooldownCounter = cooldown;
                break;
            case 2:
                skill3Cooldown = cooldown;
                skill3CooldownCounter = cooldown;
                break;
        }
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

        if (skill2CooldownCounter > 0)
        {
            skill2CooldownCounter -= Time.deltaTime;
            skill2CooldownBackground.fillAmount = skill2CooldownCounter / skill2Cooldown;
        }

        if (skill3CooldownCounter > 0)
        {
            skill3CooldownCounter -= Time.deltaTime;
            skill3CooldownBackground.fillAmount = skill3CooldownCounter / skill3Cooldown;
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
