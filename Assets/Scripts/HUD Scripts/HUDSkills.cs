using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class HUDSkills : MonoBehaviour
{
    //Dodge

    private Image dodgeCooldownBackground;
    private Image dodgeIcon;
    private TMP_Text dodgeButtonText;

    //Hit

    private Image hitCooldownBackground;
    private Image hitIcon;
    private TMP_Text hitButtonText;

    //Skill 1

    private Image skill1CooldownBackground;
    private Image skill1Icon;
    private TMP_Text skill1ButtonText;

    //Skill 2

    private Image skill2CooldownBackground;
    private Image skill2Icon;
    private TMP_Text skill2ButtonText;

    //Skill 3

    private Image speciesCooldownBackground;
    private Image speciesIcon;
    private TMP_Text speciesButtonText;

    private List<Sprite> spriteList = new List<Sprite>();
    [SerializeField] private Sprite noSkillSprite;

    void Start()
    {
        dodgeCooldownBackground = GameObject.Find("DodgeCooldownBackground").GetComponent<Image>();
        dodgeIcon = GameObject.Find("DodgeIcon").GetComponent<Image>();
        dodgeButtonText = GameObject.Find("DodgeButton").GetComponent<TMP_Text>();

        speciesCooldownBackground = GameObject.Find("SpeciesCooldownBackground").GetComponent<Image>();
        speciesIcon = GameObject.Find("SpeciesIcon").GetComponent<Image>();
        speciesButtonText = GameObject.Find("SpeciesButton").GetComponent<TMP_Text>();

        skill1CooldownBackground = GameObject.Find("Skill1CooldownBackground").GetComponent<Image>();
        skill1Icon = GameObject.Find("Skill1Icon").GetComponent<Image>();
        skill1ButtonText = GameObject.Find("Skill1Button").GetComponent<TMP_Text>();

        skill2CooldownBackground = GameObject.Find("Skill2CooldownBackground").GetComponent<Image>();
        skill2Icon = GameObject.Find("Skill2Icon").GetComponent<Image>();
        skill2ButtonText = GameObject.Find("Skill2Button").GetComponent<TMP_Text>();

        hitCooldownBackground = GameObject.Find("HitCooldownBackground").GetComponent<Image>();
        hitIcon = GameObject.Find("HitIcon").GetComponent<Image>();
        hitButtonText = GameObject.Find("HitButton").GetComponent<TMP_Text>();

        spriteList.AddRange(Resources.LoadAll<Sprite>("PlaceholderIcons"));
    }

    public void ChangeSkillIcon(string name, int slot)
    {
        Image skillIcon = speciesIcon;
        switch (slot)
        {
            //Skills
            case 0:
                skillIcon = speciesIcon;
                break;
            case 1:
                skillIcon = skill1Icon;
                break;
            case 2:
                skillIcon = skill2Icon;
                break;
        }

        int spriteIndex = -1;
        foreach(Sprite sprite in spriteList)
        {
            if (name.Contains(sprite.name))
            {

                spriteIndex = spriteList.IndexOf(sprite);
            }
        }

        //Debug.Log("Sprite Index: " + spriteIndex);
        Sprite skillSprite;
        if (spriteIndex == -1)
        {
            skillSprite = noSkillSprite;
        }
        else
        {
           skillSprite = spriteList[spriteIndex]; 
        }
        
        skillIcon.sprite = skillSprite;
    }

    public void StartCooldownUI(int slot, float cooldown)
    {
        StartCoroutine(SkillCooldown(slot, cooldown));
    }

    IEnumerator SkillCooldown(int slot, float cooldown)
    {
        Image cooldownBackground = speciesCooldownBackground;
        switch (slot)
        {
            //Skills
            case 0:
                cooldownBackground = speciesCooldownBackground;
                break;
            case 1:
                cooldownBackground = skill1CooldownBackground;
                break;
            case 2:
                cooldownBackground = skill2CooldownBackground;
                break;

            //Attack and Dodge
            case 3:
                cooldownBackground = hitCooldownBackground;
                break;
            case 4:
                cooldownBackground = dodgeCooldownBackground;
                break;
        }

        float cooldownLeft = cooldown;
        while (cooldownLeft >= 0)
        {
            cooldownLeft -= Time.deltaTime;
            cooldownBackground.fillAmount = cooldownLeft / cooldown;
            yield return null;
        }

        yield break;
    }

    public List<Sprite> GetAllSkillSprites()
    {
        List<Sprite> allSprites = new List<Sprite>();

        allSprites.Add(speciesIcon.sprite);
        allSprites.Add(skill1Icon.sprite);
        allSprites.Add(skill2Icon.sprite);

        return allSprites;
    }

    /*void Update()
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

        if (speciesCooldownCounter > 0)
        {
            speciesCooldownCounter -= Time.deltaTime;
            speciesCooldownBackground.fillAmount = speciesCooldownCounter / speciesCooldown;
        }
    }
    */

    /*
    public void UpdateSkillButtons()
    {
        hitAction = inputAction.bindings

        hitButtonText
        skillButtonText
    }
    */
}
