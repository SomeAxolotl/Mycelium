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
    if (currentstats.speedLevel >= 10 && currentstats.skillEquippables["TrophicCascade"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Trophic Cascade: <br> <size=25>Release a flurry of attacks slashing<br> all enemies around you.";
    }
    else if (currentstats.speedLevel >= 10 && currentstats.skillEquippables["TrophicCascade"] == false && nutrientTracker.currentNutrients >= skillpurchase.cascadeNutrientCost && nutrientTracker.storedExoskeleton >= skillpurchase.cascadeMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Trophic Cascade: <br> <size=25>Release a flurry of attacks slashing<br> all enemies around you.<br><br> <color=#38BC0F>Cost: 5000 Nutrients and 2 Exoskeletons";
    }
    else if (currentstats.speedLevel >= 10 && currentstats.skillEquippables["TrophicCascade"] == false)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Trophic Cascade: <br> <size=25>Release a flurry of attacks slashing<br> all enemies around you.<br><br> <color=#FF534C>Cost: 5000 Nutrients and 2 Exoskeletons";
    }
    else 
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Trophic Cascade: <br> <size=25>Release a flurry of attacks slashing<br> all enemies around you.<br><br> <color=#FF534C>Purchasable at Speed Level 10";
    }
   }
}
