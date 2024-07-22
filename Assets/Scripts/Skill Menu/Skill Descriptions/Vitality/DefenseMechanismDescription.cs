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
   public PurchaseSkills skillpurchase;
   public NutrientTracker nutrientTracker;  


   void OnEnable()
   {
    currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
    nutrientTracker = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();
    skillpurchase = GameObject.FindWithTag("MenuManager").GetComponent<PurchaseSkills>();
   }
   public void OnSelect(BaseEventData eventData)
   {
    if (currentstats.vitalityLevel >= 15 && currentstats.skillEquippables["DefenseMechanism"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Defense Mechanism: <br><size=25>Defend yourself for 1 second. During this time convert all damage taken into 50% healing. Force enemies off once the duration ends with an explosion.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Defense Mechanism: <br><size=22>Defend yourself for 1 second. During this time convert all damage taken into 50% healing. Force enemies off once the duration ends with an explosion.<br> <color=#FF534C>Unlocks at Vitality Level 15";
    }
   }
}
