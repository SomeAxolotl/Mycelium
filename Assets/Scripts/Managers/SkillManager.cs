using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField][Tooltip("Put ALL skill prefabs here")] List<GameObject> skillList = new List<GameObject>();
    [SerializeField][Tooltip("Default skill if there's no skill")] GameObject noSkill;

    private GameObject skillLoadout;

    private HUDSkills hudSkills;

    void Start()
    {
        skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
    }

    public void SetSkill(string name, int slot)
    {   
        skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
        bool skillFound = false;

        foreach (GameObject skillPrefab in skillList)
        {
            if (skillPrefab.name == name)
            {
                skillFound = true;

                Destroy(skillLoadout.transform.GetChild(slot).gameObject);

                GameObject newSkill = Instantiate(skillPrefab, skillLoadout.transform);
                newSkill.transform.SetSiblingIndex(slot);

                hudSkills.ChangeSkillIcon(name, slot);
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
}
