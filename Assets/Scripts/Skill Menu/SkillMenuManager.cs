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

}
