using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class SpeciesSkill : MonoBehaviour, ISelectHandler
{
    public TMP_Text SkillDesc;
    public GameObject SkillDescriptionPanel;
    public CharacterStats currentstats;
    public SkillManager skillManager;
    // Start is called before the first frame update
    void OnEnable()
    {
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
        Descriptions();
    }
    public void OnSelect(BaseEventData eventData)
    {
           
        SkillDescriptionPanel.SetActive(true);
        Descriptions();
        
        
    }
    void Descriptions()
    {
        switch(currentstats.equippedSkills[0])
        { 
            case "FungalMight":
                SkillDesc.text = "Fungal Might: <br> <size=25>While active, your next attack or skill deals 75% increased damage. This ability does not stack.";
                break;
            case "Zombify":
                SkillDesc.text = "Turn the nearest enemy into a zombie and approach the nearest target. <br>Upon reaching a target your zombie will explode dealing damage to all enemies around it.";
                break;
            case "DeathBlossom":
                SkillDesc.text = "Plant a death cap mushroom infront of you. <After a short delay the mushroom will explode <br>poisoning all enemies caught in its raidius.";
                break;
            case "FairyRing":
                SkillDesc.text = "A ring of coral sprouts around you. Enemies standing in the ring are slowed and take additional damage over time.";
                break;
        }
         
    }
}
