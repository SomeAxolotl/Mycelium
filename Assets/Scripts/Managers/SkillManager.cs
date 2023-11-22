using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField][Tooltip("Put ALL skill prefabs here")] public List<GameObject> skillList = new List<GameObject>();
    [SerializeField][Tooltip("Default skill if there's no skill")] GameObject noSkill;

    public void SetSkill(string name, int slot, GameObject player)
    {   
        GameObject skillLoadout = player.transform.Find("SkillLoadout").gameObject;
        bool skillFound = false;

        foreach (GameObject skillPrefab in skillList)
        {
            if (skillPrefab.name == name)
            {
                skillFound = true;

                Destroy(skillLoadout.transform.GetChild(slot).gameObject);

                GameObject newSkill = Instantiate(skillPrefab, skillLoadout.transform);
                newSkill.transform.SetSiblingIndex(slot);
            }
        }

        if (!skillFound)
        {
            Debug.Log("NO SKILL FOUND WITH THAT NAME");

            Destroy(skillLoadout.transform.GetChild(slot).gameObject);

            GameObject newSkill = Instantiate(noSkill, skillLoadout.transform);
            newSkill.transform.SetSiblingIndex(slot);
        }
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Dictionary<string, float> testValues = GetAllSkillCooldowns();

            foreach (string skillName in testValues.Keys)
            {
                float skillValue = testValues[skillName];
        
                Debug.Log($"Skill: {skillName}, Cooldown: {skillValue}");
            }
        }
    }*/

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

    public List<float> GetEquippedSkillValues(GameObject player)
    {
        Transform skillLoadout = player.transform.Find("SkillLoadout");

        List<float> allSkillValues = new List<float>();
        foreach (Transform child in skillLoadout)
        {
            allSkillValues.Add(child.gameObject.GetComponent<Skill>().GetFinalValue());
        }

        return allSkillValues;
    }

    public List<float> GetEquippedSkillCooldowns(GameObject player)
    {
        Transform skillLoadout = player.transform.Find("SkillLoadout");

        List<float> allSkillCooldowns = new List<float>();
        foreach (Transform child in skillLoadout)
        {
            allSkillCooldowns.Add(child.gameObject.GetComponent<Skill>().GetFinalCooldown());
        }

        return allSkillCooldowns;
    }
}
