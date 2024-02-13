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
     else
    {
        SkillDescriptionPanel.SetActive(true);
        SkillDesc.text ="Undergrowth: <br><size=25>An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit. <br><br> <color=#FF534C>Unlocks at Sentience Level 15";
    }
   }
}
