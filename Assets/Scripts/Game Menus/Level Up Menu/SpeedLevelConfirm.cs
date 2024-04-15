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

    private void Awake()
    {
        controls = new ThirdPersonActionsAsset();
    }

    void OnEnable()
   {
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
    if(currentstats.speedLevel == 9)
    {
        confirmtext.text = "Would you like to use x1 <br> to level?";
    }
    else if(currentstats.speedLevel == 14)
    {
        confirmtext.text = "Would you like to use x2 <br> to level?";
    }
   }
   void SetMaterial()
   {
        switch(currentstats.equippedSkills[0])
        {
            case "FungalMight":
                Material.sprite = Resources.Load<Sprite>("Rotten Log"); 
                break;
            case "DeathBlossom":
                Material.sprite =Resources.Load<Sprite>("Fresh Exoskeleton"); 
                break;
            case "FairyRing":
                Material.sprite = Resources.Load<Sprite>("Calcite Deposit");
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
        if(currentstats.speedLevel == 9 && currentstats.equippedSkills[0] == "FungalMight")
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
        else if(currentstats.speedLevel == 14 && currentstats.equippedSkills[0] == "FungalMight")
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
        else if(currentstats.speedLevel == 9 && currentstats.equippedSkills[0] == "DeathBlossom")
        {
        currentstats.speedLevel++;
        nutrientTracker.storedExoskeleton--;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedSave = currentstats.speedLevel;
        }
        else if(currentstats.speedLevel == 14 && currentstats.equippedSkills[0] == "DeathBlossom")
        {
        currentstats.speedLevel++;
        nutrientTracker.storedExoskeleton -= 2;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedSave = currentstats.speedLevel;
        }
        else if(currentstats.speedLevel == 9 && currentstats.equippedSkills[0] == "FairyRing")
        {
        currentstats.speedLevel++;
        nutrientTracker.storedCalcite--;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedSave = currentstats.speedLevel;
        }
        else if(currentstats.speedLevel == 14 && currentstats.equippedSkills[0] == "FairyRing")
        {
        currentstats.speedLevel++;
        nutrientTracker.storedCalcite -= 2;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedSave = currentstats.speedLevel;
        }
        else if(currentstats.speedLevel == 9 && currentstats.equippedSkills[0] == "Zombify")
        {
        currentstats.speedLevel++;
        nutrientTracker.storedFlesh--;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.SpeedSave = currentstats.speedLevel;
        }
        else if(currentstats.speedLevel == 14 && currentstats.equippedSkills[0] == "Zombify")
        {
        currentstats.speedLevel++;
        nutrientTracker.storedFlesh -= 2;
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