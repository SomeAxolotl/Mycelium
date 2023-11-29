using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class SkillMenuUnlocks : MonoBehaviour
{
    //Eruption Buttons
    public Button EruptionUnlocked;
    //LC Buttons
    public Button LivingCycloneUnlocked;
    //RF Buttons
    public Button RelentlessFuryUnlocked;
    //Blitz Buttons
    public Button BlitzUnlocked;
    //TC Buttons
    public Button TrophicCascadeUnlocked;
    //Mycotoxins Buttons
    public Button MycotoxinsUnlocked;
    //Spineshot Buttons
    public Button SpineshotUnlocked;
    //UPB Buttons
    public Button UnstablePuffBallUnlocked;

    //UG Buttons
    public Button UndergrowthUnlocked;

    //LS Buttons
    public Button LeechingSporesUnlocked;

    //SB Buttons
    public Button SporeburstUnlocked;

    //DM Buttons
    public Button DefenseMechanismUnlocked;

    public CharacterStats currentstats;
    public SkillMenuManager skillmenumanager;
    public NutrientTracker nutrientTracker;
    void Start()
    {
        
    }
    void OnEnable()
    {
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        //Primal Unlock Statements
        if(currentstats.primalLevel >= 5 && currentstats.skillEquippables["Eruption"] == true)
        {
            EruptionUnlocked.onClick.AddListener(skillmenumanager.EruptionsSlot1);
        }
        else
        {
            EruptionUnlocked.onClick.RemoveListener(skillmenumanager.EruptionsSlot1);
        }


        if (currentstats.primalLevel >= 10 && currentstats.skillEquippables["LivingCyclone"] == true)
        {
            LivingCycloneUnlocked.onClick.AddListener(skillmenumanager.LivingCycloneSlot1);
        }
        else
        {
            LivingCycloneUnlocked.onClick.RemoveListener(skillmenumanager.LivingCycloneSlot1);
        }

        
        if(currentstats.primalLevel >= 15 && currentstats.skillEquippables["RelentlessFury"] == true)
        {
            RelentlessFuryUnlocked.onClick.AddListener(skillmenumanager.RelentlessFurySlot1);
        }
        else
        {
            RelentlessFuryUnlocked.onClick.RemoveListener(skillmenumanager.RelentlessFurySlot1);
        }

        
        //Speed Unlock Statements
        if(currentstats.speedLevel >= 5 && currentstats.skillEquippables["Blitz"] == true)
        {
            BlitzUnlocked.onClick.AddListener(skillmenumanager.BlitzSlot1);
        }
        else
        {
            BlitzUnlocked.onClick.RemoveListener(skillmenumanager.BlitzSlot1);
        }

        
        if(currentstats.speedLevel >= 10 && currentstats.skillEquippables["TrophicCascade"] == true)
        {
            TrophicCascadeUnlocked.onClick.AddListener(skillmenumanager.TrophicCascadeSlot1);
        }
        else
        {
            TrophicCascadeUnlocked.onClick.RemoveListener(skillmenumanager.TrophicCascadeSlot1);
        }


        if(currentstats.speedLevel >= 15 && currentstats.skillEquippables["Mycotoxins"] == true)
        {
            MycotoxinsUnlocked.onClick.AddListener(skillmenumanager.MycotoxinsSlot1);
        }
        else
        {
            MycotoxinsUnlocked.onClick.RemoveListener(skillmenumanager.MycotoxinsSlot1);
        }


        //Sentience Unlock Statements
        if(currentstats.sentienceLevel >= 5 && currentstats.skillEquippables["Spineshot"] == true)
        {
            SpineshotUnlocked.onClick.AddListener(skillmenumanager.SpineShotSlot1);
        }
        else
        {
            SpineshotUnlocked.onClick.RemoveListener(skillmenumanager.SpineShotSlot1);
        }

        if(currentstats.sentienceLevel >= 10 && currentstats.skillEquippables["UnstablePuffball"] == true)
        {
            UnstablePuffBallUnlocked.onClick.AddListener(skillmenumanager.UnstablePuffballSlot1);
        }
        else
        {
            UnstablePuffBallUnlocked.onClick.RemoveListener(skillmenumanager.UnstablePuffballSlot1);
        }

        if(currentstats.sentienceLevel >= 15 && currentstats.skillEquippables["Undergrowth"] == true)
        {
            UndergrowthUnlocked.onClick.AddListener(skillmenumanager.UndergrowthSlot1);
        }
        else
        {
            UndergrowthUnlocked.onClick.RemoveListener(skillmenumanager.UndergrowthSlot1);
        }

        
        //Vitality unlock statements
        if(currentstats.vitalityLevel >= 5 && currentstats.skillEquippables["LeechingSpore"] == true)
        {
            LeechingSporesUnlocked.onClick.AddListener(skillmenumanager.LeechingSporeSlot1);
        }
        else
        {
            LeechingSporesUnlocked.onClick.RemoveListener(skillmenumanager.LeechingSporeSlot1);
        }

        if(currentstats.vitalityLevel >= 10 && currentstats.skillEquippables["Sporeburst"] == true)
        {
            SporeburstUnlocked.onClick.AddListener(skillmenumanager.SporeBurstSlot1);
        }
        else
        {
            SporeburstUnlocked.onClick.RemoveListener(skillmenumanager.SporeBurstSlot1);
        }

        if(currentstats.vitalityLevel >= 15 && currentstats.skillEquippables["DefenseMechanism"] == true)
        {
            DefenseMechanismUnlocked.onClick.AddListener(skillmenumanager.DefenseMechanismSlot1);
        }
        else
        {
            DefenseMechanismUnlocked.onClick.RemoveListener(skillmenumanager.DefenseMechanismSlot1);
        }
    }
    

    //Skill Descriptions

    
}
