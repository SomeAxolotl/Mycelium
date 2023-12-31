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
        SkillDesc.text = "Defense Mechanism: <br><size=25>Reduces damage taken by 50% for 1 second. Attacks against you while Defense Mechanism is active is stored as bonus damage on your next attack equal to 50% of the damage absorbed.";
    }
    else if(currentstats.vitalityLevel >= 15 && currentstats.skillEquippables["DefenseMechanism"] == false && nutrientTracker.currentNutrients >= skillpurchase.defenseMechanismNutrientCost && nutrientTracker.storedFlesh >= skillpurchase.defenseMechanismMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Defense Mechanism: <br><size=22>Reduces damage taken by 50% for 1 second. Attacks against you while Defense Mechanism is active is stored as bonus damage on your next attack equal to 50% of the damage absorbed.<br> <color=#38BC0F>Cost: 10000 Nutrients and 3 Flesh";
    }
    else if(currentstats.vitalityLevel >= 15 && currentstats.skillEquippables["DefenseMechanism"] == false)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Defense Mechanism: <br><size=22>Reduces damage taken by 50% for 1 second. Attacks against you while Defense Mechanism is active is stored as bonus damage on your next attack equal to 50% of the damage absorbed.<br> <color=#FF534C>Cost: 10000 Nutrients and 3 Flesh";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Defense Mechanism: <br><size=22>Reduces damage taken by 50% for 1 second. Attacks against you while Defense Mechanism is active is stored as bonus damage on your next attack equal to 50% of the damage absorbed.<br> <color=#FF534C>Purchasable at Vitality Level 15";
    }
   }
}
