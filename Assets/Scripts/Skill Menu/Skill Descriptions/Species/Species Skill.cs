using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class SpeciesSkill : MonoBehaviour, ISelectHandler
{
    public TMP_Text SkillDesc;
    public GameObject SkillDescriptionPanel;
    public CharacterStats currentstats;
    public SkillManager skillManager;
    // Start is called before the first frame update
    void OnEnable()
    {
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
    }
    public void OnSelect(BaseEventData eventData)
    {
           Debug.Log(currentstats.equippedSkills[0]);
           SkillDescriptionPanel.SetActive(true);
            if (currentstats.equippedSkills[0] == "FungalMight")
            {
                SkillDesc.text = "Hello";
            }
        
    }
}
