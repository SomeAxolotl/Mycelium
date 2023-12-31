using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class LivingCycloneDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.primalLevel >= 10 && currentstats.skillEquippables["LivingCyclone"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Living Cyclone: <br> <size=25>Spin relentlessly striking all enemies<br> around you with your currently equipped weapon. <br> You are able to move while Living Cyclone is active.";
    }
    else if (currentstats.primalLevel >= 10 && currentstats.skillEquippables["LivingCyclone"] == false && nutrientTracker.currentNutrients >= skillpurchase.cycloneNutrientCost && nutrientTracker.storedLog >= skillpurchase.cycloneMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Living Cyclone: <br> <size=25>Spin relentlessly striking all enemies<br> around you with your currently equipped weapon. <br> You are able to move while Living Cyclone is active.<br> <color=#38BC0F>Cost: 5000 Nutrients and 2 Logs";
    }
     else if (currentstats.primalLevel >= 10 && currentstats.skillEquippables["LivingCyclone"] == false)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Living Cyclone: <br> <size=25>Spin relentlessly striking all enemies<br> around you with your currently equipped weapon. <br> You are able to move while Living Cyclone is active.<br> <color=#FF534C>Cost: 5000 Nutrients and 2 Logs";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Living Cyclone: <br> <size=25>Spin relentlessly striking all enemies<br> around you with your currently equipped weapon. <br> You are able to move while Living Cyclone is active.<br> <color=#FF534C>Purchasable at Primal Level 10";
    }
   }
}
