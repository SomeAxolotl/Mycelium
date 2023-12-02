using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class UnstablePuffballDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.sentienceLevel >= 10 && currentstats.skillEquippables["UnstablePuffball"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Unstable Puffball: <br><br><size=25>Fires a puffball that explodes and damages all enemies upon contact.";
    }
    else if (currentstats.sentienceLevel >= 10 && currentstats.skillEquippables["UnstablePuffball"] == false && nutrientTracker.currentNutrients >= skillpurchase.puffballNutrientCost && nutrientTracker.storedCalcite >= skillpurchase.puffballMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Unstable Puffball: <br><size=25>Fires a puffball that explodes and damages all enemies upon contact. <br><br> <color=#38BC0F>Cost: 5000 Nutrients and 2 Calcite";
    }
    else if (currentstats.sentienceLevel >= 10 && currentstats.skillEquippables["UnstablePuffball"] == false)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Unstable Puffball: <br><size=25>Fires a puffball that explodes and damages all enemies upon contact. <br><br> <color=#FF534C>Cost: 5000 Nutrients and 2 Calcite";
    }
    else 
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Unstable Puffball: <br><size=25>Fires a puffball that explodes and damages all enemies upon contact. <br><br> <color=#FF534C>Purchasable at Sentience Level 10";
    }
   }
}
