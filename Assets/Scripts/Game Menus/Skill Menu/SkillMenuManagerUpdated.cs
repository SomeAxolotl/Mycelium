using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class SkillMenuManagerUpdated : MonoBehaviour
{
    public Button Skill1;
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

    //DM Buttons
    public Button DefenseMechanismUnlocked;
    public GameObject Skill2;
    public GameObject Skill3;
    void OnEnable()
    {
        LevelUI.SetActive(false);
        controls = new ThirdPersonActionsAsset();
        controls.UI.MenuSwapL.performed += ctx => MenuSwapLeft();
        controls.UI.Close.performed += ctx => CloseSkill();
        controls.UI.MenuSwapR.performed += ctx => MenuSwapRight();
        Skill1.Select();
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        UpdateUI();
        Invoke("ControlEnable", 0.25f);
        GrowMenu.SetActive(false);
        
        
    }
   
     void OnDisable()
    {
      controls.UI.Disable();
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
    public void UpdateUI()
    {
        PrimalBarFill();
        SpeedBarFill();
        SentienceBarFill();
        VitalityBarFill();
        List<Sprite> equippedSkillSprites = hudSkills.GetAllSkillSprites();
        Skill1.GetComponent<Image>().sprite = equippedSkillSprites[1];
        Skill2.GetComponent<Image>().sprite = equippedSkillSprites[2];
    }
     void Update()
    {
        List<Sprite> equippedSkillSprites = hudSkills.GetAllSkillSprites();
        Skill1.GetComponent<Image>().sprite = equippedSkillSprites[1];
        Skill2.GetComponent<Image>().sprite = equippedSkillSprites[2];
        if (Input.GetKeyDown(KeyCode.LeftBracket))
      {
        MenuSwapLeft();
      }
      if(Input.GetKeyDown(KeyCode.RightBracket))
      {
        MenuSwapRight();
      }
    }
}
