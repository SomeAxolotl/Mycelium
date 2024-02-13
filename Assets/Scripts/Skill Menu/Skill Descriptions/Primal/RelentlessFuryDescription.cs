using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class RelentlessFuryDescription : MonoBehaviour, ISelectHandler
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
    if (currentstats.primalLevel >= 15 && currentstats.skillEquippables["RelentlessFury"] == true)
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Relentless Fury: <br> <size=25>Go into a frenzy gaining 30% attack speed. While active lose 5% of current health every second. Attacks restore health equal to 25%<br> of weapon damage.";
    }
    else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text = "Relentless Fury: <br> <size=24>Go into a frenzy gaining 30% attack speed. While active lose 5% of current health every second. Attacks restore health equal to 25% of weapon damage.<br> <color=#FF534C>Unlocks at Primal Level 15";
    }
   }
}
