using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class LeechingSporeDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.vitalityLevel >= 5)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Leeching Spores: <br><br> <size=25>Infest a nearby enemy with a leeching spore. The spore steals health every second from the enemy and restores it to you.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Leeching Spores: <br><size=25>Infest a nearby enemy with a leeching spore. The spore steals health every second from the enemy and restores it to you.<br> <color=#FF534C>Requires Vitality Level 5";
    }
   }
}
