using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class SpineshotDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.sentienceLevel >= 5)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Spineshot: <br><br> <size=25>Fire out a spine damaging the first enemy hit.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Spineshot: <br><br> <size=25>Fire out a spine damaging the first enemy hit. <br><br> <color=#FF534C>Requires Sentience Level 5";
    }
   }
}
