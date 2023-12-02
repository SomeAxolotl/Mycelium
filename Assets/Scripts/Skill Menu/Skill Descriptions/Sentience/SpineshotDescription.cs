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
    if (currentstats.sentienceLevel >= 5 && currentstats.skillEquippables["Spineshot"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Spineshot: <br><br> <size=25>Fire out a spine damaging the first enemy hit.";
    }
    else if (currentstats.sentienceLevel >= 5 && currentstats.skillEquippables["Spineshot"] == false && nutrientTracker.currentNutrients >= skillpurchase.spineshotNutrientCost && nutrientTracker.storedCalcite >= skillpurchase.spineshotMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Spineshot: <br><br> <size=25>Fire out a spine damaging the first enemy hit. <br><br> <color=#38BC0F>Cost: 2500 Nutrients and 1 Calcite";
    }
    else if (currentstats.sentienceLevel >= 5 && currentstats.skillEquippables["Spineshot"] == false)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Spineshot: <br><br> <size=25>Fire out a spine damaging the first enemy hit. <br><br> <color=#FF534C>Cost: 2500 Nutrients and 1 Calcite";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Spineshot: <br><br> <size=25>Fire out a spine damaging the first enemy hit. <br><br> <color=#FF534C>Purchasable at Sentience Level 5";
    }
   }
}
