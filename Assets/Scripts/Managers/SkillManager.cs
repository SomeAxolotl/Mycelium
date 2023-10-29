using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField][Tooltip("Put ALL skill prefabs here")] List<GameObject> skillList = new List<GameObject>();
    [SerializeField][Tooltip("Default skill if none is found")] GameObject noSkill;

    public void SetSkill(string name, int slot)
    {   
        bool skillFound = false;

        foreach (GameObject skillPrefab in skillList)
        {
            if (skillPrefab.name == name)
            {
                skillFound = true;

                Destroy(transform.GetChild(slot).gameObject);

                GameObject newSkill = Instantiate(skillPrefab, this.gameObject.transform);
                newSkill.transform.SetSiblingIndex(slot);
            }
        }

        if (!skillFound)
        {
            Debug.Log("NO SKILL FOUND WITH THAT NAME");

            Destroy(transform.GetChild(slot).gameObject);

            GameObject newSkill = Instantiate(noSkill, this.gameObject.transform);
            newSkill.transform.SetSiblingIndex(slot);
        }
    }
}
