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

    public List<float> GetAllSkillValues(GameObject player)
    {
        Transform skillLoadout = player.transform.Find("SkillLoadout");

        List<float> allSkillValues = new List<float>();
        foreach (Transform child in skillLoadout)
        {
            allSkillValues.Add(child.gameObject.GetComponent<Skill>().GetFinalValue());
        }

        return allSkillValues;
    }

    public List<float> GetAllSkillCooldowns(GameObject player)
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
