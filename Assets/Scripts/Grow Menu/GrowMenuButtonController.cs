using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class GrowMenuButtonController : MonoBehaviour
{
    
    [SerializeField]
    private GameObject buttonBase;
    
    public SwapCharacter swapCharacterscript;

    public List<GameObject> buttons;
    public GameObject LevelUI;
    public GameObject SkillMenu;
    ThirdPersonActionsAsset controls;
    public Button button;
    public Button closebutton;
    private HUDSkills hudSkills;
    private CanvasGroup HUDCanvasGroup;
    private NewPlayerHealth playerHealth;
    private SkillManager skillManager;
    public GameObject UIenable;
    private PlayerController playerController;
    private SpawnCharacter spawnCharacterscript;




    void OnEnable()
    {
        ClearList();
        controls = new ThirdPersonActionsAsset();
        controls.UI.MenuSwapL.performed += ctx => MenuSwapLeft();
        controls.UI.MenuSwapR.performed += ctx => MenuSwapRight();
        controls.UI.Close.performed += ctx => Close();
        swapCharacterscript = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        Invoke("ControlEnable", 0.25f);
        SkillMenu.SetActive(false);
        buttons = new List<GameObject>();
        button.Select();
        GenerateList();
        LevelUI.SetActive(false);

    }
   void Start()
   {
        HUDCanvasGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        playerHealth = GameObject.FindWithTag("PlayerParent").GetComponent<NewPlayerHealth>();
        skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
        spawnCharacterscript = GameObject.FindWithTag("PlayerParent").GetComponent<SpawnCharacter>();
    }
   void MenuSwapLeft()
   {
    SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("MainCamera").transform.position);
    SkillMenu.SetActive(true);
   }
   void MenuSwapRight()
   {
    SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("MainCamera").transform.position);
    LevelUI.SetActive(true);
   }
   void ControlEnable()
    {
       controls.UI.Enable();  
    }
    void OnDisable()
    {
      controls.UI.Disable();
    }
    void GenerateList()
    {
        
        foreach (GameObject i in swapCharacterscript.characters)
    {
        GameObject button = Instantiate(buttonBase) as GameObject;
        buttons.Add(button);
        button.SetActive(true);

        string name = i.GetComponent<CharacterStats>().thisName.ToString();
        button.GetComponent<GrowMenuButtonList>().SetText(name);
        button.transform.SetParent(buttonBase.transform.parent, false);
    }
    }
    void ClearList()
    {
         if (buttons.Count > 0)
       {
        foreach (GameObject button in buttons)
        {
            Destroy (button.gameObject);
        }
        buttons.Clear();
       }
    }
    void Close()
    {
        playerController.EnableController();
        UIenable.SetActive(false);
        HUDCanvasGroup.alpha = 1;
        hudSkills.UpdateHUDIcons();
    }
    public void OnClickClose()
    {
        playerController.EnableController();
        UIenable.SetActive(false);
        HUDCanvasGroup.alpha = 1;
        hudSkills.UpdateHUDIcons();
    }
    public void Grow()
    {
        spawnCharacterscript.SpawnNewCharacter();
        playerController.EnableController();
        UIenable.SetActive(false);
        HUDCanvasGroup.alpha = 1;
        hudSkills.UpdateHUDIcons();
    }
}
