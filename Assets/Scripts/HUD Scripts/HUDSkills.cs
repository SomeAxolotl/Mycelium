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

    private AoEEffect aoeSkill;
    private float skillCooldown;
    private float skillCooldownCounter;

    private Image skillCooldownBackground;
    private TMP_Text skillText;
    private TMP_Text skillButtonText;

    void Start()
    {
        hitCooldownBackground = GameObject.Find("HitCooldownBackground").GetComponent<Image>();
        hitText = GameObject.Find("HitText").GetComponent<TMP_Text>();
        hitButtonText = GameObject.Find("HitButton").GetComponent<TMP_Text>();

        skillCooldownBackground = GameObject.Find("Skill1CooldownBackground").GetComponent<Image>();
        skillText = GameObject.Find("Skill1Text").GetComponent<TMP_Text>();
        skillButtonText = GameObject.Find("Skill1Button").GetComponent<TMP_Text>();
    }

    public void StartHitCooldownUI()
    {
        meleeAttack = GameObject.FindWithTag("currentPlayer").GetComponent<MeleeAttack>();
        hitCooldown = meleeAttack.finalAtkCooldown;
        hitCooldownCounter = hitCooldown;
    }

    /*public void StartSkillCooldownUI()
    {
        aoeSkill = GameObject.FindWithTag("currentPlayer").GetComponent<AoEEffect>();
        skillCooldown = aoeSkill.delay;
        skillCooldownCounter = skillCooldown;
    }*/

    void Update()
    {
        if (hitCooldownCounter > 0)
        {
            hitCooldownCounter -= Time.deltaTime;
            hitCooldownBackground.fillAmount = hitCooldownCounter / hitCooldown;
        }

        /*if (skillCooldownCounter > 0)
        {
            skillCooldownCounter -= Time.deltaTime;
            skillCooldownBackground.fillAmount = skillCooldownCounter / skillCooldown;
        }*/
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
