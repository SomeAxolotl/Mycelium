using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class GrowMenuButtonList : MonoBehaviour
{
    
    [SerializeField]
    private TextMeshProUGUI mytext;
    [SerializeField]
    private GrowMenuButtonController buttoncontroller;

    private string TextString;
    public SwapCharacter swapCharacterscript;

    void OnEnable()
    {
        swapCharacterscript = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
    }
    public void SetText(string textString)
    {
        TextString = textString;
        mytext.text = textString;
    }

    public void OnClick()
    {
     swapCharacterscript.SwitchCharacter(swapCharacterscript.currentCharacterIndex);
    }



}
