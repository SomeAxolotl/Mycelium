using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class GrowMenuButtonController : MonoBehaviour
{
    
    [SerializeField]
    private GameObject buttonBase;
    
    public SwapCharacter swapCharacterscript;

    //private List<GameObject> buttons;
    public GameObject LevelUI;
    public GameObject SkillMenu;
    ThirdPersonActionsAsset controls;
    
    
    
    void OnEnable()
    {
        controls = new ThirdPersonActionsAsset();
        controls.UI.MenuSwapL.performed += ctx => MenuSwapLeft();
        controls.UI.MenuSwapR.performed += ctx => MenuSwapRight();
        swapCharacterscript = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
        Invoke("ControlEnable", 0.25f);
        SkillMenu.SetActive(false);

    }
   void Start()
   {
    
    foreach (GameObject i in swapCharacterscript.characters)
    {
        GameObject button = Instantiate(buttonBase) as GameObject;
        button.SetActive(true);

        string name = i.GetComponent<CharacterStats>().thisName.ToString();
        button.GetComponent<GrowMenuButtonList>().SetText(name);
        button.transform.SetParent(buttonBase.transform.parent, false);
    }
   }
   void MenuSwapLeft()
   {
    SkillMenu.SetActive(true);
   }
   void MenuSwapRight()
   {
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
}
