using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class SkillMenuManagerUpdated : MonoBehaviour
{
    
    [SerializeField] private FullScreenPassRendererFeature fog;
    public GameObject LevelUI;
    ThirdPersonActionsAsset controls;
    public GameObject UIenable;
     private CanvasGroup HUDCanvasGroup;
    private HUDSkills hudSkills;
    private PlayerController playerController;
    public GameObject GrowMenu;
    public CharacterStats currentstats;
    public GameObject[] PrimalPoints;
    public GameObject [] SpeedPoints;
    public GameObject[] SentiencePoints;
    public GameObject[] VitalityPoints;
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
    public GameObject PanelHolder;
    //DM Buttons
    public Button DefenseMechanismUnlocked;
    public GameObject Skill2;
    public GameObject Skill3;
    public GameObject EruptionLock;
    public GameObject LivingCycloneLock;
    public GameObject RelentlessFuryLock;
    public GameObject BlitzLock;
    public GameObject TrophicCascadeLock;
    public GameObject MycoToxLock;
    public GameObject Spineshotlock;
    public GameObject UnstableLock;
    public GameObject UndergrowthLock;
    public GameObject LeechingLock;
    public GameObject SporeburstLock;
    public GameObject DefenseMechLock;
    public GameObject DescriptionPanelHolder;
    public SkillManager skillmanager;
    public bool EruptionActive = false;
    private bool LivingCycloneActive = false;
    private bool FuryActive = false;
    private bool BlitzActive = false;
    private bool TrophicActive = false;
    private bool MycoActive = false;
    private bool SpineActive = false;
    private bool UnstableActive = false;
    private bool UndergrowthActive = false;
    private bool LeechActive = false;
    private bool SporeburstActive = false;
    private bool DefenseMechActive = false;
    public Color newColor;
    public Color defaultcolor;
    public GameObject InstantiatedSpore;
    public Transform MenuSporeLocation;
    public GameObject Camera;
    private SwapCharacter swapcharacterscript;
    public Navigation NoneNav = new Navigation();
    public Navigation AutoNav = new Navigation();
    public bool ErupDescrip = false;
    public bool LCDescrip = false;
    public bool RelentDescrip = false;
    public bool BlitzDescrip = false;
    public bool TrophicDescrip = false;
    public bool MycoDescrip = false;
    public bool SpineDescrip = false;
    public bool UnstableDescrip = false;
    public bool UnderDescrip = false;
    public bool LeechDescrip = false;
    public bool SporeDescrip = false;
    public bool DefenseDescrip = false;
    public bool DescriptionActive = false;
    private bool CharacterButtonsSelected = false;
    public GameObject DescriptionPanel;
    public TMP_Text DescriptionText;

    private void Awake()
    {
        controls = new ThirdPersonActionsAsset();
    }

    void OnEnable()
    {  
        fog.SetActive(false);  
        LevelUI.SetActive(false);
        controls.UI.Close.performed += ctx => CloseSkill();
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        swapcharacterscript = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
        UpdateUI();
        Invoke("ControlEnable", 0.50f);
        Invoke("MenuSwapDelay", 0.60f);
        GrowMenu.SetActive(false);
        Camera.SetActive(true);
        EruptionUnlocked.Select();
        ErupDescrip = true;
        ButtonListeners();
        Invoke("InstantiateCurrentSpore", 0.01f);
        //Primal Unlock Statements
        
        
    }
    public void MenuSwapDelay()
    {
      controls.UI.MenuSwapL.performed += ctx => MenuSwapLeft();
      controls.UI.MenuSwapR.performed += ctx => MenuSwapRight();
    }
   
     void OnDisable()
    {
      controls.UI.MenuSwapL.performed -= ctx => MenuSwapLeft();
      controls.UI.MenuSwapR.performed -= ctx => MenuSwapRight();
      controls.UI.Disable();
      Destroy(InstantiatedSpore);
      fog.SetActive(true); 
    }
    void MenuSwapLeft()
    {
        SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("Camtracker").transform.position);
        LevelUI.SetActive(true);
    }
     void MenuSwapRight()
    {
        SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("Camtracker").transform.position);
        GrowMenu.SetActive(true);
    }
    void ControlEnable()
    {
       controls.UI.Enable();  
    }
    public void CloseSkill()
    {
            Camera.SetActive(false);
            UIenable.SetActive(false);
            LevelUI.SetActive(true);
    }
    public void PrimalBarFill()
    {
      for (int i = 0; i < PrimalPoints.Length; i++)
      {
        PrimalPoints[i].SetActive(i <= (currentstats.primalLevel - 1));
      }
    }
    public void SpeedBarFill()
    {
      for (int i = 0; i < SpeedPoints.Length; i++)
      {
        SpeedPoints[i].SetActive(i <= (currentstats.speedLevel - 1));
      }
    }
     public void SentienceBarFill()
    {
      for (int i = 0; i < SentiencePoints.Length; i++)
      {
        SentiencePoints[i].SetActive(i <= (currentstats.sentienceLevel - 1));
      }
    }
    public void VitalityBarFill()
    {
      for (int i = 0; i < VitalityPoints.Length; i++)
      {
        VitalityPoints[i].SetActive(i <= (currentstats.vitalityLevel - 1));
      }
    }
    void InstantiateCurrentSpore()
    {
      if(InstantiatedSpore != null)
      {
        Destroy(InstantiatedSpore);
      }
      InstantiatedSpore = Instantiate(GameObject.FindWithTag("currentPlayer"), MenuSporeLocation.position, Quaternion.identity);

      //MenuSporePositioning sporePositioning = InstantiatedSpore.AddComponent<MenuSporePositioning>();
      //sporePositioning.verticalOffsetScalar = 0.5f;
      //sporePositioning.horizontalOffsetScalar = 0.8375f;

      Destroy(InstantiatedSpore.transform.Find("CenterPoint").gameObject);
      InstantiatedSpore.tag = "Tree";
      Destroy(InstantiatedSpore.GetComponent<Rigidbody>());
      InstantiatedSpore.layer = LayerMask.NameToLayer("UI");
      InstantiatedSpore.transform.Find("SporeModel").gameObject.layer = LayerMask.NameToLayer("UI");
      InstantiatedSpore.transform.eulerAngles = new Vector3(0f,183.53476f,0f);
      InstantiatedSpore.transform.localScale = new Vector3(3.33189845f,3.33189845f,3.33189845f);
      InstantiatedSpore.GetComponent<CapsuleCollider>().enabled = false;
      InstantiatedSpore.GetComponent<AudioSource>().enabled = false;
      InstantiatedSpore.GetComponent<DesignTracker>().enabled = false;
      InstantiatedSpore.GetComponent<CharacterStats>().enabled = false;
      InstantiatedSpore.GetComponent<WanderingSpore>().enabled = false;
      InstantiatedSpore.GetComponent<AudioSource>().enabled = false;
      InstantiatedSpore.GetComponent<SphereCollider>().enabled = false;
      InstantiatedSpore.GetComponent<SporeInteractableFinder>().enabled = false;
      InstantiatedSpore.GetComponent<SporeInteraction>().enabled = false;
    }
      public void SwapLast()
      {
        swapcharacterscript.SwitchToLastCharacter();
        InstantiateCurrentSpore();
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        UpdateUI();
        
      }
      public void SwapNext()
      {
        swapcharacterscript.SwitchToNextCharacter();
        InstantiateCurrentSpore();
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        UpdateUI();
        

      }
  
     
    public void UpdateUI()
    {
        PrimalBarFill();
        SpeedBarFill();
        SentienceBarFill();
        VitalityBarFill();
        ColorChange();
        LockedAbilities();
        ButtonListeners();
        List<Sprite> equippedSkillSprites = hudSkills.GetAllSkillSprites();
        Skill2.GetComponent<Image>().sprite = equippedSkillSprites[1];
        Skill3.GetComponent<Image>().sprite = equippedSkillSprites[2];
    }
    //Checks if Eruption has been selected as well as Y press. Checks if double skills are active and unassigns one when selected.
    public void EruptionCheck()
    {
        if(controls.UISub.AssignX.triggered && EruptionActive == true || controls.UISub.AssignYKB.triggered && EruptionActive == true)
        {
          Debug.Log("X Pressed");
           if (currentstats.equippedSkills[2] == "Eruption")
          {
            skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.UpdateHUDIcons();
            skillmanager.SetSkill("Eruption", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Eruption", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          EruptionActive = false;
          EruptionUnlocked.navigation = AutoNav;
          }
          else 
          {
          skillmanager.SetSkill("Eruption", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Eruption", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          EruptionActive = false;
          EruptionUnlocked.navigation = AutoNav;
          }
          UpdateUI();
        }
        //Checks B button press with same conditionals
        else if(controls.UISub.AssignB.triggered && EruptionActive == true || controls.UISub.AssignBKB.triggered && EruptionActive == true)
        {
          Debug.Log("B Pressed");
          
          
          if (currentstats.equippedSkills[1] == "Eruption")
          {
            skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.UpdateHUDIcons();
            skillmanager.SetSkill("Eruption", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Eruption", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          EruptionActive = false;
          EruptionUnlocked.navigation = AutoNav;
            
          }
          else
          {
            skillmanager.SetSkill("Eruption", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Eruption", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          EruptionActive = false;
          EruptionUnlocked.navigation = AutoNav;
          }
          UpdateUI();
        }
        else
        {
          return;
        }  
    }
    public void CycloneCheck()
    {
        if(controls.UISub.AssignX.triggered && LivingCycloneActive == true || controls.UISub.AssignYKB.triggered && LivingCycloneActive == true)
        {
           if (currentstats.equippedSkills[2] == "LivingCyclone")
          {
             skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.UpdateHUDIcons();
             Debug.Log("X Pressed");
          skillmanager.SetSkill("LivingCyclone", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("LivingCyclone", 1);
          
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          LivingCycloneActive = false;
          LivingCycloneUnlocked.navigation = AutoNav;
          }
          else
          {
          Debug.Log("X Pressed");
          skillmanager.SetSkill("LivingCyclone", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("LivingCyclone", 1);
          
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          LivingCycloneActive = false;
          LivingCycloneUnlocked.navigation = AutoNav;
            }
         
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && LivingCycloneActive == true || controls.UISub.AssignBKB.triggered && LivingCycloneActive == true)
        {
          
          if (currentstats.equippedSkills[1] == "LivingCyclone")
          {
             skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.UpdateHUDIcons();
            Debug.Log("B Pressed");
          skillmanager.SetSkill("LivingCyclone", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("LivingCyclone", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          LivingCycloneActive = false;
          LivingCycloneUnlocked.navigation = AutoNav;
            }
          else
          {
             skillmanager.SetSkill("LivingCyclone", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("LivingCyclone", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          LivingCycloneActive = false;
          LivingCycloneUnlocked.navigation = AutoNav;
            }
          UpdateUI();
        }
        else
        {
            return;
        }  
    }
     public void FuryCheck()
    {
        if(controls.UISub.AssignX.triggered && FuryActive == true || controls.UISub.AssignYKB.triggered && FuryActive == true)
        {
          if(currentstats.equippedSkills[1] == "RelentlessFury")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("RelentlessFury", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("RelentlessFury", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          FuryActive = false;
           RelentlessFuryUnlocked.navigation = AutoNav;
          }
          else
          {
          Debug.Log("X Pressed");
          skillmanager.SetSkill("RelentlessFury", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("RelentlessFury", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          FuryActive = false;
          RelentlessFuryUnlocked.navigation = AutoNav;
            }
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && FuryActive == true || controls.UISub.AssignBKB.triggered && FuryActive == true)
        {
          if(currentstats.equippedSkills[1] == "RelentlessFury")
          {
          skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("B Pressed");
          skillmanager.SetSkill("RelentlessFury", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("RelentlessFury", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          FuryActive = false;
          RelentlessFuryUnlocked.navigation = AutoNav;
            }
          else
          {
          Debug.Log("B Pressed");
          skillmanager.SetSkill("RelentlessFury", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("RelentlessFury", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          FuryActive = false;
          RelentlessFuryUnlocked.navigation = AutoNav;
            }
          UpdateUI();
        }
        else
        {
            return;
        }  
    }
    public void BlitzCheck()
    {
        if(controls.UISub.AssignX.triggered && BlitzActive == true || controls.UISub.AssignYKB.triggered && BlitzActive == true)
        {
          if(currentstats.equippedSkills[2] == "Blitz")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("Blitz", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Blitz", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          BlitzActive = false;
         BlitzUnlocked.navigation = AutoNav;
          }
          else
          {
          Debug.Log("X Pressed");
          skillmanager.SetSkill("Blitz", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Blitz", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          BlitzActive = false;
          BlitzUnlocked.navigation = AutoNav;
            }
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && BlitzActive == true || controls.UISub.AssignBKB.triggered && BlitzActive == true)
        {
          if(currentstats.equippedSkills[1] == "Blitz")
          {
          skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();  
          Debug.Log("B Pressed");
          skillmanager.SetSkill("Blitz", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Blitz", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          BlitzActive = false;
          BlitzUnlocked.navigation = AutoNav;
            }
          else
          {
          Debug.Log("B Pressed");
          skillmanager.SetSkill("Blitz", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Blitz", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          BlitzActive = false;
          BlitzUnlocked.navigation = AutoNav;
            }
          UpdateUI();
        }
        else
        {
            return;
        }  
    }
    public void TrophicCheck()
    {
        if(controls.UISub.AssignX.triggered && TrophicActive == true || controls.UISub.AssignYKB.triggered && TrophicActive == true)
        {
         
          if(currentstats.equippedSkills[2] == "TrophicCascade")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("TrophicCascade", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("TrophicCascade", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          TrophicActive = false;
          TrophicCascadeUnlocked.navigation = AutoNav;
          }
          else
          {
            Debug.Log("X Pressed");
          skillmanager.SetSkill("TrophicCascade", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("TrophicCascade", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          TrophicActive = false;
          TrophicCascadeUnlocked.navigation = AutoNav;
          }
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && TrophicActive == true || controls.UISub.AssignBKB.triggered && TrophicActive == true)
        {
          
          if(currentstats.equippedSkills[1] == "TrophicCascade")
          {
          skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
            Debug.Log("B Pressed");
          skillmanager.SetSkill("TrophicCascade", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("TrophicCascade", 2);
           controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          TrophicActive = false;
          TrophicCascadeUnlocked.navigation = AutoNav;
          }
         else
         {
           Debug.Log("B Pressed");
          skillmanager.SetSkill("TrophicCascade", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("TrophicCascade", 2);
           controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          TrophicActive = false;
          TrophicCascadeUnlocked.navigation = AutoNav;
            }
          UpdateUI();
        }
        else
        {
          return;
        }  
    }
    public void MycoCheck()
    {
        if(controls.UISub.AssignX.triggered && MycoActive == true || controls.UISub.AssignYKB.triggered && MycoActive == true)
        {
         
          if(currentstats.equippedSkills[2] == "Mycotoxins")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("Mycotoxins", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Mycotoxins", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          MycoActive = false;
          MycotoxinsUnlocked.navigation = AutoNav;
          }
          else
          {
            Debug.Log("X Pressed");
          skillmanager.SetSkill("Mycotoxins", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Mycotoxins", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          MycoActive = false;
          MycotoxinsUnlocked.navigation = AutoNav;
          }
          
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && MycoActive == true || controls.UISub.AssignBKB.triggered && MycoActive == true)
        {
          
          if(currentstats.equippedSkills[1] == "Mycotoxins")
          {
          skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("B Pressed");
          skillmanager.SetSkill("Mycotoxins", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Mycotoxins", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          MycoActive = false;
        MycotoxinsUnlocked.navigation=AutoNav;
          }
          else
          {
            Debug.Log("B Pressed");
          skillmanager.SetSkill("Mycotoxins", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Mycotoxins", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          MycoActive = false;
        MycotoxinsUnlocked.navigation =AutoNav;
          }
          UpdateUI();
        }
        else
        {
          return;
        }  
    }
    public void SpineCheck()
    {
        if(controls.UISub.AssignX.triggered && SpineActive == true || controls.UISub.AssignYKB.triggered && SpineActive == true)
        {
          
          if(currentstats.equippedSkills[2] == "Spineshot")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("Spineshot", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Spineshot", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          SpineActive = false;
          SpineshotUnlocked.navigation =AutoNav;
          }
          else
          {
          Debug.Log("X Pressed");
          skillmanager.SetSkill("Spineshot", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Spineshot", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          SpineActive = false;
          SpineshotUnlocked.navigation = AutoNav;
            }
          
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && SpineActive == true || controls.UISub.AssignBKB.triggered && SpineActive == true)
        {
         
          if(currentstats.equippedSkills[1] == "Spineshot")
          {
          skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("B Pressed");
          skillmanager.SetSkill("Spineshot", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Spineshot", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          SpineActive = false;
                SpineshotUnlocked.navigation = AutoNav;
          }
          else
          {
          Debug.Log("B Pressed");
          skillmanager.SetSkill("Spineshot", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Spineshot", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          SpineActive = false;
                SpineshotUnlocked.navigation = AutoNav;
          }
          UpdateUI();
        }
        else
        {
          return;
        }  
    }
    public void UnstableCheck()
    {
        if(controls.UISub.AssignX.triggered && UnstableActive == true || controls.UISub.AssignYKB.triggered && UnstableActive == true)
        {
          
          if(currentstats.equippedSkills[2] == "UnstablePuffball")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("UnstablePuffball", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("UnstablePuffball", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          UnstableActive = false;
                UnstablePuffBallUnlocked.navigation = AutoNav;
          }
          else
          {
          Debug.Log("X Pressed");
          skillmanager.SetSkill("UnstablePuffball", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("UnstablePuffball", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          UnstableActive = false;
                UnstablePuffBallUnlocked.navigation = AutoNav;
          }
          
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && UnstableActive == true || controls.UISub.AssignBKB.triggered && UnstableActive == true)
        {
          
          if(currentstats.equippedSkills[1] == "UnstablePuffball")
          {
             skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
            Debug.Log("B Pressed");
          skillmanager.SetSkill("UnstablePuffball", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("UnstablePuffball", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          UnstableActive = false;
                UnstablePuffBallUnlocked.navigation = AutoNav;
          }
          else
          {
             Debug.Log("B Pressed");
          skillmanager.SetSkill("UnstablePuffball", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("UnstablePuffball", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          UnstableActive = false;
                UnstablePuffBallUnlocked.navigation = AutoNav;
          }
          
          UpdateUI();
        }
        else
        {
          return;
        }  
    }
    public void UndergrowthCheck()
    {
        if(controls.UISub.AssignX.triggered && UndergrowthActive == true || controls.UISub.AssignYKB.triggered && UndergrowthActive == true)
        {
          
          if(currentstats.equippedSkills[2] == "Undergrowth")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("Undergrowth", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Undergrowth", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          UndergrowthActive = false;
            UndergrowthUnlocked.navigation = AutoNav;
          }
          else
          {
          Debug.Log("X Pressed");
          skillmanager.SetSkill("Undergrowth", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Undergrowth", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          UndergrowthActive = false;
                UndergrowthUnlocked.navigation = AutoNav;
          }
          
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && UndergrowthActive == true || controls.UISub.AssignBKB.triggered && UndergrowthActive == true)
        {
          
          if(currentstats.equippedSkills[1] == "Undergrowth")
          {
          skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
            Debug.Log("B Pressed");
          skillmanager.SetSkill("Undergrowth", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Undergrowth", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          UndergrowthActive = false;
        UndergrowthUnlocked.navigation=AutoNav;
          }
          else
          {
             Debug.Log("B Pressed");
          skillmanager.SetSkill("Undergrowth", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Undergrowth", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          UndergrowthActive = false;
        UndergrowthUnlocked.navigation = AutoNav;
          }
          
          UpdateUI();
        }
        else
        {
          return;
        }  
    }
    public void LeechCheck()
    {
        if(controls.UISub.AssignX.triggered && LeechActive == true || controls.UISub.AssignYKB.triggered && LeechActive == true)
        {
          
          if(currentstats.equippedSkills[2] == "LeechingSpore")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("LeechingSpore", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("LeechingSpore", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          LeechActive = false;
          LeechingSporesUnlocked.navigation = AutoNav;
          }
          else
          {
            Debug.Log("X Pressed");
          skillmanager.SetSkill("LeechingSpore", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("LeechingSpore", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          LeechActive = false;
                LeechingSporesUnlocked.navigation = AutoNav;
          }
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && LeechActive == true || controls.UISub.AssignBKB.triggered && LeechActive == true)
        {
          
          if(currentstats.equippedSkills[1] == "LeechingSpore")
          {
          skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();  
            Debug.Log("B Pressed");
          skillmanager.SetSkill("LeechingSpore", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("LeechingSpore", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          LeechActive = false;
                LeechingSporesUnlocked.navigation = AutoNav;
          }
          else
          {
             Debug.Log("B Pressed");
          skillmanager.SetSkill("LeechingSpore", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("LeechingSpore", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          LeechActive = false;
                LeechingSporesUnlocked.navigation = AutoNav;
          }
          
          UpdateUI();
        }
        else
        {
          return;
        }  
    }
    public void SporeburstCheck()
    {
        if(controls.UISub.AssignX.triggered && SporeburstActive == true || controls.UISub.AssignYKB.triggered && SporeburstActive == true)
        {
          
          if(currentstats.equippedSkills[2] == "Sporeburst")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("Sporeburst", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Sporeburst", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          SporeburstActive = false;
        SporeburstUnlocked.navigation = AutoNav;
          }
          else
          {
            Debug.Log("X Pressed");
          skillmanager.SetSkill("Sporeburst", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Sporeburst", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          SporeburstActive = false;
                SporeburstUnlocked.navigation = AutoNav;
          }
          
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && SporeburstActive == true || controls.UISub.AssignBKB.triggered && SporeburstActive == true)
        {
          
          if(currentstats.equippedSkills[1] == "Sporeburst")
          {
            skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();  
            Debug.Log("B Pressed");
          skillmanager.SetSkill("Sporeburst", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Sporeburst", 2);
           controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          SporeburstActive = false;
                SporeburstUnlocked.navigation = AutoNav;
          }
          else
          {
            Debug.Log("B Pressed");
          skillmanager.SetSkill("Sporeburst", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Sporeburst", 2);
           controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          SporeburstActive = false;
                SporeburstUnlocked.navigation = AutoNav;
          }
         
          UpdateUI();
        }
        else
        {
          return;
        }  
    }
    public void DefenseMechCheck()
    {
        if(controls.UISub.AssignX.triggered && DefenseMechActive == true || controls.UISub.AssignYKB.triggered && DefenseMechActive == true)
        {
         
          if(currentstats.equippedSkills[2] == "DefenseMechanism")
          {
          skillmanager.SetSkill("NoSkill", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();
          Debug.Log("X Pressed");
          skillmanager.SetSkill("DefenseMechanism", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("DefenseMechanism", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          DefenseMechActive = false;
                DefenseMechanismUnlocked.navigation = AutoNav;
          }
          else
          {
             Debug.Log("X Pressed");
          skillmanager.SetSkill("DefenseMechanism", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("DefenseMechanism", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          DefenseMechActive = false;
                DefenseMechanismUnlocked.navigation = AutoNav;
          }
          
          UpdateUI();
          
        }
        else if(controls.UISub.AssignB.triggered && DefenseMechActive == true || controls.UISub.AssignBKB.triggered && DefenseMechActive == true)
        {
          
          if(currentstats.equippedSkills[1] == "DefenseMechanism")
          {
            skillmanager.SetSkill("NoSkill", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.UpdateHUDIcons();  
            Debug.Log("B Pressed");
          skillmanager.SetSkill("DefenseMechanism", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("DefenseMechanism", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          DefenseMechActive = false;
                DefenseMechanismUnlocked.navigation = AutoNav;
          }
          else
          {
             Debug.Log("B Pressed");
          skillmanager.SetSkill("DefenseMechanism", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("DefenseMechanism", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          DefenseMechActive = false;
                DefenseMechanismUnlocked.navigation = AutoNav;
          }
          
          UpdateUI();
        }
        else
        {
          return;
        }  
    }
     void Update()
    {
        List<Sprite> equippedSkillSprites = hudSkills.GetAllSkillSprites();
        Skill2.GetComponent<Image>().sprite = equippedSkillSprites[1];
        Skill3.GetComponent<Image>().sprite = equippedSkillSprites[2];
        EruptionCheck();
        CycloneCheck();
        FuryCheck();
        BlitzCheck();
        TrophicCheck();
        MycoCheck();
        SpineCheck();
        UnstableCheck();
        UndergrowthCheck();
        SporeburstCheck();
        LeechCheck();
        DefenseMechCheck();
        SkillDescriptions();
    }

    public void EruptionsSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        EruptionActive = true;
        EruptionUnlocked.navigation = NoneNav;
     
    }
    //Set Living Cyclone Skill slot 1 or 2
    public void LivingCycloneSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        LivingCycloneActive = true;
        LivingCycloneUnlocked.navigation = NoneNav;
        
    }
    
    //Set Relentless Fury Skill slot 1 or 2
    public void RelentlessFurySlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        FuryActive = true;
        RelentlessFuryUnlocked.navigation = NoneNav;    
        
    }
    //Set Spine Shot skill slot 1 or 2  
    public void SpineShotSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        SpineActive = true;
        SpineshotUnlocked.navigation = NoneNav;
        
    }
    //Set Undergrowth Skill slot 1 or 2
     public void UndergrowthSlot1()
    {
       controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        UndergrowthActive = true;
        UndergrowthUnlocked.navigation = NoneNav;
       
    }
    //Set Unstable Puff Ball Skill slot 1 or 2
     public void UnstablePuffballSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        UnstableActive = true;
        UnstablePuffBallUnlocked.navigation = NoneNav;
    }
    //Sets blitz skill for slot 1 or 2
     public void BlitzSlot1()
    {
         controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        BlitzActive = true;
        BlitzUnlocked.navigation = NoneNav;
        
    }
    //Sets Mycotoxin for slot 1 or 2
     public void MycotoxinsSlot1()
    {
       controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        MycoActive = true;
        MycotoxinsUnlocked.navigation = NoneNav;
    }

    //Sets Trophic Cascade for slot 1 or 2
    public void TrophicCascadeSlot1()
    {
         controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        TrophicActive = true;
        TrophicCascadeUnlocked.navigation = NoneNav;
        
    }
    //Set Defense Mechanism for slot1 or 2
     public void DefenseMechanismSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        DefenseMechActive = true;
        DefenseMechanismUnlocked.navigation = NoneNav;
        
    }

    //Set leeching spores for slot 1 or 2
    public void LeechingSporeSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        LeechActive = true;
        LeechingSporesUnlocked.navigation = NoneNav;
    }

    //Set Spore burst for slot 1 or 2
    public void SporeBurstSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        SporeburstActive = true;
        SporeburstUnlocked.navigation = NoneNav;
    }
   
    public void ColorChange()
    {
      Color defaultcolor = new Color32(25,43,29,255);
      ColorBlock defaultcb = EruptionUnlocked.colors;
      defaultcb.normalColor = defaultcolor;
      EruptionUnlocked.colors = defaultcb;
      LivingCycloneUnlocked.colors = defaultcb;
      RelentlessFuryUnlocked.colors = defaultcb;
      BlitzUnlocked.colors = defaultcb;
      TrophicCascadeUnlocked.colors = defaultcb;
      MycotoxinsUnlocked.colors = defaultcb;
      SpineshotUnlocked.colors = defaultcb;
      UnstablePuffBallUnlocked.colors = defaultcb;
      UndergrowthUnlocked.colors = defaultcb;
      LeechingSporesUnlocked.colors = defaultcb;
      SporeburstUnlocked.colors = defaultcb;
      DefenseMechanismUnlocked.colors = defaultcb;

    }
    public void LockedAbilities()
    {
      ColorChange();
      Color newColor = new Color32(241, 251, 56, 255);
        ColorBlock cb1 = EruptionUnlocked.colors;
        cb1.normalColor = newColor;
        ColorBlock cb2 = LivingCycloneUnlocked.colors;
        cb2.normalColor = newColor;
        ColorBlock cb3 = RelentlessFuryUnlocked.colors;
        cb3.normalColor = newColor;
        ColorBlock cb4 = BlitzUnlocked.colors;
        cb4.normalColor = newColor;
        ColorBlock cb5 = TrophicCascadeUnlocked.colors;
        cb5.normalColor = newColor;
        ColorBlock cb6 = MycotoxinsUnlocked.colors;
        cb6.normalColor = newColor;
        ColorBlock cb7 = SpineshotUnlocked.colors;
        cb7.normalColor = newColor;
        ColorBlock cb8 = UnstablePuffBallUnlocked.colors;
        cb8.normalColor = newColor;
        ColorBlock cb9 = UndergrowthUnlocked.colors;
        cb9.normalColor = newColor;
        ColorBlock cb10 = LeechingSporesUnlocked.colors;
        cb10.normalColor = newColor;
        ColorBlock cb11 = SporeburstUnlocked.colors;
        cb11.normalColor = newColor;
        ColorBlock cb12 = DefenseMechanismUnlocked.colors;
        cb12.normalColor = newColor;
      switch(currentstats.equippedSkills[1])
      {
        case "Eruption":
        EruptionUnlocked.colors = cb1;
        break;
        case"LivingCyclone":
        LivingCycloneUnlocked.colors = cb2;
        break;
        case "RelentlessFury":
        RelentlessFuryUnlocked.colors = cb3;
        break;
        case "Blitz":
        BlitzUnlocked.colors = cb4;
        break;
        case "TrophicCascade":
        TrophicCascadeUnlocked.colors = cb5;
        break;
        case "Mycotoxins":
        MycotoxinsUnlocked.colors = cb6;
        break;
        case "Spineshot":
        SpineshotUnlocked.colors = cb7;
        break;
        case "UnstablePuffball":
        UnstablePuffBallUnlocked.colors = cb8;
        break;
        case "Undergrowth":
        UndergrowthUnlocked.colors = cb9;
        break;
        case "LeechingSpore":
        LeechingSporesUnlocked.colors = cb10;
        break;
        case "Sporeburst":
        SporeburstUnlocked.colors = cb11;
        break;
        case "DefenseMechanism":
        DefenseMechanismUnlocked.colors = cb12;
        break;
        default:
        return;
      }
    switch(currentstats.equippedSkills[2])
      {
        case "Eruption":
        EruptionUnlocked.colors = cb1;
        break;
        case"LivingCyclone":
        LivingCycloneUnlocked.colors = cb2;
        break;
        case "RelentlessFury":
        RelentlessFuryUnlocked.colors = cb3;
        break;
        case "Blitz":
        BlitzUnlocked.colors = cb4;
        break;
        case "TrophicCascade":
        TrophicCascadeUnlocked.colors = cb5;
        break;
        case "Mycotoxins":
        MycotoxinsUnlocked.colors = cb6;
        break;
        case "Spineshot":
        SpineshotUnlocked.colors = cb7;
        break;
        case "UnstablePuffball":
        UnstablePuffBallUnlocked.colors = cb8;
        break;
        case "Undergrowth":
        UndergrowthUnlocked.colors = cb9;
        break;
        case "LeechingSpore":
        LeechingSporesUnlocked.colors = cb10;
        break;
        case "Sporeburst":
        SporeburstUnlocked.colors = cb11;
        break;
        case "DefenseMechanism":
        DefenseMechanismUnlocked.colors = cb12;
        break;
        default:
        return;
      }
    }
    public void ButtonListeners()
    {
        if (currentstats.primalLevel >= 5)
        {
            EruptionUnlocked.onClick.AddListener(EruptionsSlot1);
            EruptionLock.SetActive(false);
        }
        else
        {
            EruptionUnlocked.onClick.RemoveListener(EruptionsSlot1);
            EruptionLock.SetActive(true);
        }


        if (currentstats.primalLevel >= 10)
        {
            LivingCycloneUnlocked.onClick.AddListener(LivingCycloneSlot1);
            LivingCycloneLock.SetActive(false);
        }
        else
        {
            LivingCycloneUnlocked.onClick.RemoveListener(LivingCycloneSlot1);
            LivingCycloneLock.SetActive(true);
        }


        if (currentstats.primalLevel >= 15)
        {
            RelentlessFuryUnlocked.onClick.AddListener(RelentlessFurySlot1);
            RelentlessFuryLock.SetActive(false);

        }
        else
        {
            RelentlessFuryUnlocked.onClick.RemoveListener(RelentlessFurySlot1);
            RelentlessFuryLock.SetActive(true);
        }


        //Speed Unlock Statements
        if (currentstats.speedLevel >= 5)
        {
            BlitzUnlocked.onClick.AddListener(BlitzSlot1);
            BlitzLock.SetActive(false);
        }
        else
        {
            BlitzUnlocked.onClick.RemoveListener(BlitzSlot1);
            BlitzLock.SetActive(true);
        }


        if (currentstats.speedLevel >= 10)
        {
            TrophicCascadeUnlocked.onClick.AddListener(TrophicCascadeSlot1);
            TrophicCascadeLock.SetActive(false);
        }
        else
        {
            TrophicCascadeUnlocked.onClick.RemoveListener(TrophicCascadeSlot1);
            TrophicCascadeLock.SetActive(true);
        }


        if (currentstats.speedLevel >= 15)
        {
            MycotoxinsUnlocked.onClick.AddListener(MycotoxinsSlot1);
            MycoToxLock.SetActive(false);
        }
        else
        {
            MycotoxinsUnlocked.onClick.RemoveListener(MycotoxinsSlot1);
            MycoToxLock.SetActive(true);
        }


        //Sentience Unlock Statements
        if (currentstats.sentienceLevel >= 5)
        {
            SpineshotUnlocked.onClick.AddListener(SpineShotSlot1);
            Spineshotlock.SetActive(false);
        }
        else
        {
            SpineshotUnlocked.onClick.RemoveListener(SpineShotSlot1);
            Spineshotlock.SetActive(true);
        }

        if (currentstats.sentienceLevel >= 10)
        {
            UnstablePuffBallUnlocked.onClick.AddListener(UnstablePuffballSlot1);
            UnstableLock.SetActive(false);
        }
        else
        {
            UnstablePuffBallUnlocked.onClick.RemoveListener(UnstablePuffballSlot1);
            UnstableLock.SetActive(true);
        }

        if (currentstats.sentienceLevel >= 15)
        {
            UndergrowthUnlocked.onClick.AddListener(UndergrowthSlot1);
            UndergrowthLock.SetActive(false);
        }
        else
        {
            UndergrowthUnlocked.onClick.RemoveListener(UndergrowthSlot1);
            UndergrowthLock.SetActive(true);
        }


        //Vitality unlock statements
        if (currentstats.vitalityLevel >= 5)
        {
            LeechingSporesUnlocked.onClick.AddListener(LeechingSporeSlot1);
            LeechingLock.SetActive(false);
        }
        else
        {
            LeechingSporesUnlocked.onClick.RemoveListener(LeechingSporeSlot1);
            LeechingLock.SetActive(true);
        }

        if (currentstats.vitalityLevel >= 10)
        {
            SporeburstUnlocked.onClick.AddListener(SporeBurstSlot1);
            SporeburstLock.SetActive(false);
        }
        else
        {
            SporeburstUnlocked.onClick.RemoveListener(SporeBurstSlot1);
            SporeburstLock.SetActive(true);
        }

        if (currentstats.vitalityLevel >= 15)
        {
            DefenseMechanismUnlocked.onClick.AddListener(DefenseMechanismSlot1);
            DefenseMechLock.SetActive(false);
        }
        else
        {
            DefenseMechanismUnlocked.onClick.RemoveListener(DefenseMechanismSlot1);
            DefenseMechLock.SetActive(true);
        }
    }
  public void EruptionSelect()
  {
    ErupDescrip = true;
  }
  public void EruptionDeSelect()
  {
    ErupDescrip = false;
  }
  public void LivingCycloneSelect()
  {
    LCDescrip = true;
  }
  public void LivingCycloneDeselect()
  {
    LCDescrip = false;
  }
  public void RelentlessFurySelect()
  {
    RelentDescrip = true;
  }
  public void RelentlessFuryDeSelect()
  {
    RelentDescrip = false;
  }
  public void BlitzSelect()
  {
    BlitzDescrip = true;
  }
  public void BlitzDeSelect()
  {
    BlitzDescrip = false;
  }
  public void TrophicSelect()
  {
    TrophicDescrip = true;
  }
  public void TrophicDeSelect()
  {
    TrophicDescrip = false;
  }
  public void MycotoxinsSelect()
  {
    MycoDescrip = true;
  }
  public void MycotoxinsDeSelect()
  {
    MycoDescrip = false;
  }
  public void SpineShotSelect()
  {
    SpineDescrip = true;
  }
  public void SpineShotDeSelect()
  {
    SpineDescrip = false;
  }
  public void UnstablePuffSelect()
  {
    UnstableDescrip = true;
  }
  public void UnstablePuffDeselect()
  {
    UnstableDescrip = false;
  }
  public void UndergrowthSelect()
  {
    UnderDescrip = true;
  }
  public void UndergrowthDeSelect()
  {
    UnderDescrip = false;
  }
  public void LeechingSporeSelect()
  {
    LeechDescrip = true;
  }
  public void LeechingSporeDeSelect()
  {
    LeechDescrip = false;
  }
  public void SporeshotSelect()
  {
    SporeDescrip = true;
  }
  public void SporeShotDeSelect()
  {
    SporeDescrip = false; 
  }
  public void DefenseMechSelect()
  {
    DefenseDescrip = true;
  }
  public void DefenseMechDeSelect()
  {
    DefenseDescrip = false;
  }
  public void CharacterbuttonSelect()
  {
    CharacterButtonsSelected = true;
  }
  public void CharacterbuttonDeselect()
  {
    CharacterButtonsSelected = false;
  }
  public void SkillDescriptions()
  {
    if(controls.UI.MoreInfo.triggered && DescriptionActive == false && CharacterButtonsSelected == false)
    {
      DescriptionPanel.SetActive(true);
      DescriptionActive = true;
      DescriptionPanelHolder.SetActive(true);
      controls.UISub.Enable();
      controls.UI.Disable();
      if(ErupDescrip == true)
      {
        DescriptionText.text = "Eruption: <br> <size=25>Stomp the ground with primal strength <br> dealing damage to all enemies around you.<br> Deals additional damage to enemies closer to you.";
        EruptionUnlocked.navigation = NoneNav;
        EruptionUnlocked.onClick.RemoveListener(EruptionsSlot1);
      }
      else if(LCDescrip == true)
      {
        DescriptionText.text = "Living Cyclone: <br> <size=25>Spin relentlessly striking all enemies<br> around you with your currently equipped weapon. <br> You are able to move while Living Cyclone is active.";
        LivingCycloneUnlocked.navigation = NoneNav;
        LivingCycloneUnlocked.onClick.RemoveListener(LivingCycloneSlot1);
      }
      else if(RelentDescrip == true)
      {
        DescriptionText.text = "Relentless Fury: <br> <size=25>Go into a frenzy gaining 30% attack speed. While active lose 5% of current health every second. Attacks restore health equal to 25%<br> of weapon damage.";
        RelentlessFuryUnlocked.navigation = NoneNav; 
        RelentlessFuryUnlocked.onClick.RemoveListener(RelentlessFurySlot1);
      }
      else if (BlitzDescrip == true)
      {
        DescriptionText.text = "Blitz: <br> <size=25>Dash in a straight line damaging all enemies hit.<br> If an enemy is hit the cooldown of Blitz<br> is reduced by 50%.";
        BlitzUnlocked.navigation = NoneNav;
        BlitzUnlocked.onClick.RemoveListener(BlitzSlot1);
      }
      else if (TrophicDescrip == true)
      {
        DescriptionText.text = "Trophic Cascade: <br> <size=25>Release a flurry of attacks slashing<br> all enemies around you.";
        TrophicCascadeUnlocked.navigation = NoneNav;
        TrophicCascadeUnlocked.onClick.RemoveListener(TrophicCascadeSlot1);
      }
      else if (MycoDescrip == true)
      {
        DescriptionText.text = "Mycotoxins: <br> <size=25>Gain 50% bonus movement speed and release a trail of spores behind you. Enemies hit by these spores are damaged.";
        MycotoxinsUnlocked.navigation = NoneNav;
        MycotoxinsUnlocked.onClick.RemoveListener(MycotoxinsSlot1);
      }
      else if (SpineDescrip == true)
      {
        DescriptionText.text = "Spineshot: <br> <size=25>Fire out a spine damaging the first enemy hit.";
        SpineshotUnlocked.navigation = NoneNav;
        SpineshotUnlocked.onClick.RemoveListener(SpineShotSlot1);
      }
      else if (UnstableDescrip == true)
      {
        DescriptionText.text = "Unstable Puffball: <br><size=25>Fires a puffball that explodes and damages all enemies upon contact.";
        UnstablePuffBallUnlocked.navigation = NoneNav;
        UnstablePuffBallUnlocked.onClick.RemoveListener(UnstablePuffballSlot1);
      }
      else if (UnderDescrip == true)
      {
        DescriptionText.text = "Undergrowth: <br><size=25> An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit.";
        UndergrowthUnlocked.navigation = NoneNav;
        UndergrowthUnlocked.onClick.RemoveListener(UndergrowthSlot1);
      }
      else if(LeechDescrip == true)
      {
        DescriptionText.text = "Leeching Spores: <br><size=25>Infest a nearby enemy with a leeching spore. The spore steals health every second from the enemy and restores it to you.";
        LeechingSporesUnlocked.navigation = NoneNav;
        LeechingSporesUnlocked.onClick.RemoveListener(LeechingSporeSlot1);
      }
      else if(SporeDescrip == true)
      {
        DescriptionText.text = "Sporeburst: <br><size=25>Spores explode from you stunning and damaging all enemies caught in its radius. Heal for 50% of all damage dealt.";
        SporeburstUnlocked.navigation = NoneNav;
        SporeburstUnlocked.onClick.RemoveListener(SporeBurstSlot1);
      }
      else if (DefenseDescrip == true)
      {
        DescriptionText.text = "Defense Mechanism: <br><size=25>Reduces damage taken by 50% for 1 second. Attacks against you while Defense Mechanism is active is stored as bonus damage on your next attack equal to 50% of the damage absorbed.";
        DefenseMechanismUnlocked.navigation = NoneNav;
        DefenseMechanismUnlocked.onClick.RemoveListener(DefenseMechanismSlot1);
      }
      }
      if(controls.UISub.Cancel.triggered && DescriptionActive == true)
      {
        DescriptionPanel.SetActive(false);
        controls.UISub.Disable();
        controls.UI.Enable();
        EruptionUnlocked.navigation = AutoNav;
        LivingCycloneUnlocked.navigation = AutoNav;
        RelentlessFuryUnlocked.navigation = AutoNav;
        BlitzUnlocked.navigation = AutoNav;
        TrophicCascadeUnlocked.navigation = AutoNav;
        MycotoxinsUnlocked.navigation = AutoNav;
        SpineshotUnlocked.navigation = AutoNav;
        UnstablePuffBallUnlocked.navigation = AutoNav;
        UndergrowthUnlocked.navigation = AutoNav;
        LeechingSporesUnlocked.navigation = AutoNav;
        SporeburstUnlocked.navigation = AutoNav;
        DefenseMechanismUnlocked.navigation = AutoNav;
        DescriptionActive = false;
        DescriptionPanelHolder.SetActive(false);
        ButtonListeners();
    }
  }
  }
