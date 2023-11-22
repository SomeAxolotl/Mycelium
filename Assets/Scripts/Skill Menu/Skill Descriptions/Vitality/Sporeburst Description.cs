using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class SporeburstDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.vitalityLevel >= 10)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Sporeburst: <br><br> <size=25>Spores explode from you stunning and damaging all enemies caught in its radius. Heal for 50% of all damage dealt.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Sporeburst: <br><size=25>Spores explode from you stunning and damaging all enemies caught in its radius. Heal for 50% of all damage dealt.<br> <color=#FF534C>Requires Vitality Level 10";
    }
   }
}
