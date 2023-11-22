using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class DefenseMechanismDescription : MonoBehaviour, ISelectHandler
{
   public TMP_Text SkillDesc;
   public GameObject SkillDescriptionPanel;
   public CharacterStats currentstats;  


   void OnEnable()
   {
    currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
   }
   public void OnSelect(BaseEventData eventData)
   {
    if (currentstats.vitalityLevel >= 15)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Defense Mechanism: <br><size=25>Reduces damage taken by 50% for 1 second. Attacks against you while Defense Mechanism is active is stored as bonus damage on your next attack equal to 50% of the damage absorbed.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Defense Mechanism: <br><size=22>Reduces damage taken by 50% for 1 second. Attacks against you while Defense Mechanism is active is stored as bonus damage on your next attack equal to 50% of the damage absorbed.<br> <color=#FF534C>Requires Vitality Level 10";
    }
   }
}
