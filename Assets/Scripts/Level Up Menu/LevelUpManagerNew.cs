using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class LevelUpManagerNew : MonoBehaviour
{
    public TMP_Text PrimalText;
    public TMP_Text SpeedText;
    public TMP_Text SentienceText;
    public TMP_Text VitalityText;
    public GameObject UIenable;
    public Button PrimalLevelUp;
    public Button PrimalLevelDown;
    public Button SpeedLevelUp;
    public Button SpeedLevelDown;
    public Button SentienceLevelUp;
    public Button SentienceLevelDown;
     public Button VitalityLevelUp;
    public Button VitalityLevelDown;
    public GameObject HUD;

   public CharacterStats currentstats;
   private PlayerController playerController;
  
    void Start()
    {
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        PrimalCheck();
        SpeedCheck();
        VitalityCheck();
        SentienceCheck();
        PrimalCap();
        
    }

    void Update()
    {
        UpdateUI();
        
    }
    void UpdateUI()
    {
        
        PrimalText.text = currentstats.primalLevel.ToString();
        SpeedText.text = currentstats.speedLevel.ToString(); 
        SentienceText.text = currentstats.sentienceLevel.ToString();
        VitalityText.text = currentstats.vitalityLevel.ToString(); 
    }

    public void PrimalUP()
    {
      currentstats.LevelPrimal();
      PrimalCap();
      PrimalCheck();
    }
    public void PrimalDown()
    {
      currentstats.DeLevelPrimal(); 
      PrimalCheck();
      PrimalCap();
    }
    public void PrimalCheck()
    {
        if(currentstats.primalLevel == 0)
        {
        PrimalLevelDown.interactable = false;
        PrimalLevelUp.Select();
        }
        else 
        {
        PrimalLevelDown.interactable = true;
        }
    }
     public void PrimalCap()
    {
      if(currentstats.primalLevel == 25)
        {
        PrimalLevelUp.interactable = false;
        PrimalLevelDown.Select();
        }
        else 
        {
        PrimalLevelUp.interactable = true;
        }
    }
    public void SpeedUP()
    {
      currentstats.LevelSpeed();
      SpeedCheck();
      SpeedCap();
    }
    public void SpeedDown()
    {
      currentstats.DeLevelSpeed(); 
      SpeedCheck();
      SpeedCap();
    }
    public void SpeedCheck()
    {
        if(currentstats.speedLevel == 0)
        {
        SpeedLevelDown.interactable = false;
        SpeedLevelUp.Select();
        }
        else 
        {
        SpeedLevelDown.interactable = true;
        }
    }
     public void SpeedCap()
    {
      if(currentstats.speedLevel == 25)
        {
        SpeedLevelUp.interactable = false;
        SpeedLevelDown.Select();
        }
        else 
        {
        SpeedLevelUp.interactable = true;
        }
    }
    public void SentienceUP()
    {
      currentstats.LevelSentience();
      SentienceCheck();
      SentienceCap();
    }
    public void SentienceDown()
    {
      currentstats.DeLevelSentience(); 
      SentienceCheck();
      SentienceCap();
    }
    public void SentienceCheck()
    {
        if(currentstats.sentienceLevel == 0)
        {
        SentienceLevelDown.interactable = false;
        SentienceLevelUp.Select();
        }
        else 
        {
        SentienceLevelDown.interactable = true;
        }
    }
     public void SentienceCap()
    {
      if(currentstats.sentienceLevel == 25)
        {
        SentienceLevelUp.interactable = false;
        SentienceLevelDown.Select();
        }
        else 
        {
        SentienceLevelUp.interactable = true;
        }
    }
    public void VitalityUP()
    {
      currentstats.LevelVitality();
      VitalityCheck();
      VitalityCap();
    }
    public void VitalityDown()
    {
      currentstats.DeLevelVitality(); 
      VitalityCheck();
      VitalityCap();
    }
    public void VitalityCheck()
    {
        if(currentstats.vitalityLevel == 0)
        {
        VitalityLevelDown.interactable = false;
        VitalityLevelUp.Select();
        }
        else 
        {
        VitalityLevelDown.interactable = true;
        }
    }
    public void VitalityCap()
    {
      if(currentstats.vitalityLevel == 25)
        {
        VitalityLevelUp.interactable = false;
        VitalityLevelDown.Select();
        }
        else 
        {
        VitalityLevelUp.interactable = true;
        }
    }
    public void Commit()
    {
         UIenable.SetActive(false);
         playerController.EnableController();
         HUD.SetActive(true);
    }
}
