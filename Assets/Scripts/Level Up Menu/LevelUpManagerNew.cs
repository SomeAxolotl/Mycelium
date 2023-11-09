using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class LevelUpManagerNew : MonoBehaviour
{
    public TMP_Text skillcdr;
    public TMP_Text primaldam;
    public TMP_Text skilldam;
    public TMP_Text movespeed;
    public TMP_Text regen;
    public TMP_Text health;
    public TMP_Text Nutrients;
    public TMP_Text LevelUpCost;
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
    public Button Commitbutton;
    public TMP_Text CurrentLevel;
    public int PrimalSave;
    public int SpeedSave;
    public int SentienceSave;
    public int VitalitySave;
    public int nutrientsSave;
    private float healthsave;
    private float regensave;
    private float movespeedsave;
    private float damagesave;
    private float skilldmgsave;
    private float cdrsave;
    private int totalLevelsave;
    private int levelupsave;
    private CanvasGroup HUDCanvasGroup;
    private NewPlayerHealth playerHealth;
    public CharacterStats currentstats;
    private PlayerController playerController;
    public NutrientTracker currentnutrients;
    private SkillManager skillManager;
    private HUDSkills hudSkills;
   
  
    void Start()
    {
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<NewPlayerHealth>();
        skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
    }
    void OnEnable()
    {
       currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
       playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
       PrimalSave = currentstats.primalLevel;
       SpeedSave = currentstats.speedLevel;
       SentienceSave = currentstats.sentienceLevel;
       VitalitySave = currentstats.vitalityLevel;
       nutrientsSave = currentnutrients.currentNutrients;
       healthsave = currentstats.baseHealth;
       regensave = currentstats.baseRegen;
       movespeedsave = currentstats.moveSpeed;
       damagesave = currentstats.primalDmg;
       skilldmgsave = currentstats.skillDmg;
       cdrsave = currentstats.atkCooldownBuff;
       totalLevelsave = currentstats.totalLevel;
       levelupsave = currentstats.levelUpCost;
       Commitbutton.Select();
       PrimalStartCheck();
       SpeedStartCheck();
       VitalityStartCheck();
       SentienceStartCheck();
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
        LevelUpCost.text = currentstats.levelUpCost.ToString();
        Nutrients.text = currentnutrients.currentNutrients.ToString();
        CurrentLevel.text = currentstats.totalLevel.ToString();
        health.text = currentstats.baseHealth.ToString();
        regen.text = currentstats.baseRegen.ToString("0.00") + " HPS";
        movespeed.text = currentstats.moveSpeed.ToString("0.0") + " m/s";
        primaldam.text = currentstats.primalDmg.ToString();
        skilldam.text = currentstats.skillDmg.ToString();
        skillcdr.text = currentstats.atkCooldownBuff.ToString("0.00") + " seconds";
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
        if(currentstats.primalLevel == 1)
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
        if(currentstats.speedLevel == 1)
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

      //Testing GetAllSkillCooldowns
      GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
      List<float> allCooldowns = skillManager.GetAllSkillCooldowns(currentPlayer);
      for (int i = 0; i < allCooldowns.Count; i++)
      {
        Debug.Log("Cooldown " + i + ": " + allCooldowns[i].ToString("F2"));
      }
    }
    public void SentienceDown()
    {
      currentstats.DeLevelSentience(); 
      SentienceCheck();
      SentienceCap();

      //Testing GetAllSkillCooldowns
      GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
      List<float> allCooldowns = skillManager.GetAllSkillCooldowns(currentPlayer);
      //Debug
      for (int i = 0; i < allCooldowns.Count; i++)
      {
        Debug.Log("Cooldown " + i + ": " + allCooldowns[i].ToString("F2"));
      }
    }
    public void SentienceCheck()
    {
        if(currentstats.sentienceLevel == 1)
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
        if(currentstats.vitalityLevel == 1)
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
         playerHealth.ResetHealth();
         HUDCanvasGroup.alpha = 1;
    }
    public void Close()
    {
      currentstats.primalLevel = PrimalSave;
      currentstats.speedLevel = SpeedSave;
      currentstats.sentienceLevel = SentienceSave;
      currentstats.vitalityLevel = VitalitySave;
      currentnutrients.currentNutrients = nutrientsSave;
      currentstats.baseHealth = healthsave;
      currentstats.baseRegen = regensave;
      currentstats.moveSpeed = movespeedsave;
      currentstats.primalDmg = damagesave;
      currentstats.skillDmg = skilldmgsave;
      currentstats.atkCooldownBuff = cdrsave;
      currentstats.totalLevel = totalLevelsave;
      currentstats.levelUpCost = levelupsave;
      currentstats.UpdateLevel();
      UIenable.SetActive(false);
      HUDCanvasGroup.alpha = 1;

    }
    public void PrimalStartCheck()
    {
        if(currentstats.primalLevel == 1)
        {
        PrimalLevelDown.interactable = false;
        }
        else 
        {
        PrimalLevelDown.interactable = true;
        }
    }
    public void SpeedStartCheck()
    {
        if(currentstats.speedLevel == 1)
        {
        SpeedLevelDown.interactable = false;
        }
        else 
        {
        SpeedLevelDown.interactable = true;
        }
    }
    public void SentienceStartCheck()
    {
        if(currentstats.sentienceLevel == 1)
        {
        SentienceLevelDown.interactable = false;
        }
        else 
        {
        SentienceLevelDown.interactable = true;
        }
    }
     public void VitalityStartCheck()
    {
        if(currentstats.vitalityLevel == 1)
        {
        VitalityLevelDown.interactable = false;
        }
        else 
        {
        VitalityLevelDown.interactable = true;
        }
    }
}
