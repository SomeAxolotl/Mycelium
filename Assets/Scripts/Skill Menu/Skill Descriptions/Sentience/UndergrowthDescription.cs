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
    if (currentstats.sentienceLevel >= 15 && currentstats.skillEquippables["Undergrowth"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Undergrowth: <br><br><size=25> An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit.";
    }
    else if (currentstats.sentienceLevel >= 15 && currentstats.skillEquippables["Undergrowth"] == false && nutrientTracker.currentNutrients >= skillpurchase.undergrowthNutrientCost && nutrientTracker.storedCalcite >= skillpurchase.undergrowthMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Undergrowth: <br><size=25>An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit. <br><br> <color=#38BC0F>Cost: 10000 Nutrients and 3 Calcite";
    }
     else if (currentstats.sentienceLevel >= 15 && currentstats.skillEquippables["Undergrowth"] == false )
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Undergrowth: <br><size=25>An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit. <br><br> <color=#FF534C>Cost: 10000 Nutrients and 3 Calcite";
    }
     else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Undergrowth: <br><size=25>An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit. <br><br> <color=#FF534C>Purchasable at Sentience Level 15";
    }
   }
}
