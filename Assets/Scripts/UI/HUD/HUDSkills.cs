using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;

public class HUDSkills : MonoBehaviour
{
    [SerializeField] List<Skill> skills = new List<Skill>();

    private List<Sprite> spriteList = new List<Sprite>();
    [SerializeField] private Sprite noSkillSprite;

    [SerializeField] float popSizeScalar = 1.5f;
    [SerializeField] float popDuration = 0.5f;
    [SerializeField] float deflateDuration = 0.5f;

    void Start()
    {
        foreach (Skill skill in skills)
        {
            skill.Initialize();
        }

        spriteList.AddRange(Resources.LoadAll<Sprite>("Skill Icons"));

        UpdateHUDIcons();
    }

    public void UpdateHUDIcons()
    {
        StartCoroutine(UpdateHUDIconsCoroutine());
    }

    IEnumerator UpdateHUDIconsCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject player = GameObject.FindWithTag("currentPlayer");
        Transform skillLoadout = player.transform.Find("SkillLoadout");
        ChangeSkillIcon(skillLoadout.GetChild(0).gameObject.name, 0);
        ChangeSkillIcon(skillLoadout.GetChild(1).gameObject.name, 1);
        ChangeSkillIcon(skillLoadout.GetChild(2).gameObject.name, 2);
    }

    public void ChangeSkillIcon(string skillName, int slot)
    {
        Skill selectedSkill = GetSkill(slot);
        selectedSkill.icon.sprite = NameToSkillSprite(skillName);
    }

    Sprite NameToSkillSprite(string name)
    {
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

        return skillSprite;
    }

    public void StopHUDCoroutine(Coroutine hudCooldownCoroutine)
    {
        StopCoroutine(hudCooldownCoroutine);
    }
    public void StopHUDEffectCoroutine(Coroutine hudCooldownCoroutine)
    {
        StopCoroutine(hudCooldownCoroutine);
    }

    public Coroutine StartCooldownUI(int slot, float cooldown)
    {
        return StartCoroutine(SkillCooldown(slot, cooldown));
    }

    public Coroutine StartEffectUI(int slot, float cooldown)
    {
        return StartCoroutine(EffectCooldown(slot, cooldown));
    }

    public bool pauseEffect = false;
    IEnumerator EffectCooldown(int slot, float cooldown)
    {
        Skill selectedSkill = GetSkill(slot);

        Image cooldownBackground = selectedSkill.cooldownBackground;
        Image skillIcon = selectedSkill.icon;
        Vector3 iconStartingScale = selectedSkill.iconStartingScale;

        float cooldownLeft = cooldown;
        while(cooldownLeft >= 0){
            while(pauseEffect){
                yield return null;
            }
            cooldownLeft -= Time.deltaTime;
            //cooldownBackground.color = Color.white;
            cooldownBackground.fillAmount = 1 - (cooldownLeft / cooldown);
            yield return null;
        }

        StartCoroutine(SkillCooldownIconPop(skillIcon.gameObject, iconStartingScale));
    }

    IEnumerator SkillCooldown(int slot, float cooldown)
    {
        Skill selectedSkill = GetSkill(slot);

        Image cooldownBackground = selectedSkill.cooldownBackground;
        Image skillIcon = selectedSkill.icon;
        Vector3 iconStartingScale = selectedSkill.iconStartingScale;

        float cooldownLeft = cooldown;
        while (cooldownLeft >= 0)
        {
            cooldownLeft -= Time.deltaTime;
            cooldownBackground.fillAmount = cooldownLeft / cooldown;
            yield return null;
        }

        StartCoroutine(SkillCooldownIconPop(skillIcon.gameObject, iconStartingScale));
    }

   private void SlotSwitch(){
    
   } 

    IEnumerator SkillCooldownIconPop(GameObject icon, Vector3 iconStartingScale)
    {
        Vector3 popSize = iconStartingScale * popSizeScalar;

        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float popLerp = DylanTree.EaseOutQuart(popCounter / popDuration);
            icon.transform.localScale = Vector3.Lerp(iconStartingScale, popSize, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }
        icon.transform.localScale = popSize;

        float deflateCounter = 0f;
        while (deflateCounter < deflateDuration)
        {
            float deflateLerp = deflateCounter / deflateDuration;
            icon.transform.localScale = Vector3.Lerp(popSize, iconStartingScale, deflateLerp);

            deflateCounter += Time.deltaTime;
            yield return null;  
        }
        icon.transform.localScale = iconStartingScale;
    }

    public List<Sprite> GetAllSkillSprites()
    {
        List<Sprite> allSprites = new List<Sprite>();

        allSprites.Add(GetSkill(0).icon.sprite);
        allSprites.Add(GetSkill(1).icon.sprite);
        allSprites.Add(GetSkill(2).icon.sprite);

        return allSprites;
    }

    public Sprite GetSkillSprite(string skillName)
    {
        return NameToSkillSprite(skillName);
    }

    public void ToggleActiveBorder(int slot, bool isActive)
    {
        Debug.Log("Pop that pussy");
        Skill selectedSkill = GetSkill(slot);
        StartCoroutine(PopActivateBorder(selectedSkill, isActive));
    }

    IEnumerator PopActivateBorder(Skill skill, bool doesPopIn)
    {
        Vector3 fromScale = doesPopIn ? skill.borderHiddenScale : skill.borderStartingScale;
        Vector3 toScale = doesPopIn ? skill.borderStartingScale : skill.borderHiddenScale;

        GameObject activateBorder = skill.activateBorder;

        if (activateBorder.transform.localScale == toScale)
        {
            yield break;
        }

        float popCounter = 0f;
        float popDuration = 0.5f;
        while (popCounter < popDuration)
        {
            float popLerp = DylanTree.EaseOutQuart(popCounter / popDuration);
            activateBorder.transform.localScale = Vector3.Lerp(fromScale, toScale, popLerp);

            popCounter += Time.deltaTime;
            yield return null;  
        }

        activateBorder.transform.localScale = toScale;
    }

    //Skills

    Skill GetSkill(int slot)
    {
        Skill foundSkill = skills.Find(s => s.slot == slot);
        if (foundSkill == null)
        {
            Debug.LogError("No skill found for slot " + slot);
            return null;
        }
        
        return foundSkill;
    }

    [System.Serializable]
    class Skill
    {
        public int slot;
        public Image cooldownBackground;
        public Image icon;
        public Vector3 iconStartingScale {get; private set;}
        public Vector3 borderStartingScale {get; private set;}
        public Vector3 borderHiddenScale {get; private set;}
        public TMP_Text buttonText;
        public GameObject activateBorder;

        public void Initialize()
        {
            iconStartingScale = icon.transform.localScale;

            borderStartingScale = activateBorder.transform.localScale;
            borderHiddenScale = borderStartingScale * 0.92f;
            activateBorder.transform.localScale = borderHiddenScale;
        }
    }
}
