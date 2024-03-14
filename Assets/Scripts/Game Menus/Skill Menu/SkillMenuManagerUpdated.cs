using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class SkillMenuManagerUpdated : MonoBehaviour
{
    
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
   
    void OnEnable()
    {
        
        LevelUI.SetActive(false);
        controls = new ThirdPersonActionsAsset();
        controls.UI.MenuSwapL.performed += ctx => MenuSwapLeft();
        controls.UI.Close.performed += ctx => CloseSkill();
        controls.UI.MenuSwapR.performed += ctx => MenuSwapRight();
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        swapcharacterscript = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
        UpdateUI();
        Invoke("ControlEnable", 0.25f);
        GrowMenu.SetActive(false);
        Camera.SetActive(true);
        
        EruptionUnlocked.Select();
        Invoke("InstantiateCurrentSpore", 0.01f);
        //Primal Unlock Statements
        
        
    }
   
     void OnDisable()
    {
      controls.UI.Disable();
      Destroy(InstantiatedSpore);
    }
    void MenuSwapLeft()
    {
        SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("MainCamera").transform.position);
        LevelUI.SetActive(true);
    }
     void MenuSwapRight()
    {
        SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("MainCamera").transform.position);
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
      Destroy(InstantiatedSpore.transform.Find("CenterPoint").gameObject);
      InstantiatedSpore.tag = "Tree";
      Destroy(InstantiatedSpore.GetComponent<Rigidbody>());
      InstantiatedSpore.layer = LayerMask.NameToLayer("MenuSpore");
      InstantiatedSpore.transform.Find("SporeModel").gameObject.layer = LayerMask.NameToLayer("MenuSpore");
      InstantiatedSpore.transform.eulerAngles = new Vector3(0f,183.53476f,0f);
      InstantiatedSpore.transform.localScale = new Vector3(3.33189845f,3.33189845f,3.33189845f);
      InstantiatedSpore.GetComponent<CapsuleCollider>().enabled = false;
      InstantiatedSpore.GetComponent<AudioSource>().enabled = false;
      InstantiatedSpore.GetComponent<DesignTracker>().enabled = false;
      InstantiatedSpore.GetComponent<CharacterStats>().enabled = false;
      InstantiatedSpore.GetComponent<IdleWalking>().enabled = false;
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
          }
          else 
          {
          skillmanager.SetSkill("Eruption", 1, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Eruption", 1);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          EruptionActive = false;
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
            
          }
          else
          {
            skillmanager.SetSkill("Eruption", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("Eruption", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          EruptionActive = false;
          }
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
          
          }
          else
          {
             skillmanager.SetSkill("LivingCyclone", 2, GameObject.FindWithTag("currentPlayer"));
          hudSkills.ChangeSkillIcon("LivingCyclone", 2);
          controls.UISub.Disable();
          controls.UI.Enable();
          PanelHolder.SetActive(false);
          LivingCycloneActive = false;
          }
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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

          }
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
          }
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
         }
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
          }
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
          }
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
          }
          
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
          }
          
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
          }
          
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
          }
         
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
          }
          
          UpdateUI();
        }
        else
        {
          Debug.Log("return");
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
        if (Input.GetKeyDown(KeyCode.LeftBracket))
      {
        MenuSwapLeft();
      }
      if(Input.GetKeyDown(KeyCode.RightBracket))
      {
        MenuSwapRight();
      }
      if(currentstats.primalLevel >= 5)
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

        
        if(currentstats.primalLevel >= 15)
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
        if(currentstats.speedLevel >= 5)
        {
            BlitzUnlocked.onClick.AddListener(BlitzSlot1);
            BlitzLock.SetActive(false);
        }
        else
        {
            BlitzUnlocked.onClick.RemoveListener(BlitzSlot1);
            BlitzLock.SetActive(true);
        }

        
        if(currentstats.speedLevel >= 10)
        {
            TrophicCascadeUnlocked.onClick.AddListener(TrophicCascadeSlot1);
            TrophicCascadeLock.SetActive(false);
        }
        else
        {
            TrophicCascadeUnlocked.onClick.RemoveListener(TrophicCascadeSlot1);
            TrophicCascadeLock.SetActive(true);
        }


        if(currentstats.speedLevel >= 15)
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
        if(currentstats.sentienceLevel >= 5)
        {
            SpineshotUnlocked.onClick.AddListener(SpineShotSlot1);
            Spineshotlock.SetActive(false);
        }
        else
        {
            SpineshotUnlocked.onClick.RemoveListener(SpineShotSlot1);
            Spineshotlock.SetActive(true);
        }

        if(currentstats.sentienceLevel >= 10)
        {
            UnstablePuffBallUnlocked.onClick.AddListener(UnstablePuffballSlot1);
            UnstableLock.SetActive(false);
        }
        else
        {
            UnstablePuffBallUnlocked.onClick.RemoveListener(UnstablePuffballSlot1);
            UnstableLock.SetActive(true);
        }

        if(currentstats.sentienceLevel >= 15)
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
        if(currentstats.vitalityLevel >= 5)
        {
            LeechingSporesUnlocked.onClick.AddListener(LeechingSporeSlot1);
            LeechingLock.SetActive(false);
        }
        else
        {
            LeechingSporesUnlocked.onClick.RemoveListener(LeechingSporeSlot1);
            LeechingLock.SetActive(true);
        }

        if(currentstats.vitalityLevel >= 10)
        {
            SporeburstUnlocked.onClick.AddListener(SporeBurstSlot1);
            SporeburstLock.SetActive(false);
        }
        else
        {
            SporeburstUnlocked.onClick.RemoveListener(SporeBurstSlot1);
            SporeburstLock.SetActive(true);
        }

        if(currentstats.vitalityLevel >= 15)
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

    public void EruptionsSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        EruptionActive = true;
     
    }
    //Set Living Cyclone Skill slot 1 or 2
    public void LivingCycloneSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        LivingCycloneActive = true;
        
    }
    
    //Set Relentless Fury Skill slot 1 or 2
    public void RelentlessFurySlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        FuryActive = true;
        
    }
    //Set Spine Shot skill slot 1 or 2  
    public void SpineShotSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        SpineActive = true;
        
    }
    //Set Undergrowth Skill slot 1 or 2
     public void UndergrowthSlot1()
    {
       controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        UndergrowthActive = true;
       
    }
    //Set Unstable Puff Ball Skill slot 1 or 2
     public void UnstablePuffballSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        UnstableActive = true;
        
    }
    //Sets blitz skill for slot 1 or 2
     public void BlitzSlot1()
    {
         controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        BlitzActive = true;
        
    }
    //Sets Mycotoxin for slot 1 or 2
     public void MycotoxinsSlot1()
    {
       controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        MycoActive = true;
        
    }

    //Sets Trophic Cascade for slot 1 or 2
    public void TrophicCascadeSlot1()
    {
         controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        TrophicActive = true;
        
    }
    //Set Defense Mechanism for slot1 or 2
     public void DefenseMechanismSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        DefenseMechActive = true;
        
    }

    //Set leeching spores for slot 1 or 2
    public void LeechingSporeSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        LeechActive = true;
        
    }

    //Set Spore burst for slot 1 or 2
    public void SporeBurstSlot1()
    {
        controls.UI.Disable();
        controls.UISub.Enable();
        PanelHolder.SetActive(true);
        SporeburstActive = true;
        
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
        break;
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
        break;
      }
    }
}
