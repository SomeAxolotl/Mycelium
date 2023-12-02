using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class MycotoxinsDescriptions : MonoBehaviour, ISelectHandler
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
    if (currentstats.speedLevel >= 15 && currentstats.skillEquippables["Mycotoxins"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Mycotoxins: <br> <size=25>Gain 50% bonus movement speed and release a trail of spores behind you. Enemies hit by these spores are damaged.";
    }
    else if (currentstats.speedLevel >= 15 && currentstats.skillEquippables["Mycotoxins"] == false && nutrientTracker.currentNutrients >= skillpurchase.mycotoxinsNutrientCost && nutrientTracker.storedExoskeleton >= skillpurchase.mycotoxinsMaterialCost)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Mycotoxins: <br> <size=25>Gain 50% bonus movement speed and release a trail of spores behind you. Enemies hit by these spores are damaged. <br> <color=#38BC0F>Cost: 10000 Nutrients and 3 Exoskeletons";
    }
    else if (currentstats.speedLevel >= 15 && currentstats.skillEquippables["Mycotoxins"] == false )
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Mycotoxins: <br> <size=25>Gain 50% bonus movement speed and release a trail of spores behind you. Enemies hit by these spores are damaged. <br> <color=#FF534C>Cost: 10000 Nutrients and 3 Exoskeletons";
    }
    else 
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Mycotoxins: <br> <size=25>Gain 50% bonus movement speed and release a trail of spores behind you. Enemies hit by these spores are damaged. <br> <color=#FF534C>Purchasable at Speed Level 15";
    }
   }
}
