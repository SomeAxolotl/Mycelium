using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class UndergrowthDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.sentienceLevel >= 15)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Undergrowth: <br><br><size=25> An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Undergrowth: <br><size=25>An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit. <br><br> <color=#FF534C>Requires Sentience Level 15";
    }
   }
}
