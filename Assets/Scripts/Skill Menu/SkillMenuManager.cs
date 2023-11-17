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
    
    void Start()
    {
        
    }
    void Awake()
    {
      controls = new ThirdPersonActionsAsset();
      controls.UI.MenuSwapL.performed += ctx => MenuSwapLeft();
      controls.UI.Close.performed += ctx => CloseSkill();
    }
    void OnEnable()
    {
        LevelUI.SetActive(false);
        Skill1.Select();
        controls.UI.Enable(); 
        Skill1ListEnable.SetActive(false);  
        Skill2ListEnable.SetActive(false);
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        SpriteUpdate();
        
    }
    void OnDisable()
    {
      controls.UI.Disable();
    }
    void MenuSwapLeft()
    {
        LevelUI.SetActive(true);
    }

   
    void Update()
    {
        
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
            Skill1.Select();
        }
        else if (Skill2ListEnable.activeInHierarchy == true)
        {
            Skill2ListEnable.SetActive(false);
            Skill2.Select();
        }
        else
        {
            UIenable.SetActive(false);
            HUDCanvasGroup.alpha = 1;
        }
    }

    public void slot1Changeskill1()
    {
        skillmanager.SetSkill("Eruption", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Eruption", 0);
        SpriteUpdate();
    }
    public void slot1Changeskill2()
    {
        skillmanager.SetSkill("Living Cyclone", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Living Cyclone", 0);
        SpriteUpdate();
    }
    public void slot1Changeskill3()
    {
        skillmanager.SetSkill("Relentless Fury", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Relentless Fury", 0);
        SpriteUpdate();
    }
    public void slot1Changeskill4()
    {
        skillmanager.SetSkill("Spineshot", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Spineshot", 0);
        SpriteUpdate();
    }
     public void slot1Changeskill5()
    {
        skillmanager.SetSkill("Undergrowth", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Undergrowth", 0);
        SpriteUpdate();
    }
     public void slot1Changeskill6()
    {
        skillmanager.SetSkill("UnstablePuffball", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("UnstablePuffball", 0);
        SpriteUpdate();
    }
     public void slot1Changeskill7()
    {
        skillmanager.SetSkill("Deathblossom", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Deathblossom", 0);
        SpriteUpdate();
    }
    public void slot1Changeskill8()
    {
        skillmanager.SetSkill("FairyRing", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("FairyRing", 0);
        SpriteUpdate();
    }
     public void slot1Changeskill9()
    {
        skillmanager.SetSkill("FungalMight", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("FungalMight", 0);
        SpriteUpdate();
    }
     public void slot1Changeskill10()
    {
        skillmanager.SetSkill("Zombify", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Zombify", 0);
        SpriteUpdate();
    }
     public void slot1Changeskill11()
    {
        skillmanager.SetSkill("Blitz", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Blitz", 0);
        SpriteUpdate();
    }
     public void slot1Changeskill12()
    {
        skillmanager.SetSkill("Mycotoxins", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Mycotoxins", 0);
        SpriteUpdate();
    }
    public void slot1Changeskill13()
    {
        skillmanager.SetSkill("TrophicCascade", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("TrophicCascade", 0);
        SpriteUpdate();
    }
     public void slot1Changeskill14()
    {
        skillmanager.SetSkill("DefenseMechanism", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("DefenseMechanism", 0);
        SpriteUpdate();
    }
    public void slot1Changeskill15()
    {
        skillmanager.SetSkill("LeechingSpore", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("LeechingSpore", 0);
        SpriteUpdate();
    }
    public void slot1Changeskill16()
    {
        skillmanager.SetSkill("Sporeburst", 0, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Sporeburst", 0);
        SpriteUpdate();
    }


    public void slot2Changeskill1()
    {
        skillmanager.SetSkill("Eruption", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Eruption", 1);
        SpriteUpdate();
    }
    public void slot2Changeskill2()
    {
        skillmanager.SetSkill("Living Cyclone", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Living Cyclone", 1);
        SpriteUpdate();
    }
    public void slot2Changeskill3()
    {
        skillmanager.SetSkill("Relentless Fury", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Relentless Fury", 1);
        SpriteUpdate();
    }
    public void slot2Changeskill4()
    {
        skillmanager.SetSkill("Spineshot", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Spineshot", 1);
        SpriteUpdate();
    }
     public void slot2Changeskill5()
    {
        skillmanager.SetSkill("Undergrowth", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Undergrowth", 1);
        SpriteUpdate();
    }
     public void slot2Changeskill6()
    {
        skillmanager.SetSkill("UnstablePuffball", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("UnstablePuffball", 1);
        SpriteUpdate();
    }
     public void slot2Changeskill7()
    {
        skillmanager.SetSkill("Deathblossom", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Deathblossom", 1);
        SpriteUpdate();
    }
    public void slot2Changeskill8()
    {
        skillmanager.SetSkill("FairyRing", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("FairyRing", 1);
        SpriteUpdate();
    }
     public void slot2Changeskill9()
    {
        skillmanager.SetSkill("FungalMight", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("FungalMight", 1);
        SpriteUpdate();
    }
     public void slot2Changeskill10()
    {
        skillmanager.SetSkill("Zombify", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Zombify", 1);
        SpriteUpdate();
    }
     public void slot2Changeskill11()
    {
        skillmanager.SetSkill("Blitz", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Blitz", 1);
        SpriteUpdate();
    }
     public void slot2Changeskill12()
    {
        skillmanager.SetSkill("Mycotoxins", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Mycotoxins", 1);
        SpriteUpdate();
    }
    public void slot2Changeskill13()
    {
        skillmanager.SetSkill("TrophicCascade", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("TrophicCascade", 1);
        SpriteUpdate();
    }
     public void slot2Changeskill14()
    {
        skillmanager.SetSkill("DefenseMechanism", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("DefenseMechanism", 1);
        SpriteUpdate();
    }
    public void slot2Changeskill15()
    {
        skillmanager.SetSkill("LeechingSpore", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("LeechingSpore", 1);
        SpriteUpdate();
    }
    public void slot2Changeskill16()
    {
        skillmanager.SetSkill("Sporeburst", 1, GameObject.FindWithTag("currentPlayer"));
        hudSkills.ChangeSkillIcon("Sporeburst", 1);
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
