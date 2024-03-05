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
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftBracket))
      {
        MenuSwapLeft();
      }
      if(Input.GetKeyDown(KeyCode.RightBracket))
      {
        MenuSwapRight();
      }
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
        if (!IsEruptionAssignedToSlot2())
        {
            skillmanager.SetSkill("Eruption", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Eruption", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Eruption is already assigned to Skill2. Cannot assign to Skill1.");
        }
    }
    public void EruptionsSlot2()
    {
        if (!IsEruptionAssignedToSlot1())
        {
            skillmanager.SetSkill("Eruption", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Eruption", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Eruption is already assigned to Skill2. Cannot assign to Skill1.");
        }
    }
    private bool IsEruptionAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "Eruption";
    }
    private bool IsEruptionAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "Eruption";
    }
    
    //Set Living Cyclone Skill slot 1 or 2
    public void LivingCycloneSlot1()
    {
        if (!IsLivingCycloneAssignedToSlot2())
        {
            skillmanager.SetSkill("LivingCyclone", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("LivingCyclone", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("LivingCyclone is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void LivingCycloneSlot2()
    {
        if (!IsLivingCycloneAssignedToSlot1())
        {
            skillmanager.SetSkill("LivingCyclone", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("LivingCyclone", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("LivingCyclone is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsLivingCycloneAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "LivingCyclone";
    }
    private bool IsLivingCycloneAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "LivingCyclone";
    }
    
    
    //Set Relentless Fury Skill slot 1 or 2
    public void RelentlessFurySlot1()
    {
        if (!IsRelentlessFuryAssignedToSlot2())
        {
            skillmanager.SetSkill("RelentlessFury", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("RelentlessFury", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("RelentlessFur is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void RelentlessFurySlot2()
    {
        if (!IsRelentlessFuryAssignedToSlot1())
        {
            skillmanager.SetSkill("RelentlessFury", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("RelentlessFury", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("RelentlessFur is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsRelentlessFuryAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "RelentlessFury";
    }
    private bool IsRelentlessFuryAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "RelentlessFury";
    }

    //Set Spine Shot skill slot 1 or 2  
    public void SpineShotSlot1()
    {
        if (!IsSpineShotAssignedToSlot2())
        {
            skillmanager.SetSkill("Spineshot", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Spineshot", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Spineshot is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void SpineShotSlot2()
    {
        if (!IsSpineShotAssignedToSlot1())
        {
            skillmanager.SetSkill("Spineshot", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Spineshot", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Spineshot is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsSpineShotAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "Spineshot";
    }
    private bool IsSpineShotAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "Spineshot";
    }

    //Set Undergrowth Skill slot 1 or 2
    public void UndergrowthSlot1()
    {
        if (!IsUndergrowthAssignedToSlot2())
        {
            skillmanager.SetSkill("Undergrowth", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Undergrowth", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Undergrowth is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void UndergrowthSlot2()
    {
        if (!IsUndergrowthAssignedToSlot1())
        {
            skillmanager.SetSkill("Undergrowth", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Undergrowth", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Undergrowth is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsUndergrowthAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "Undergrowth";
    }
    private bool IsUndergrowthAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "Undergrowth";
    }

    //Set Unstable Puff Ball Skill slot 1 or 2
    public void UnstablePuffballSlot1()
    {
        if (!IsUnstablePuffballAssignedToSlot2())
        {
            skillmanager.SetSkill("UnstablePuffball", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("UnstablePuffball", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("UnstablePuffball is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void UnstablePuffballSlot2()
    {
        if (!IsUnstablePuffballAssignedToSlot1())
        {
            skillmanager.SetSkill("UnstablePuffball", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("UnstablePuffball", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("UnstablePuffball is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsUnstablePuffballAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "UnstablePuffball";
    }
    private bool IsUnstablePuffballAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "UnstablePuffball";
    }

    //Sets blitz skill for slot 1 or 2
     public void BlitzSlot1()
    {
        if (!IsBlitzAssignedToSlot2())
        {
            skillmanager.SetSkill("Blitz", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Blitz", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Blitz is already assigned to Skill2. Cannot assign to Skill1.");
        }
    }
    public void BlitzSlot2()
    {
        if (!IsBlitzAssignedToSlot1())
        {
            skillmanager.SetSkill("Blitz", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Blitz", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Blitz is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }

    private bool IsBlitzAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "Blitz";
    }

    private bool IsBlitzAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "Blitz";
    }

    //Sets Mycotoxin for slot 1 or 2
    public void MycotoxinsSlot1()
    {
        if (!IsMycotoxinsAssignedToSlot2())
        {
            skillmanager.SetSkill("Mycotoxins", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Mycotoxins", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Mycotoxins is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void MycotoxinsSlot2()
    {
        if (!IsMycotoxinsAssignedToSlot1())
        {
            skillmanager.SetSkill("Mycotoxins", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("Mycotoxins", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Mycotoxins is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsMycotoxinsAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "Mycotoxins";
    }
    private bool IsMycotoxinsAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "Mycotoxins";
    }

    //Sets Trophic Cascade for slot 1 or 2
    public void TrophicCascadeSlot1()
    {
        if (!IsTrophicCascadeAssignedToSlot2())
        {
            skillmanager.SetSkill("TrophicCascade", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("TrophicCascade", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("TrophicCascade is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void TrophicCascadeSlot2()
    {
        if (!IsTrophicCascadeAssignedToSlot1())
        {
            skillmanager.SetSkill("TrophicCascade", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("TrophicCascade", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("TrophicCascade is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsTrophicCascadeAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "TrophicCascade";
    }
    private bool IsTrophicCascadeAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "TrophicCascade";
    }

    //Set Defense Mechanism for slot1 or 2
    public void DefenseMechanismSlot1()
    {
        if (!IsDefenseMechanismAssignedToSlot2())
        {
        skillmanager.SetSkill("DefenseMechanism", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("DefenseMechanism", 1);
        SpriteUpdate();
        }
        else
        {
            Debug.Log("DefenseMechanism is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void DefenseMechanismSlot2()
    {
        if (!IsDefenseMechanismAssignedToSlot1())
        {
        skillmanager.SetSkill("DefenseMechanism", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("DefenseMechanism", 2);
        SpriteUpdate();
        }
        else
        {
            Debug.Log("DefenseMechanism is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsDefenseMechanismAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "DefenseMechanism";
    }
    private bool IsDefenseMechanismAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "DefenseMechanism";
    }

    //Set leeching spores for slot 1 or 2
    public void LeechingSporeSlot1()
    {
        if (!IsLeechingSporeAssignedToSlot2())
        {
            skillmanager.SetSkill("LeechingSpore", 1, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("LeechingSpore", 1);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Blitz is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void LeechingSporeSlot2()
    {
        if (!IsLeechingSporeAssignedToSlot1())
        {
            skillmanager.SetSkill("LeechingSpore", 2, GameObject.FindWithTag("currentPlayer"));
            hudSkills.ChangeSkillIcon("LeechingSpore", 2);
            SpriteUpdate();
        }
        else
        {
            Debug.Log("Blitz is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsLeechingSporeAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "LeechingSpore";
    }
    private bool IsLeechingSporeAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "LeechingSpore";
    }

    //Set Spore burst for slot 1 or 2
    public void SporeBurstSlot1()
    {
        if (!IsSporeBurstAssignedToSlot2())
        {
        skillmanager.SetSkill("Sporeburst", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Sporeburst", 1);
        SpriteUpdate();
        }
        else
        {
            Debug.Log("SporeBurst is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    public void SporeBurstSlot2()
    {
        if (!IsSporeBurstAssignedToSlot1())
        {
        skillmanager.SetSkill("Sporeburst", 2, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Sporeburst", 2);
        SpriteUpdate();
        }
        else
        {
            Debug.Log("SporeBurst is already assigned to Skill1. Cannot assign to Skill2.");
        }
    }
    private bool IsSporeBurstAssignedToSlot1()
    {
        return hudSkills.GetSkillNameInSlot(1) == "Sporeburst";
    }
    private bool IsSporeBurstAssignedToSlot2()
    {
        return hudSkills.GetSkillNameInSlot(2) == "Sporeburst";
    }

    public void SpriteUpdate()
    {
        List<Sprite> equippedSkillSprites = hudSkills.GetAllSkillSprites();
        Skill1Image.GetComponent<Image>().sprite = equippedSkillSprites[0];
        Skill2Image.GetComponent<Image>().sprite = equippedSkillSprites[1];
        Skill3Image.GetComponent<Image>().sprite = equippedSkillSprites[2];
    }
}
