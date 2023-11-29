using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class SkillMenuUnlocks2 : MonoBehaviour
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
    void Start()
    {
        
    }
    void OnEnable()
    {
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        //Primal Unlock Statements
        if(currentstats.primalLevel >= 5 && currentstats.skillEquippables["Eruption"] == true)
        {
            EruptionUnlocked.onClick.AddListener(skillmenumanager.EruptionsSlot2);
        }
        else
        {
            EruptionUnlocked.onClick.RemoveListener(skillmenumanager.EruptionsSlot2);
        }

        
        if(currentstats.primalLevel >= 10 && currentstats.skillEquippables["LivingCyclone"] == true)
        {
            LivingCycloneUnlocked.onClick.AddListener(skillmenumanager.LivingCycloneSlot2);
        }
        else
        {
            LivingCycloneUnlocked.onClick.RemoveListener(skillmenumanager.LivingCycloneSlot2);
        }

        
        if(currentstats.primalLevel >= 15 && currentstats.skillEquippables["RelentlessFury"] == true)
        {
            RelentlessFuryUnlocked.onClick.AddListener(skillmenumanager.RelentlessFurySlot2);
        }
        else
        {
            RelentlessFuryUnlocked.onClick.RemoveListener(skillmenumanager.RelentlessFurySlot2);
        }

        
        //Speed Unlock Statements
        if(currentstats.speedLevel >= 5 && currentstats.skillEquippables["Blitz"] == true)
        {
            BlitzUnlocked.onClick.AddListener(skillmenumanager.BlitzSlot2);
        }
        else
        {
            BlitzUnlocked.onClick.RemoveListener(skillmenumanager.BlitzSlot2);
        }

        
        if(currentstats.speedLevel >= 10 && currentstats.skillEquippables["TrophicCascade"] == true)
        {
            TrophicCascadeUnlocked.onClick.AddListener(skillmenumanager.TrophicCascadeSlot2);
        }
        else
        {
            TrophicCascadeUnlocked.onClick.RemoveListener(skillmenumanager.TrophicCascadeSlot2);
        }


        if(currentstats.speedLevel >= 15 && currentstats.skillEquippables["Mycotoxins"] == true)
        {
            MycotoxinsUnlocked.onClick.AddListener(skillmenumanager.MycotoxinsSlot2);
        }
        else
        {
            MycotoxinsUnlocked.onClick.RemoveListener(skillmenumanager.MycotoxinsSlot2);
        }


        //Sentience Unlock Statements
        if(currentstats.sentienceLevel >= 5 && currentstats.skillEquippables["Spineshot"] == true)
        {
            SpineshotUnlocked.onClick.AddListener(skillmenumanager.SpineShotSlot2);
        }
        else
        {
            SpineshotUnlocked.onClick.RemoveListener(skillmenumanager.SpineShotSlot2);
        }

        if(currentstats.sentienceLevel >= 10 && currentstats.skillEquippables["UnstablePuffball"] == true)
        {
            UnstablePuffBallUnlocked.onClick.AddListener(skillmenumanager.UnstablePuffballSlot2);
        }
        else
        {
            UnstablePuffBallUnlocked.onClick.RemoveListener(skillmenumanager.UnstablePuffballSlot2);
        }

        if(currentstats.sentienceLevel >= 15 && currentstats.skillEquippables["Undergrowth"] == true)
        {
            UndergrowthUnlocked.onClick.AddListener(skillmenumanager.UndergrowthSlot2);
        }
        else
        {
            UndergrowthUnlocked.onClick.RemoveListener(skillmenumanager.UndergrowthSlot2);
        }

        
        //Vitality unlock statements
        if(currentstats.vitalityLevel >= 5 && currentstats.skillEquippables["LeechingSpore"] == true)
        {
            LeechingSporesUnlocked.onClick.AddListener(skillmenumanager.LeechingSporeSlot2);
        }
        else
        {
            LeechingSporesUnlocked.onClick.RemoveListener(skillmenumanager.LeechingSporeSlot2);
        }

        if(currentstats.vitalityLevel >= 10 && currentstats.skillEquippables["Sporeburst"] == true)
        {
            SporeburstUnlocked.onClick.AddListener(skillmenumanager.SporeBurstSlot2);
        }
        else
        {
            SporeburstUnlocked.onClick.RemoveListener(skillmenumanager.SporeBurstSlot2);
        }

        if(currentstats.vitalityLevel >= 15 && currentstats.skillEquippables["DefenseMechanism"] == true)
        {
            DefenseMechanismUnlocked.onClick.AddListener(skillmenumanager.DefenseMechanismSlot2);
        }
        else
        {
            DefenseMechanismUnlocked.onClick.RemoveListener(skillmenumanager.DefenseMechanismSlot2);
        }
    }

}
