using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class MycotoxinsDescriptions : MonoBehaviour, ISelectHandler
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
    if (currentstats.speedLevel >= 15)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Mycotoxins: <br> <size=25>Gain 50% bonus movement speed and release a trail of spores behind you. Enemies hit by these spores are damaged.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Mycotoxins: <br> <size=25>Gain 50% bonus movement speed and release a trail of spores behind you. Enemies hit by these spores are damaged. <br> <color=#FF534C>Requires Speed Level 15";
    }
   }
}
