using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class EruptionDesc : MonoBehaviour, ISelectHandler
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
    if (currentstats.primalLevel >= 5)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Eruption: <br> <size=25>Stomp the ground with primal strength<br> dealing damage to all enemies around you. <br>Deals more damage to enemies closer to you.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Eruption: <br> <size=25>Stomp the ground with primal strength <br> dealing damage to all enemies around you.<br> Deals additional damage to enemies closer to you.<br> <color=#FF534C>Requires Primal Level 5";
    }
   }
}
