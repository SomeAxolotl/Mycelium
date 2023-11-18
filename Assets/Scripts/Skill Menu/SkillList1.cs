using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class SkillList1 : MonoBehaviour
{
   ThirdPersonActionsAsset controls;
   public GameObject SkillList;
    void Start()
    {
        
    }
    void OnEnable()
    {
      controls = new ThirdPersonActionsAsset();
      controls.UI.Close.performed += ctx => CloseMenu();
    }
    void CloseMenu()
    {
        SkillList.SetActive(false);
    }

    
    void Update()
    {
        
    }
}
