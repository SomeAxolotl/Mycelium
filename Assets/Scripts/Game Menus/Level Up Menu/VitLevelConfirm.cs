using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class VitLevelConfirm : MonoBehaviour
{
    ThirdPersonActionsAsset controls;
    public CharacterStats currentstats;
    private NutrientTracker nutrientTracker;
    public GameObject ConfirmPanel;
    public GameObject LevelMenu;
    private LevelUpManagerNew levelscript;
    public Button Lock;
    public Button Vitality;
    public TMP_Text confirmtext;
   void OnEnable()
   {
    controls = new ThirdPersonActionsAsset();
    currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
    levelscript = LevelMenu.GetComponent<LevelUpManagerNew>();
    nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
    controls.UISub.Enable();
    controls.UISub.Confirm.performed += ctx => Confirm();
    controls.UISub.CancelConfirm.performed += ctx => Close();
    levelscript.ControlDisable();
    SetText();
    Lock.Select();
   }
   void OnDisable()
   {
    controls.UISub.Confirm.Disable();
    controls.UISub.CancelConfirm.Disable();
    levelscript.ControlEnable();
    Vitality.Select();
   }
   void SetText()
   {
    if(currentstats.vitalityLevel == 4)
    {
        confirmtext.text = "Would you like to use 1x <br> to level?";
    }
    else if(currentstats.vitalityLevel == 9)
    {
        confirmtext.text = "Would you like to use 2x <br> to level?";
    }
    else if(currentstats.vitalityLevel == 14)
    {
        confirmtext.text = "Would you like to use 3x <br> to level?";
    }
   }

    void Confirm()
    {
        if(currentstats.vitalityLevel == 4)
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedLog--;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else if(currentstats.vitalityLevel == 9)
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedLog -= 2;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else if(currentstats.vitalityLevel == 14)
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedLog -= 3;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SpeedDeselect();
        levelscript.SentienceDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
    }
    void Close()
    {
        ConfirmPanel.SetActive(false);
    }
}