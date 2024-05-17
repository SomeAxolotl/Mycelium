using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField][Tooltip("Put ALL skill prefabs here")] public List<GameObject> skillList = new List<GameObject>();
    [SerializeField][Tooltip("Default skill if there's no skill")] GameObject noSkill;

    public enum StatSkills
    {
        NoSkill,
        Eruption,
        LivingCyclone,
        RelentlessFury,
        Blitz,
        TrophicCascade,
        Mycotoxins,
        Spineshot,
        UnstablePuffball,
        Undergrowth,
        LeechingSpore,
        Sporeburst,
        DefenseMechanism
    }

    public void SetSkill(string skillName, int slot, GameObject player)
    {   
        GameObject skillLoadout = player.transform.Find("SkillLoadout").gameObject;
        bool skillFound = false;

        GameObject newSkill;
        foreach (GameObject skillPrefab in skillList)
        {
            if (skillPrefab.name == skillName)
            {
                skillFound = true;

                Destroy(skillLoadout.transform.GetChild(slot).gameObject);

                newSkill = Instantiate(skillPrefab, skillLoadout.transform);
                newSkill.transform.SetSiblingIndex(slot);

                SetEquippedCharacterSkill(skillName, slot, player);
            }
        }

        if (!skillFound)
        {
            //Debug.Log("NO SKILL FOUND WITH THAT NAME");

            Destroy(skillLoadout.transform.GetChild(slot).gameObject);

            newSkill = Instantiate(noSkill, skillLoadout.transform);
            newSkill.transform.SetSiblingIndex(slot);

            SetEquippedCharacterSkill(skillName, slot, player);
        }
    }

    public void SetEquippedCharacterSkill(string skillString, int slot, GameObject character)
    {
        CharacterStats currentStats = character.GetComponent<CharacterStats>();
        currentStats.SetEquippedSkill(skillString, slot);
    }

    public Dictionary<string, float> GetAllSkillValues()
    {
        Dictionary<string, float> allSkillValues = new Dictionary<string, float>();

        foreach (GameObject skill in skillList)
        {
            GameObject skillObject = Instantiate(skill);

            string skillName = skill.name;
            float skillValue = skill.GetComponent<Skill>().GetFinalValue();
            allSkillValues.Add(skillName, skillValue);

            Destroy(skillObject);
        }

        return allSkillValues;
    }

    public Dictionary<string, float> GetAllSkillCooldowns()
    {
        Dictionary<string, float> allSkillCooldowns = new Dictionary<string, float>();

        foreach (GameObject skill in skillList)
        {
            GameObject skillObject = Instantiate(skill);

            string skillName = skill.name;
            float skillCooldown = skill.GetComponent<Skill>().GetFinalCooldown();
            allSkillCooldowns.Add(skillName, skillCooldown);

            Destroy(skillObject);
        }

        return allSkillCooldowns;
    }

    public List<float> GetEquippedSkillValues(GameObject character)
    {
        Transform skillLoadout = character.transform.Find("SkillLoadout");

        List<float> allSkillValues = new List<float>();
        foreach (Transform child in skillLoadout)
        {
            allSkillValues.Add(child.gameObject.GetComponent<Skill>().GetFinalValue());
        }

        return allSkillValues;
    }

    public List<float> GetEquippedSkillCooldowns(GameObject character)
    {
        Transform skillLoadout = character.transform.Find("SkillLoadout");

        List<float> allSkillCooldowns = new List<float>();
        foreach (Transform child in skillLoadout)
        {
            allSkillCooldowns.Add(child.gameObject.GetComponent<Skill>().GetFinalCooldown());
        }

        return allSkillCooldowns;
    }
}
