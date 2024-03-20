using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class LevelUpManagerNew : MonoBehaviour
{
    //[SerializeField] private int maxStatLevel = 15;

    public TMP_Text primaldam;
    public TMP_Text movespeed;
    public TMP_Text regen;
    public TMP_Text health;
    public TMP_Text Nutrients;
    public TMP_Text LevelUpCost;
    public TMP_Text VitalityText;
    public GameObject UIenable;
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
    private int totalLevelsave;
    private int levelupsave;
    private HUDController hudController;
    private PlayerHealth playerHealth;
    public CharacterStats currentstats;
    private PlayerController playerController;
    public NutrientTracker currentnutrients;
    public GameObject Skill1Image;
    public GameObject Skill2Image;
    public GameObject Skill3Image;
    public TMP_Text Skill1CD;
    public TMP_Text Skill1Dam;
    public TMP_Text Skill2CD;
    public TMP_Text Skill2Dam;
    public TMP_Text Skill3CD;
    public TMP_Text Skill3Dam;
    private SkillManager skillManager;
    public GameObject SkillMenu;
    private HUDSkills hudSkills;
    private HUDNutrients hudNutrients;
    private PlayerHealth playerhealth;
    public GameObject[] PrimalPoints;
    public GameObject [] SpeedPoints;
    public GameObject[] SentiencePoints;
    public GameObject[] VitalityPoints;
    public GameObject ConfirmPrimal;
    public GameObject ConfirmSpeed;
    public GameObject ConfirmSent;
    public GameObject ConfirmVit;
    public Image PrimalArrowUp;
    public Image PrimalArrowDown;
    public Image SpeedArrowUp;
    public Image SpeedArrowDown;
    public Image SentienceArrowUp;
    public Image SentienceArrowDown;
    public Image VitalityArrowUp;
    public Image VitalityArrowDown;
    public GameObject Camera;
    
    ThirdPersonActionsAsset controls;
    [SerializeField] private SkillUnlockNotifications skillUnlockNotifications;
    List<string> newlyUnlockedSkills = new List<string>();
    public GameObject GrowMenu;
   
  
    void Start()
    {
        hudController = GameObject.Find("HUD").GetComponent<HUDController>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        hudNutrients = GameObject.Find("HUD").GetComponent<HUDNutrients>();
        playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>();
        skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
        StartCoroutine(UpdateUI());
    }

    void OnEnable()
    {
      Camera.SetActive(true);
      currentnutrients = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();
      controls = new ThirdPersonActionsAsset();
      controls.UI.Close.performed += ctx => Close();
       currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
       playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
       playerhealth = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>();
       hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
       PrimalSave = currentstats.primalLevel;
       SpeedSave = currentstats.speedLevel;
       SentienceSave = currentstats.sentienceLevel;
       VitalitySave = currentstats.vitalityLevel;
       nutrientsSave = currentnutrients.currentNutrients;
       healthsave = currentstats.baseHealth;
       regensave = currentstats.baseRegen;
       movespeedsave = currentstats.moveSpeed;
       damagesave = currentstats.primalDmg;
       totalLevelsave = currentstats.totalLevel;
       levelupsave = currentstats.levelUpCost;
       Commitbutton.Select();
       SkillCD();
       SkillDam();
       StartCoroutine(UpdateUI());
       SkillMenu.SetActive(false);
       GrowMenu.SetActive(false);
       PrimalBarFill();
       SpeedBarFill();
       SentienceBarFill();
       VitalityBarFill();
       List<Sprite> equippedSkillSprites = hudSkills.GetAllSkillSprites();
        Skill1Image.GetComponent<Image>().sprite = equippedSkillSprites[0];
        Skill2Image.GetComponent<Image>().sprite = equippedSkillSprites[1];
        Skill3Image.GetComponent<Image>().sprite = equippedSkillSprites[2];
      ControlEnable();
      Camera.SetActive(true);
      Invoke("MenuSwapDelay", 0.25f);

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
    public void ControlEnable()
    {
       controls.UI.Enable();  
    }
    void MenuSwapDelay()
    {
      controls.UI.MenuSwapR.performed += ctx => MenuSwap();
      controls.UI.MenuSwapL.performed += ctx => MenuSwapL();
    }
    void OnDisable()
    {
      controls.UI.Disable();
    }
    public void ControlDisable()
    {
      controls.UI.Disable();
    }
     IEnumerator UpdateUI()
    {
        
        yield return null;       
        LevelUpCost.text = currentstats.levelUpCost.ToString();
        Nutrients.text = currentnutrients.currentNutrients.ToString();
        CurrentLevel.text = currentstats.totalLevel.ToString();
        health.text = currentstats.baseHealth.ToString();
        regen.text = currentstats.baseRegen.ToString("0.00") + " HPS";
        movespeed.text = currentstats.moveSpeed.ToString("0.0") + " m/s";
        primaldam.text = currentstats.primalDmg.ToString();
        PrimalBarFill();
        SpeedBarFill();
        SentienceBarFill();
        VitalityBarFill();
        DownArrowUpdate();
        PrimalArrowIncrease();
        SpeedArrowIncrease();
        SentienceArrowIncrease();
        VitalityArrowIncrease();
    }
    void PrimalDownUpdate()
    {
      if(currentstats.primalLevel == 1 || currentstats.primalLevel == PrimalSave)
      {
        PrimalArrowDown.enabled = false;
      }
      else
      {
        PrimalArrowDown.enabled = true;
      }
    }
    void SpeedDownUpdate()
    {
      if(currentstats.speedLevel == 1 || currentstats.speedLevel == SpeedSave)
      {
        SpeedArrowDown.enabled = false;
      }
      else
      {
        SpeedArrowDown.enabled = true;
      }
    }
    void SentienceDownUpdate()
    {
      if(currentstats.sentienceLevel == 1 || currentstats.sentienceLevel == SentienceSave)
      {
        SentienceArrowDown.enabled = false;
      }
      else
      {
        SentienceArrowDown.enabled = true;
      }
    }
    void VitalityDownUpdate()
    {
      if(currentstats.vitalityLevel == 1 || currentstats.vitalityLevel == VitalitySave)
      {
        VitalityArrowDown.enabled = false;
      }
      else
      {
        VitalityArrowDown.enabled=true;
      }
    }
    void DownArrowUpdate()
    {
      PrimalDownUpdate();
      SpeedDownUpdate();
      SentienceDownUpdate();
      VitalityDownUpdate();
    }
    void PrimalArrowSprite()
    {
      switch(currentstats.equippedSkills[0])
      {
      case "FungalMight":
      PrimalArrowUp.sprite = Resources.Load<Sprite>("RottenLog");
      break;
      case "DeathBlossom":
      PrimalArrowUp.sprite = Resources.Load<Sprite>("FreshExoskeleton");
      break;
      default:
      break;
      }
    }
    void SpeedArrowSprite()
    {
      switch(currentstats.equippedSkills[0])
      {
      case "FungalMight":
      SpeedArrowUp.sprite = Resources.Load<Sprite>("RottenLog");
      break;
      case "DeathBlossom":
      SpeedArrowUp.sprite = Resources.Load<Sprite>("FreshExoskeleton");
      break;
      default:
      break;
      }
    }
    void SentienceArrowSprite()
    {
      switch(currentstats.equippedSkills[0])
      {
      case "FungalMight":
      SentienceArrowUp.sprite = Resources.Load<Sprite>("RottenLog");
      break;
      case "DeathBlossom":
      SentienceArrowUp.sprite = Resources.Load<Sprite>("FreshExoskeleton");
      break;
      default:
      break;
      }
    }
    void VitalityArrowSprite()
    {
      switch(currentstats.equippedSkills[0])
      {
      case "FungalMight":
      VitalityArrowUp.sprite = Resources.Load<Sprite>("RottenLog");
      break;
      case "DeathBlossom":
      VitalityArrowUp.sprite = Resources.Load<Sprite>("FreshExoskeleton");
      break;
      default:
      break;
      }
    }
    void PrimalArrowIncrease()
    {
      if(currentstats.primalLevel == 4 || currentstats.primalLevel == 9 || currentstats.primalLevel == 14)
      {
        PrimalArrowSprite();
      }
      else
      {
        PrimalArrowUp.sprite = Resources.Load<Sprite>("arrow 1");
      }
    }
    
    void SpeedArrowIncrease()
    {
      if(currentstats.speedLevel == 4 || currentstats.speedLevel == 9 || currentstats.speedLevel == 14)
      {
        SpeedArrowSprite(); 
      }
      else
      {
        SpeedArrowUp.sprite = Resources.Load<Sprite>("arrow 1");
      }
    }
    void SentienceArrowIncrease()
    {
      if(currentstats.sentienceLevel == 4 || currentstats.sentienceLevel == 9 || currentstats.sentienceLevel == 14)
      {
        SentienceArrowSprite(); 
      }
      else
      {
        SentienceArrowUp.sprite = Resources.Load<Sprite>("arrow 1");
      }
    }
    void VitalityArrowIncrease()
    {
      if(currentstats.vitalityLevel == 4 || currentstats.vitalityLevel == 9 || currentstats.vitalityLevel == 14)
      {
        VitalityArrowSprite(); 
      }
      else
      {
        VitalityArrowUp.sprite = Resources.Load<Sprite>("arrow 1");
      }
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
      currentstats.totalLevel = totalLevelsave;
      currentstats.levelUpCost = levelupsave;
      currentstats.UpdateLevel();
      GrowMenu.SetActive(true); 
    }

    public void PrimalBarFill()
    {
      for (int i = 0; i < PrimalPoints.Length; i++)
      {
        PrimalPoints[i].SetActive(i <= (currentstats.primalLevel - 1));
      }
    }
    public void PrimalBarLevelUp()
    {
      
      controls.UI.KeyLevelUpPrimal.Enable();
      controls.UI.KeyLevelDownPrimal.Enable();
      controls.UI.PrimalLevelRight.Enable();
      controls.UI.PrimalLevelLeft.Enable();
      controls.UI.PrimalLevelRightStick.Enable();
      controls.UI.PrimalLevelLeftStick.Enable();
      controls.UI.KeyLevelUpPrimal.started += ctx => PrimalUP();
      controls.UI.KeyLevelDownPrimal.started += ctx => PrimalDown();
      controls.UI.PrimalLevelRight.started += ctx => PrimalUP();
      controls.UI.PrimalLevelRightStick.started += ctx => PrimalUP(); 
      controls.UI.PrimalLevelLeft.started += ctx => PrimalDown(); 
      controls.UI.PrimalLevelLeftStick.started += ctx => PrimalDown();
    }
    public void UIUpdate()
    {
      StartCoroutine(UpdateUI());
    }
    
    public void PrimalUP()
    {
      switch(currentstats.equippedSkills[0])
      {
        case "FungalMight":
        currentstats.LevelPrimal();
        StartCoroutine(UpdateUI());
        break;
        case "DeathBlossom":
        currentstats.LevelPrimalPoison();
        StartCoroutine(UpdateUI());
        break;
        case "FairyRing":
        currentstats.LevelPrimalCoral();
        StartCoroutine(UpdateUI());
        break;
        case "Zombify":
        currentstats.LevelPrimalCordyceps();
        StartCoroutine(UpdateUI());
        break;
        default:
        break;
      }
    }
    public void PrimalDown()
    {
      if(currentstats.primalLevel == PrimalSave)
      {
        return;
      }
      else
      {
      currentstats.DeLevelPrimal(); 
      StartCoroutine(UpdateUI());
      }
    }
    public void PrimalDeselect()
    {
      controls.UI.KeyLevelDownPrimal.Disable();
      controls.UI.KeyLevelUpPrimal.Disable();
      controls.UI.PrimalLevelRight.Disable();
      controls.UI.PrimalLevelLeft.Disable();
      controls.UI.PrimalLevelRightStick.Disable();
      controls.UI.PrimalLevelLeftStick.Disable();
    }
       public void SpeedBarFill()
    {
      for (int i = 0; i < SpeedPoints.Length; i++)
      {
        SpeedPoints[i].SetActive(i <= (currentstats.speedLevel - 1));
      }
    }
    public void SpeedBarLevelUp()
    {
      controls.UI.KeyLevelUpSpeed.Enable();
      controls.UI.KeyLevelDownSpeed.Enable();
      controls.UI.SpeedLevelRight.Enable();
      controls.UI.SpeedLevelLeft.Enable();
      controls.UI.SpeedLevelRightStick.Enable();
      controls.UI.SpeedLevelLeftStick.Enable();
      controls.UI.KeyLevelUpSpeed.started += ctx => SpeedUP();
      controls.UI.KeyLevelDownSpeed.started += ctx => SpeedDown();
      controls.UI.SpeedLevelRight.started += ctx => SpeedUP();
     controls.UI.SpeedLevelRightStick.started += ctx => SpeedUP(); 
     controls.UI.SpeedLevelLeft.started += ctx => SpeedDown(); 
      controls.UI.SpeedLevelLeftStick.started += ctx => SpeedDown();
    }
  
    public void SpeedUP()
    {
      switch(currentstats.equippedSkills[0])
      {
        case "FungalMight":
        currentstats.LevelSpeed();
        StartCoroutine(UpdateUI());
        break;
        case "DeathBlossom":
        currentstats.LevelSpeedPoison();
        StartCoroutine(UpdateUI());
        break;
        case "FairyRing":
        currentstats.LevelSpeedCoral();
        StartCoroutine(UpdateUI());
        break;
        case "Zombify":
        currentstats.LevelSpeedCordyceps();
        StartCoroutine(UpdateUI());
        break;
        default:
        break;
      }  
    }
    public void SpeedDown()
    {
      if(currentstats.speedLevel == SpeedSave)
      {
        return;
      }
      else
      {
      currentstats.DeLevelSpeed(); 
      StartCoroutine(UpdateUI());
      }
    }
     public void SpeedDeselect()
    {
      controls.UI.KeyLevelDownSpeed.Disable();
      controls.UI.KeyLevelUpSpeed.Disable();
      controls.UI.SpeedLevelRight.Disable();
      controls.UI.SpeedLevelLeft.Disable();
      controls.UI.SpeedLevelRightStick.Disable();
      controls.UI.SpeedLevelLeftStick.Disable();
    }
    
     public void SentienceBarFill()
    {
      for (int i = 0; i < SentiencePoints.Length; i++)
      {
        SentiencePoints[i].SetActive(i <= (currentstats.sentienceLevel - 1));
      }
    }
      public void SentienceBarLevelUp()
    {
       controls.UI.KeyLevelUpSentience.Enable();
       controls.UI.KeyLevelDownSentience.Enable();
       controls.UI.SentienceLevelRight.Enable();
      controls.UI.SentienceLevelLeft.Enable();
      controls.UI.SentienceLevelRightStick.Enable();
      controls.UI.SentienceLevelLeftStick.Enable();
      controls.UI.KeyLevelUpSentience.started += ctx => SentienceUP();
      controls.UI.KeyLevelDownSentience.started += ctx => SentienceDown();
      controls.UI.SentienceLevelRight.started += ctx => SentienceUP();
       controls.UI.SentienceLevelRightStick.started += ctx => SentienceUP(); 
       controls.UI.SentienceLevelLeft.started += ctx => SentienceDown(); 
       controls.UI.SentienceLevelLeftStick.started += ctx => SentienceDown();
    }
    public void SentienceUP()
    {
      switch(currentstats.equippedSkills[0])
      {
        case "FungalMight":
         currentstats.LevelSentience();
         SkillCD();
         SkillDam();
         StartCoroutine(UpdateUI());
         SkillUIUpdate();
         break;
        case "DeathBlossom":
         currentstats.LevelSentiencePoison();
         SkillCD();
         SkillDam();
         StartCoroutine(UpdateUI());
         SkillUIUpdate();
        break;
        case "FairyRing":
        currentstats.LevelSentienceCoral();
        SkillCD();
        SkillDam();
        StartCoroutine(UpdateUI());
        SkillUIUpdate();
        break;
        case "Zombify":
        currentstats.LevelSentienceCordyceps();
        SkillCD();
        SkillDam();
        StartCoroutine(UpdateUI());
        SkillUIUpdate();
        break;
        default:
        break;
      }
    }
    void SkillUIUpdate()
    {
      GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
      List<float> allCooldowns = skillManager.GetEquippedSkillCooldowns(currentPlayer);
    }
    
    public void SentienceDown()
    {
      if(currentstats.sentienceLevel == SentienceSave)
      {
        return;
      }
      else
      {
      currentstats.DeLevelSentience(); 
      SkillCD();
      SkillDam();
      StartCoroutine(UpdateUI());
      GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
      List<float> allCooldowns = skillManager.GetEquippedSkillCooldowns(currentPlayer);
      }
    }
    public void SentienceDeselect()
    {
      controls.UI.KeyLevelDownSentience.Disable();
      controls.UI.KeyLevelUpSentience.Disable();
      controls.UI.SentienceLevelRight.Disable();
      controls.UI.SentienceLevelLeft.Disable();
      controls.UI.SentienceLevelRightStick.Disable();
      controls.UI.SentienceLevelLeftStick.Disable();
    }
    public void VitalityBarFill()
    {
      for (int i = 0; i < VitalityPoints.Length; i++)
      {
        VitalityPoints[i].SetActive(i <= (currentstats.vitalityLevel - 1));
      }
    }
      public void VitalityBarLevelUp()
    {
      controls.UI.KeyLevelUpVitality.Enable();
      controls.UI.KeyLevelDownVitality.Enable();
      controls.UI.VitalityLevelRight.Enable();
      controls.UI.VitalityLevelLeft.Enable();
      controls.UI.VitalityLevelRightStick.Enable();
      controls.UI.VitalityLevelLeftStick.Enable();
       controls.UI.KeyLevelUpVitality.started += ctx => VitalityUP();
       controls.UI.KeyLevelDownVitality.started += ctx => VitalityDown();
       controls.UI.VitalityLevelRight.started += ctx => VitalityUP();
      controls.UI.VitalityLevelRightStick.started += ctx => VitalityUP(); 
      controls.UI.VitalityLevelLeft.started += ctx => VitalityDown(); 
      controls.UI.VitalityLevelLeftStick.started += ctx => VitalityDown();
    }
    public void VitalityDeselect()
    {
      controls.UI.KeyLevelDownVitality.Disable();
      controls.UI.KeyLevelUpVitality.Disable();
      controls.UI.VitalityLevelRight.Disable();
      controls.UI.VitalityLevelLeft.Disable();
      controls.UI.VitalityLevelRightStick.Disable();
      controls.UI.VitalityLevelLeftStick.Disable();
    }
    public void VitalityUP()
    {
      switch(currentstats.equippedSkills[0])
      {
        case "FungalMight":
        currentstats.LevelVitalityPoison();
        StartCoroutine(UpdateUI());
        break;
        case "DeathBlossom":
        currentstats.LevelVitalityPoison();
        StartCoroutine(UpdateUI());
        break;
        case "FairyRing":
        currentstats.LevelVitalityCoral();
        StartCoroutine(UpdateUI());
        break;
        case "Zombify":
        currentstats.LevelVitalityCordyceps();
        StartCoroutine(UpdateUI());
        break;
        default:
        break;
      }
    }
    public void VitalityDown()
    {
      if(currentstats.vitalityLevel == VitalitySave)
      {
        return;
      }
      else{
      currentstats.DeLevelVitality(); 
      StartCoroutine(UpdateUI());
      }
    }
    public void Commit()
    {
      Camera.SetActive(false);
      PrimalSave = currentstats.primalLevel;
      SpeedSave = currentstats.speedLevel;
      SentienceSave = currentstats.sentienceLevel;
      VitalitySave = currentstats.vitalityLevel;
      playerhealth.GetHealthStats();
      playerhealth.currentHealth = playerhealth.maxHealth;
      UIenable.SetActive(false);
      hudController.FadeInHUD();
      hudSkills.UpdateHUDIcons();
      currentstats.UpdateLevel();
      playerController.EnableController();
      UnlockSkills();

      //This helps fix the bug where you could pause in the shop
      GlobalData.isAbleToPause = true;
    }
    /*public void CloseController()
    {
      Closebutton.Select();
    }*/
    public void Close()
    {
      Camera.SetActive(false);
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
      hudController.FadeInHUD();
      hudSkills.UpdateHUDIcons();
      hudNutrients.UpdateNutrientsUI(nutrientsSave);

      //This helps fix the bug where you could pause in the shop
      GlobalData.isAbleToPause = true;
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
      if (currentStatLevel >= targetStatLevel && currentstats.skillEquippables[skillName] == false)
      { 
        newlyUnlockedSkills.Add(skillName);
        currentstats.UnlockSkill(skillName);

        //Debug.Log("Unlocked: " + currentstats.skillUnlocks[skillName]);
        //Debug.Log("Equippable: " + currentstats.skillEquippables[skillName]);
      }
    }
    void SkillCD()
    {
      skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
      List<float> CDList = skillManager.GetEquippedSkillCooldowns(GameObject.FindWithTag("currentPlayer"));
      Skill1CD.text = "Cooldown: <br>" + CDList[0].ToString("0.0") + " Seconds";
      Skill2CD.text = "Cooldown: <br>" + CDList[1].ToString("0.0") + " Seconds";
      Skill3CD.text = "Cooldown: <br>" + CDList[2].ToString("0.0") + " Seconds";
    }
    void SkillDam()
    {
      skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
      List<float> DamList = skillManager.GetEquippedSkillValues(GameObject.FindWithTag("currentPlayer"));
      Skill1Dam.text = "Damage: " + DamList[0].ToString();
      Skill2Dam.text =  "Damage: " + DamList[1].ToString();
      Skill3Dam.text =  "Damage: " + DamList[2].ToString();
    }
}
