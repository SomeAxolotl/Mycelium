using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
public class SkillMenuManager : MonoBehaviour
{
    public Button Skill1;
    public Button Skill2;
    public Button Skill3;
    public GameObject Skill1ListEnable;
    public GameObject Skill2ListEnable;
    public GameObject LevelUI;
    public Button Skill1ListButton;
    public Button Skill2ListButton;
    public GameObject UIenable;
    ThirdPersonActionsAsset controls;
    private CanvasGroup HUDCanvasGroup;
    private HUDSkills hudSkills;
    public SkillManager skillmanager;
    public GameObject Skill1Image;
    public GameObject Skill2Image;
    public GameObject Skill3Image;
    private PlayerController playerController;
    public GameObject SkillDescription;
    public GameObject GrowMenu;

    
    
    void OnEnable()
    {
        LevelUI.SetActive(false);
        controls = new ThirdPersonActionsAsset();
        controls.UI.MenuSwapL.performed += ctx => MenuSwapLeft();
        controls.UI.Close.performed += ctx => CloseSkill();
        controls.UI.MenuSwapR.performed += ctx => MenuSwapRight();
        Skill1.Select();
        Skill1ListEnable.SetActive(false);  
        Skill2ListEnable.SetActive(false);
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        SpriteUpdate();
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
   
    public void Skill1Select()
    {
        Skill1ListEnable.SetActive(true);
        Skill1ListButton.Select();
    }
    public void Skill2Select()
    {
        Skill2ListEnable.SetActive(true);
        Skill2ListButton.Select();
    }
    public void CloseSkill()
    {
        if(Skill1ListEnable.activeInHierarchy == true)
        {
            Skill1ListEnable.SetActive(false);
            Skill2.Select();
        }
        else if (Skill2ListEnable.activeInHierarchy == true)
        {
            Skill2ListEnable.SetActive(false);
            Skill3.Select();
        }
        else
        {
            UIenable.SetActive(false);
            LevelUI.SetActive(true);
        }
    }

    //Set Eruption Skill slot 1 or 2
    public void EruptionsSlot1()
    {
        skillmanager.SetSkill("Eruption", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Eruption", 1);
        SpriteUpdate();
    }
     public void EruptionsSlot2()
    {
        skillmanager.SetSkill("Eruption", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Eruption", 2);
        SpriteUpdate();
    }
    
    //Set Living Cyclone Skill slot 1 or 2
    public void LivingCycloneSlot1()
    {
        skillmanager.SetSkill("LivingCyclone", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("LivingCyclone", 1);
        SpriteUpdate();
    }
     public void LivingCycloneSlot2()
    {
        skillmanager.SetSkill("LivingCyclone", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("LivingCyclone", 2);
        SpriteUpdate();
    }
    
    //Set Relentless Fury Skill slot 1 or 2
    public void RelentlessFurySlot1()
    {
        skillmanager.SetSkill("RelentlessFury", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("RelentlessFury", 1);
        SpriteUpdate();
    }
     public void RelentlessFurySlot2()
    {
        skillmanager.SetSkill("RelentlessFury", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("RelentlessFury", 2);
        SpriteUpdate();
    }

    //Set Spine Shot skill slot 1 or 2  
    public void SpineShotSlot1()
    {
        skillmanager.SetSkill("Spineshot", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Spineshot", 1);
        SpriteUpdate();
    }
    public void SpineShotSlot2()
    {
        skillmanager.SetSkill("Spineshot", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Spineshot", 2);
        SpriteUpdate();
    }

    //Set Undergrowth Skill slot 1 or 2
     public void UndergrowthSlot1()
    {
        skillmanager.SetSkill("Undergrowth", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Undergrowth", 1);
        SpriteUpdate();
    }
     public void UndergrowthSlot2()
    {
        skillmanager.SetSkill("Undergrowth", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Undergrowth", 2);
        SpriteUpdate();
    }

    //Set Unstable Puff Ball Skill slot 1 or 2
     public void UnstablePuffballSlot1()
    {
        skillmanager.SetSkill("UnstablePuffball", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("UnstablePuffball", 1);
        SpriteUpdate();
    }
      public void UnstablePuffballSlot2()
    {
        skillmanager.SetSkill("UnstablePuffball", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("UnstablePuffball", 2);
        SpriteUpdate();
    }

    //Sets blitz skill for slot 1 or 2
     public void BlitzSlot1()
    {
        skillmanager.SetSkill("Blitz", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Blitz", 1);
        SpriteUpdate();
    }
    public void BlitzSlot2()
    {
        skillmanager.SetSkill("Blitz", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Blitz", 2);
        SpriteUpdate();
    }

    //Sets Mycotoxin for slot 1 or 2
     public void MycotoxinsSlot1()
    {
        skillmanager.SetSkill("Mycotoxins", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Mycotoxins", 1);
        SpriteUpdate();
    }
     public void MycotoxinsSlot2()
    {
        skillmanager.SetSkill("Mycotoxins", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Mycotoxins", 2);
        SpriteUpdate();
    }

    //Sets Trophic Cascade for slot 1 or 2
    public void TrophicCascadeSlot1()
    {
        skillmanager.SetSkill("TrophicCascade", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("TrophicCascade", 1);
        SpriteUpdate();
    }
    public void TrophicCascadeSlot2()
    {
        skillmanager.SetSkill("TrophicCascade", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("TrophicCascade", 2);
        SpriteUpdate();
    }

    //Set Defense Mechanism for slot1 or 2
     public void DefenseMechanismSlot1()
    {
        skillmanager.SetSkill("DefenseMechanism", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("DefenseMechanism", 1);
        SpriteUpdate();
    }
         public void DefenseMechanismSlot2()
    {
        skillmanager.SetSkill("DefenseMechanism", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("DefenseMechanism", 2);
        SpriteUpdate();
    }

    //Set leeching spores for slot 1 or 2
    public void LeechingSporeSlot1()
    {
        skillmanager.SetSkill("LeechingSpore", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("LeechingSpore", 1);
        SpriteUpdate();
    }
    public void LeechingSporeSlot2()
    {
        skillmanager.SetSkill("LeechingSpore", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("LeechingSpore", 2);
        SpriteUpdate();
    }

    //Set Spore burst for slot 1 or 2
    public void SporeBurstSlot1()
    {
        skillmanager.SetSkill("Sporeburst", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Sporeburst", 1);
        SpriteUpdate();
    }
     public void SporeBurstSlot2()
    {
        skillmanager.SetSkill("Sporeburst", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Sporeburst", 2);
        SpriteUpdate();
    }

    public void SpriteUpdate()
    {
        List<Sprite> equippedSkillSprites = hudSkills.GetAllSkillSprites();
        Skill1Image.GetComponent<Image>().sprite = equippedSkillSprites[0];
        Skill2Image.GetComponent<Image>().sprite = equippedSkillSprites[1];
        Skill3Image.GetComponent<Image>().sprite = equippedSkillSprites[2];
    }
}
