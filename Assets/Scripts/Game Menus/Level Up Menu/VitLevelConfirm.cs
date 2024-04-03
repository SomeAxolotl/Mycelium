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
    public Image Material;
   void OnEnable()
   {
    controls = InputManager.actionAsset;
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
    Vitality.Select();
   }
   void SetText()
   {
    if(currentstats.vitalityLevel == 4)
    {
        confirmtext.text = "Would you like to use x1 <br> to level?";
    }
    else if(currentstats.vitalityLevel == 9)
    {
        confirmtext.text = "Would you like to use x2 <br> to level?";
    }
    else if(currentstats.vitalityLevel == 14)
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
            case "FairyRing":
                Material.sprite = Resources.Load<Sprite>("CalciteDeposit");
                break;
            case "Zombify":
                Material.sprite = Resources.Load<Sprite>("Flesh");
                break;
            default:
                return;
        }
   }

    void Confirm()
    {
        if(currentstats.vitalityLevel == 4 && currentstats.equippedSkills[0] == "FungalMight")
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
        else if(currentstats.vitalityLevel == 9 && currentstats.equippedSkills[0] == "FungalMight")
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
        else if(currentstats.vitalityLevel == 14 && currentstats.equippedSkills[0] == "FungalMight")
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
        else  if(currentstats.vitalityLevel == 4 && currentstats.equippedSkills[0] == "DeathBlossom")
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedExoskeleton--;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else if(currentstats.vitalityLevel == 9 && currentstats.equippedSkills[0] == "DeathBlossom")
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedExoskeleton -= 2;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else if(currentstats.vitalityLevel == 14 && currentstats.equippedSkills[0] == "DeathBlossom")
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedExoskeleton -= 3;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SpeedDeselect();
        levelscript.SentienceDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else  if(currentstats.vitalityLevel == 4 && currentstats.equippedSkills[0] == "FairyRing")
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedCalcite--;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else if(currentstats.vitalityLevel == 9 && currentstats.equippedSkills[0] == "FairyRing")
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedCalcite -= 2;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else if(currentstats.vitalityLevel == 14 && currentstats.equippedSkills[0] == "FairyRing")
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedCalcite -= 3;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SpeedDeselect();
        levelscript.SentienceDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else  if(currentstats.vitalityLevel == 4 && currentstats.equippedSkills[0] == "Zombify")
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedFlesh--;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else if(currentstats.vitalityLevel == 9 && currentstats.equippedSkills[0] == "Zombify")
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedFlesh -= 2;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedDeselect();
        levelscript.VitalitySave = currentstats.vitalityLevel;
        }
        else if(currentstats.vitalityLevel == 14 && currentstats.equippedSkills[0] == "Zombify")
        {
        currentstats.vitalityLevel++;
        nutrientTracker.storedFlesh -= 3;
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
        levelscript.PrimalDeselect();
        levelscript.SpeedDeselect();
        levelscript.SentienceDeselect();
    }
}