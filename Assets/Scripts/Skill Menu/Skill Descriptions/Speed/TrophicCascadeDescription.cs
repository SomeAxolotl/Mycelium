using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class TrophicCascadeDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.speedLevel >= 10)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Trophic Cascade: <br> <size=25>Release a flurry of attacks slashing<br> all enemies around you.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Trophic Cascade: <br> <size=25>Release a flurry of attacks slashing<br> all enemies around you.<br><br> <color=#FF534C>Requires Speed Level 10";
    }
   }
}
