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
    private bool PrimalSelected;
    private bool SpeedSelected;
    private bool SentienceSelected;
    private bool VitalitySelected;
    ThirdPersonActionsAsset controls;
    List<string> newlyUnlockedSkills = new List<string>();
    public GameObject GrowMenu;
    public GameObject EventSystem;

    int ogPrimalLevel;
    int ogSentienceLevel;
    int ogSpeedLevel;
    int ogVitalityLevel;

    private void Awake()
    {
        controls = new ThirdPersonActionsAsset();
    }

    void Start()
    {
        hudController = GameObject.Find("HUD").GetComponent<HUDController>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        hudNutrients = GameObject.Find("HUD").GetComponent<HUDNutrients>();
        playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>();
        skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
        StartCoroutine(UpdateUI());
    }

    void InitializeOGStats(CharacterStats currentStats)
    {
      ogPrimalLevel = currentStats.primalLevel;
      ogSentienceLevel = currentStats.sentienceLevel;
      ogSpeedLevel = currentStats.speedLevel;
      ogVitalityLevel = currentStats.vitalityLevel;
    }

    void OnEnable()
    {
      EventSystem.SetActive(false);
      Invoke("EnableEvent", 0.60f);
      Invoke("Commitbuttonselect", 0.65f);
      Camera.SetActive(true);
      currentnutrients = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();
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
       SkillCD();
       SkillDam();
       StartCoroutine(UpdateUI());
       SkillMenu.SetActive(false);
       GrowMenu.SetActive(false);
       PrimalBarFill();
       SpeedBarFill();
       SentienceBarFill();
       VitalityBarFill();
       PrimalSelected = false;
       SpeedSelected = false;
       SentienceSelected = false;
       VitalitySelected = false;
       List<Sprite> equippedSkillSprites = hudSkills.GetAllSkillSprites();
        Skill1Image.GetComponent<Image>().sprite = equippedSkillSprites[0];
        Skill2Image.GetComponent<Image>().sprite = equippedSkillSprites[1];
        Skill3Image.GetComponent<Image>().sprite = equippedSkillSprites[2];
      Camera.SetActive(true);
      Invoke("ControlEnable", 0.60f);
      Invoke("MenuSwapDelay", 0.60f);

      InitializeOGStats(currentstats);
    }
    void Update()
    {

    }
    void EnableEvent()
    {
     EventSystem.SetActive(true);
     
    }
    void Commitbuttonselect()
    {
      Commitbutton.Select();
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
        regen.text = currentstats.baseRegen.ToString("0.00") + " hp/s";
        movespeed.text = currentstats.moveSpeed.ToString("0.0") + "m/s";
        primaldam.text = currentstats.primalDmg.ToString();
        controls.Player.Disable();
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
      PrimalArrowUp.sprite = Resources.Load<Sprite>("Rotten Log");
      break;
      case "DeathBlossom":
      PrimalArrowUp.sprite = Resources.Load<Sprite>("Fresh Exoskeleton");
      break;
      case "FairyRing":
      PrimalArrowUp.sprite = Resources.Load<Sprite>("Calcite Deposit");
      break;
      case "Zombify":
      PrimalArrowUp.sprite = Resources.Load<Sprite>("Flesh");
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
      SpeedArrowUp.sprite = Resources.Load<Sprite>("Rotten Log");
      break;
      case "DeathBlossom":
      SpeedArrowUp.sprite = Resources.Load<Sprite>("Fresh Exoskeleton");
      break;
      case "FairyRing":
      SpeedArrowUp.sprite = Resources.Load<Sprite>("Calcite Deposit");
      break;
      case "Zombify":
      SpeedArrowUp.sprite = Resources.Load<Sprite>("Flesh");
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
      SentienceArrowUp.sprite = Resources.Load<Sprite>("Rotten Log");
      break;
      case "DeathBlossom":
      SentienceArrowUp.sprite = Resources.Load<Sprite>("Fresh Exoskeleton");
      break;
      case "FairyRing":
      SentienceArrowUp.sprite = Resources.Load<Sprite>("Calcite Deposit");
      break;
      case "Zombify":
      SentienceArrowUp.sprite = Resources.Load<Sprite>("Flesh");
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
      VitalityArrowUp.sprite = Resources.Load<Sprite>("Rotten Log");
      break;
      case "DeathBlossom":
      VitalityArrowUp.sprite = Resources.Load<Sprite>("Fresh Exoskeleton");
      break;
      case "FairyRing":
      VitalityArrowUp.sprite = Resources.Load<Sprite>("Calcite Deposit");
      break;
      case "Zombify":
      VitalityArrowUp.sprite = Resources.Load<Sprite>("Flesh");
      break;
      default:
      break;
      }
    }
    void PrimalArrowIncrease()
    {
      if(currentstats.primalLevel == 9 || currentstats.primalLevel == 14)
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
      if(currentstats.speedLevel == 9 || currentstats.speedLevel == 14)
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
      if(currentstats.sentienceLevel == 9 || currentstats.sentienceLevel == 14)
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
      if(currentstats.vitalityLevel == 9 || currentstats.vitalityLevel == 14)
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
      SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("Camtracker").transform);
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
      SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("Camtracker").transform);
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
      PrimalSelected = true;
      SpeedSelected = false;
      SentienceSelected = false;
      VitalitySelected = false;
      if(PrimalSelected == true && SpeedSelected == false && SentienceSelected == false && VitalitySelected == false)
      {
      controls.UI.PrimalLevelRight.Enable();
      controls.UI.PrimalLevelLeft.Enable();
      controls.UI.PrimalLevelRight.started += ctx => PrimalUP();
      controls.UI.PrimalLevelLeft.started += ctx => PrimalDown(); 
      SpeedDeselect();
      SentienceDeselect();
      VitalityDeselect();
      }
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
      controls.UI.PrimalLevelRight.Disable();
      controls.UI.PrimalLevelLeft.Disable();
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
      PrimalSelected = false;
      SpeedSelected = true;
      SentienceSelected = false;
      VitalitySelected = false;
      if(SpeedSelected == true && PrimalSelected == false && SentienceSelected == false && VitalitySelected == false)
      {
      controls.UI.SpeedLevelRight.Enable();
      controls.UI.SpeedLevelLeft.Enable();
      controls.UI.SpeedLevelRight.started += ctx => SpeedUP();
     controls.UI.SpeedLevelLeft.started += ctx => SpeedDown(); 
      PrimalDeselect();
       SentienceDeselect();
       VitalityDeselect();
      }
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
      controls.UI.SpeedLevelRight.Disable();
      controls.UI.SpeedLevelLeft.Disable();
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
      PrimalSelected = false;
      SpeedSelected = false;
      SentienceSelected = true;
      VitalitySelected = false;
      if(SentienceSelected == true && PrimalSelected == false && SpeedSelected == false && VitalitySelected == false)
      {
      controls.UI.SentienceLevelRight.Enable();
      controls.UI.SentienceLevelLeft.Enable();
      controls.UI.SentienceLevelRight.started += ctx => SentienceUP();
      controls.UI.SentienceLevelLeft.started += ctx => SentienceDown(); 
      PrimalDeselect();
      SpeedDeselect();
      VitalityDeselect();
      }
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
      controls.UI.SentienceLevelRight.Disable();
      controls.UI.SentienceLevelLeft.Disable();
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
      PrimalSelected = false;
      SpeedSelected = false;
      SentienceSelected = false;
      VitalitySelected = true;
      if(VitalitySelected == true && PrimalSelected == false && SpeedSelected == false && SentienceSelected == false)
      {
      controls.UI.VitalityLevelRight.Enable();
      controls.UI.VitalityLevelLeft.Enable();
      controls.UI.VitalityLevelRight.started += ctx => VitalityUP();
      controls.UI.VitalityLevelLeft.started += ctx => VitalityDown(); 
      PrimalDeselect();
       SpeedDeselect();
       SentienceDeselect();
      }
    }
    public void VitalityDeselect()
    {
      controls.UI.VitalityLevelRight.Disable();
      controls.UI.VitalityLevelLeft.Disable();
    }
    public void VitalityUP()
    {
      switch(currentstats.equippedSkills[0])
      {
        case "FungalMight":
        currentstats.LevelVitality();
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
    public void CommitSelect()
    {
      PrimalDeselect();
       SpeedDeselect();
       SentienceDeselect();
       VitalityDeselect();
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
      int ogStat = ogPrimalLevel;
      UnlockConditional("Eruption", currentStat, ogStat, 5);
      UnlockConditional("LivingCyclone", currentStat, ogStat, 10);
      UnlockConditional("RelentlessFury", currentStat, ogStat, 15);
      currentStat = currentstats.speedLevel;
      ogStat = ogSpeedLevel;
      UnlockConditional("Blitz", currentStat, ogStat, 5);
      UnlockConditional("TrophicCascade", currentStat, ogStat, 10);
      UnlockConditional("Mycotoxins", currentStat, ogStat, 15);
      currentStat = currentstats.sentienceLevel;
      ogStat = ogSentienceLevel;
      UnlockConditional("Spineshot", currentStat, ogStat, 5);
      UnlockConditional("UnstablePuffball", currentStat, ogStat, 10);
      UnlockConditional("Undergrowth", currentStat, ogStat, 15);
      currentStat = currentstats.vitalityLevel;
      ogStat = ogVitalityLevel;
      UnlockConditional("LeechingSpore", currentStat, ogStat, 5);
      UnlockConditional("Sporeburst", currentStat, ogStat, 10);
      UnlockConditional("DefenseMechanism", currentStat, ogStat, 15);
      if (newlyUnlockedSkills.Count == 1)
      {
        NotificationManager.Instance.Notification(newlyUnlockedSkills[0] + " Unlocked", "Equip at Sporemother", hudSkills.GetSkillSprite(newlyUnlockedSkills[0]));
      }
      else if (newlyUnlockedSkills.Count > 1)
      {
        NotificationManager.Instance.Notification(newlyUnlockedSkills.Count + " New Skills Unlocked", "Equip at Sporemother");
      }
    }
    void UnlockConditional(string skillName, int currentStatLevel, int ogStatLevel, int targetStatLevel)
    {
      if (currentStatLevel >= targetStatLevel && ogStatLevel < targetStatLevel)
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
      Skill1CD.text = "Cooldown: <color=#97F8D2>" + CDList[0].ToString("0.0") + " s</color>";
      Skill2CD.text = "Cooldown: <color=#97F8D2>" + CDList[1].ToString("0.0") + " s</color>";
      Skill3CD.text = "Cooldown: <color=#97F8D2>" + CDList[2].ToString("0.0") + " s</color>";
    }
    void SkillDam()
    {
      skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
      List<float> DamList = skillManager.GetEquippedSkillValues(GameObject.FindWithTag("currentPlayer"));
      Skill1Dam.text = "Damage: <color=#97F8D2>" + DamList[0].ToString() + "</color>";
      Skill2Dam.text = "Damage: <color=#97F8D2>" + DamList[1].ToString() + "</color>";
      Skill3Dam.text = "Damage: <color=#97F8D2>" + DamList[2].ToString() + "</color>";
    }
}
