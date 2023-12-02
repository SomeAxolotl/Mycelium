using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class LeechingSporeDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.vitalityLevel >= 5 && currentstats.skillEquippables["LeechingSpore"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Leeching Spores: <br><br> <size=25>Infest a nearby enemy with a leeching spore. The spore steals health every second from the enemy and restores it to you.";
    }
    else if(currentstats.vitalityLevel >= 5 && currentstats.skillEquippables["LeechingSpore"] == false && nutrientTracker.currentNutrients >= skillpurchase.leechingSporeNutrientCost && nutrientTracker.storedFlesh >= skillpurchase.leechingSporeMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Leeching Spores: <br><size=25>Infest a nearby enemy with a leeching spore. The spore steals health every second from the enemy and restores it to you.<br> <color=#38BC0F>Cost: 2500 Nutrients and 1 Flesh";
    }
    else if(currentstats.vitalityLevel >= 5 && currentstats.skillEquippables["LeechingSpore"] == false)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Leeching Spores: <br><size=25>Infest a nearby enemy with a leeching spore. The spore steals health every second from the enemy and restores it to you.<br> <color=#FF534C>Cost: 2500 Nutrients and 1 Flesh";
    }
    else 
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Leeching Spores: <br><size=25>Infest a nearby enemy with a leeching spore. The spore steals health every second from the enemy and restores it to you.<br> <color=#FF534C>Purchasable at Vitality Level 5";
    }
   }
}
