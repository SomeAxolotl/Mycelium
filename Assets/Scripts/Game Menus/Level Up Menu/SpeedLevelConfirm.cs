using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class SpeedLevelConfirm : MonoBehaviour
{
    ThirdPersonActionsAsset controls;
    public CharacterStats currentstats;
    private NutrientTracker nutrientTracker;
    public GameObject ConfirmPanel;
    public GameObject LevelMenu;
    private LevelUpManagerNew levelscript;
    public Button Lock;
    public Button Speed;
    public TMP_Text confirmtext;
    public Image Material;
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
    SetMaterial();
    Lock.Select();
   }
   void OnDisable()
   {
    controls.UISub.Confirm.Disable();
    controls.UISub.CancelConfirm.Disable();
    levelscript.ControlEnable();
    Speed.Select();
   }
   void SetText()
   {
    if(currentstats.speedLevel == 4)
    {
        confirmtext.text = "Would you like to use x1 <br> to level?";
    }
    else if(currentstats.speedLevel == 9)
    {
        confirmtext.text = "Would you like to use x2 <br> to level?";
    }
    else if(currentstats.speedLevel == 14)
    {
        confirmtext.text = "Would you like to use x3 <br> to level?";
    }
   }
   void SetMaterial()
   {
     switch(currentstats.equippedSkills[0])
    {
        case "FungalMight":
        Material.sprite = Resources.Load<Sprite>("RottenLog"); 
        break;
        case "DeathBlossom":
        Material.sprite =Resources.Load<Sprite>("FreshExoskeleton"); 
        break;
        default:
        return;
        break;
    }
   }

    void Confirm()
    {
        if(currentstats.speedLevel == 4)
        {
        currentstats.speedLevel++;
        nutrientTracker.storedLog--;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedSave = currentstats.speedLevel;
        }
        else if(currentstats.speedLevel == 9)
        {
        currentstats.speedLevel++;
        nutrientTracker.storedLog -= 2;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedSave = currentstats.speedLevel;
        }
        else if(currentstats.speedLevel == 14)
        {
        currentstats.speedLevel++;
        nutrientTracker.storedLog -= 3;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedSave = currentstats.speedLevel;
        }
    }
    void Close()
    {
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
    }
}