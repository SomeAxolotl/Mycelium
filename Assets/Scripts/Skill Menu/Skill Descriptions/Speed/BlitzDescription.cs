using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class BlitzDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.speedLevel >= 5 && currentstats.skillEquippables["Blitz"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Blitz: <br> <size=25>Dash in a straight line damaging all enemies hit.<br> If an enemy is hit the cooldown of Blitz<br> is reduced by 50%.";
    }
    else if(currentstats.speedLevel >= 5 && currentstats.skillEquippables["Blitz"] == false && nutrientTracker.currentNutrients >= skillpurchase.blitzNutrientCost && nutrientTracker.storedExoskeleton >= skillpurchase.blitzMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Blitz: <br> <size=25>Dash in a straight line damaging all enemies hit.<br> If an enemy is hit the cooldown of Blitz<br> is reduced by 50%.<br> <color=#38BC0F>Cost: 2500 Nutrients and 1 Exoskeleton";
    }
    else if(currentstats.speedLevel >= 5 && currentstats.skillEquippables["Blitz"] == false)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Blitz: <br> <size=25>Dash in a straight line damaging all enemies hit.<br> If an enemy is hit the cooldown of Blitz<br> is reduced by 50%.<br> <color=#FF534C>Cost: 2500 Nutrients and 1 Exoskeleton";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Blitz: <br> <size=25>Dash in a straight line damaging all enemies hit.<br> If an enemy is hit the cooldown of Blitz<br> is reduced by 50%.<br> <color=#FF534C>Purchasable at Speed Level 5";
    }
   }
}
