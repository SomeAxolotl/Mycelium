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
    if (currentstats.primalLevel >= 5  && currentstats.skillEquippables["Eruption"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Eruption: <br> <size=25>Stomp the ground with primal strength <br> dealing damage to all enemies around you.<br> Deals additional damage to enemies closer to you.";
    }
    
    else if(currentstats.primalLevel >= 5 && currentstats.skillEquippables["Eruption"] == false && nutrientTracker.currentNutrients >= skillpurchase.eruptionNutrientCost && nutrientTracker.storedLog >= skillpurchase.eruptionMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Eruption: <br> <size=25>Stomp the ground with primal strength <br> dealing damage to all enemies around you.<br> Deals additional damage to enemies closer to you.<br> <color=#38BC0F>Cost: 2500 Nutrients and 1 Log";
    }
    else if (currentstats.primalLevel >= 5 && currentstats.skillEquippables["Eruption"] == false)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Eruption: <br> <size=25>Stomp the ground with primal strength <br> dealing damage to all enemies around you.<br> Deals additional damage to enemies closer to you.<br> <color=#FF534C>Cost: 2500 Nutrients and 1 Log";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Eruption: <br> <size=25>Stomp the ground with primal strength <br> dealing damage to all enemies around you.<br> Deals additional damage to enemies closer to you.<br> <color=#FF534C>Purchasable at Primal Level 5";
    }
   }
}
