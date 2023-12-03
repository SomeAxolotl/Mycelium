using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class LevelUpManagerNew : MonoBehaviour
{
    [SerializeField] private int maxStatLevel = 15;

    //public TMP_Text skillcdr;
    public TMP_Text primaldam;
    //public TMP_Text skilldam;
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
    public Button Closebutton;
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
    //private float skilldmgsave;
    //private float cdrsave;
    private int totalLevelsave;
    private int levelupsave;
    private CanvasGroup HUDCanvasGroup;
    private NewPlayerHealth playerHealth;
    public CharacterStats currentstats;
    private PlayerController playerController;
    public NutrientTracker currentnutrients;
    private SkillManager skillManager;
    public GameObject SkillMenu;
    private HUDSkills hudSkills;
    ThirdPersonActionsAsset controls;
    [SerializeField] private SkillUnlockNotifications skillUnlockNotifications;
    List<string> newlyUnlockedSkills = new List<string>();
    public GameObject GrowMenu;
   
  
    void Start()
    {
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<NewPlayerHealth>();
        skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
        StartCoroutine(UpdateUI());
    }

    void OnEnable()
    {
      currentnutrients = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();
      controls = new ThirdPersonActionsAsset();
      controls.UI.MenuSwapR.started += ctx => MenuSwap();
      controls.UI.MenuSwapL.started += ctx => MenuSwapL();
      controls.UI.Close.performed += ctx => Close();
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
       //skilldmgsave = currentstats.skillDmg;
       //cdrsave = currentstats.atkCooldownBuff;
       totalLevelsave = currentstats.totalLevel;
       levelupsave = currentstats.levelUpCost;
       Commitbutton.Select();
       PrimalStartCheck();
       SpeedStartCheck();
       VitalityStartCheck();
       SentienceStartCheck();
       StartCoroutine(UpdateUI());
       SkillMenu.SetActive(false);
       GrowMenu.SetActive(false);
       Invoke("ControlEnable", 0.25f); 


    }
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.LeftBracket))
      {
        MenuSwapL();
      }
      if(Input.GetKeyDown(KeyCode.RightBracket))
      {
        MenuSwap();
      }
    }
    void ControlEnable()
    {
       controls.UI.Enable();  
    }
    void OnDisable()
    {
      controls.UI.Disable();
    }
     IEnumerator UpdateUI()
    {
        
        yield return null;       
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
        //skilldam.text = currentstats.skillDmg.ToString();
        //skillcdr.text = currentstats.atkCooldownBuff.ToString("0.00") + " seconds";
    }
    void MenuSwap()
    {
      SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("MainCamera").transform.position);
      currentstats.primalLevel = PrimalSave;
      currentstats.speedLevel = SpeedSave;
      currentstats.sentienceLevel = SentienceSave;
      currentstats.vitalityLevel = VitalitySave;
      currentnutrients.currentNutrients = nutrientsSave;
      currentstats.baseHealth = healthsave;
      currentstats.baseRegen = regensave;
      currentstats.moveSpeed = movespeedsave;
      currentstats.primalDmg = damagesave;
      //currentstats.skillDmg = skilldmgsave;
      //currentstats.atkCooldownBuff = cdrsave;
      currentstats.totalLevel = totalLevelsave;
      currentstats.levelUpCost = levelupsave;
      currentstats.UpdateLevel();
      SkillMenu.SetActive(true);
    }
    void MenuSwapL()
    {
      SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("MainCamera").transform.position);
      currentstats.primalLevel = PrimalSave;
      currentstats.speedLevel = SpeedSave;
      currentstats.sentienceLevel = SentienceSave;
      currentstats.vitalityLevel = VitalitySave;
      currentnutrients.currentNutrients = nutrientsSave;
      currentstats.baseHealth = healthsave;
      currentstats.baseRegen = regensave;
      currentstats.moveSpeed = movespeedsave;
      currentstats.primalDmg = damagesave;
      //currentstats.skillDmg = skilldmgsave;
      //currentstats.atkCooldownBuff = cdrsave;
      currentstats.totalLevel = totalLevelsave;
      currentstats.levelUpCost = levelupsave;
      currentstats.UpdateLevel();
      GrowMenu.SetActive(true); 
    }

    public void PrimalUP()
    {
      currentstats.LevelPrimal();
      PrimalCap();
      PrimalCheck();
      StartCoroutine(UpdateUI());
    }
    public void PrimalDown()
    {
      currentstats.DeLevelPrimal(); 
      PrimalCheck();
      PrimalCap();
      StartCoroutine(UpdateUI());
    }
    public void PrimalCheck()
    {
        if(currentstats.primalLevel == PrimalSave)
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
      if(currentstats.primalLevel == maxStatLevel)
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
      StartCoroutine(UpdateUI());
    }
    public void SpeedDown()
    {
      currentstats.DeLevelSpeed(); 
      SpeedCheck();
      SpeedCap();
      StartCoroutine(UpdateUI());
    }
    public void SpeedCheck()
    {
        if(currentstats.speedLevel == SpeedSave)
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
      if(currentstats.speedLevel == maxStatLevel)
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
      StartCoroutine(UpdateUI());

      //Testing GetEquippedSkillValues
      GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
      List<float> allCooldowns = skillManager.GetEquippedSkillCooldowns(currentPlayer);
      /*for (int i = 0; i < allCooldowns.Count; i++)
      {
        Debug.Log("Cooldown " + i + ": " + allCooldowns[i].ToString("F2"));
      }*/
    }
    public void SentienceDown()
    {
      currentstats.DeLevelSentience(); 
      SentienceCheck();
      SentienceCap();
      StartCoroutine(UpdateUI());

      //Testing GetEquippedSkillCooldowns
      GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
      List<float> allCooldowns = skillManager.GetEquippedSkillCooldowns(currentPlayer);
      //Debug
      /*for (int i = 0; i < allCooldowns.Count; i++)
      {
        Debug.Log("Cooldown " + i + ": " + allCooldowns[i].ToString("F2"));
      }*/
    }
    public void SentienceCheck()
    {
        if(currentstats.sentienceLevel == SentienceSave)
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
      if(currentstats.sentienceLevel == maxStatLevel)
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
      StartCoroutine(UpdateUI());
    }
    public void VitalityDown()
    {
      currentstats.DeLevelVitality(); 
      VitalityCheck();
      VitalityCap();
      StartCoroutine(UpdateUI());
    }
    public void VitalityCheck()
    {
        if(currentstats.vitalityLevel == VitalitySave)
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
      if(currentstats.vitalityLevel == maxStatLevel)
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
      PrimalSave = currentstats.primalLevel;
      SpeedSave = currentstats.speedLevel;
      SentienceSave = currentstats.sentienceLevel;
      VitalitySave = currentstats.vitalityLevel;
      UIenable.SetActive(false);
      HUDCanvasGroup.alpha = 1;
      hudSkills.UpdateHUDIcons();
      currentstats.UpdateLevel();
      playerController.EnableController();
    }
    /*public void CloseController()
    {
      Closebutton.Select();
    }*/
    public void Close()
    {
      playerController.EnableController();
      currentstats.primalLevel = PrimalSave;
      currentstats.speedLevel = SpeedSave;
      currentstats.sentienceLevel = SentienceSave;
      currentstats.vitalityLevel = VitalitySave;
      currentnutrients.currentNutrients = nutrientsSave;
      currentstats.baseHealth = healthsave;
      currentstats.baseRegen = regensave;
      currentstats.moveSpeed = movespeedsave;
      currentstats.primalDmg = damagesave;
      //currentstats.skillDmg = skilldmgsave;
      //currentstats.atkCooldownBuff = cdrsave;
      currentstats.totalLevel = totalLevelsave;
      currentstats.levelUpCost = levelupsave;
      currentstats.UpdateLevel();
      UIenable.SetActive(false);
      HUDCanvasGroup.alpha = 1;
      hudSkills.UpdateHUDIcons();
      

    }
    public void PrimalStartCheck()
    {
        if(currentstats.primalLevel == PrimalSave)
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
        if(currentstats.speedLevel == SpeedSave)
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
        if(currentstats.sentienceLevel == SentienceSave)
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
        if(currentstats.vitalityLevel == VitalitySave)
        {
        VitalityLevelDown.interactable = false;
        }
        else 
        {
        VitalityLevelDown.interactable = true;
        }
    }

    
    void UnlockSkills()
    {
      newlyUnlockedSkills.Clear();
      int currentStat = currentstats.primalLevel;
      UnlockConditional("Eruption", currentStat, 5);
      UnlockConditional("LivingCyclone", currentStat, 10);
      UnlockConditional("RelentlessFury", currentStat, 15);
      currentStat = currentstats.speedLevel;
      UnlockConditional("Blitz", currentStat, 5);
      UnlockConditional("TrophicCascade", currentStat, 10);
      UnlockConditional("Mycotoxins", currentStat, 15);
      currentStat = currentstats.sentienceLevel;
      UnlockConditional("Spineshot", currentStat, 5);
      UnlockConditional("UnstablePuffball", currentStat, 10);
      UnlockConditional("Undergrowth", currentStat, 15);
      currentStat = currentstats.vitalityLevel;
      UnlockConditional("LeechingSpore", currentStat, 5);
      UnlockConditional("Sporeburst", currentStat, 10);
      UnlockConditional("DefenseMechanism", currentStat, 15);
      if (newlyUnlockedSkills.Count == 1)
      {
        skillUnlockNotifications.NotifySkillUnlock(newlyUnlockedSkills[0], hudSkills.GetSkillSprite(newlyUnlockedSkills[0]));
      }
      else if (newlyUnlockedSkills.Count > 1)
      {
        skillUnlockNotifications.NotifyMultipleSkillUnlocks(newlyUnlockedSkills.Count);
      }
    }
    void UnlockConditional(string skillName, int currentStatLevel, int targetStatLevel)
    {
      if (currentStatLevel >= targetStatLevel && currentstats.skillUnlocks[skillName] == false)
      { 
        newlyUnlockedSkills.Add(skillName);
        currentstats.UnlockSkill(skillName);

        //Debug.Log("Unlocked: " + currentstats.skillUnlocks[skillName]);
        //Debug.Log("Equippable: " + currentstats.skillEquippables[skillName]);
      }
    }
}
