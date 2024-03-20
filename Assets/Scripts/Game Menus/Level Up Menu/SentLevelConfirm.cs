using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class SentLevelConfirm : MonoBehaviour
{
    ThirdPersonActionsAsset controls;
    public CharacterStats currentstats;
    private NutrientTracker nutrientTracker;
    public GameObject ConfirmPanel;
    public GameObject LevelMenu;
    private LevelUpManagerNew levelscript;
    public Button Lock;
    public Button Sentience;
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
    Sentience.Select();
   }
   void SetText()
   {
    if(currentstats.sentienceLevel == 4)
    {
        confirmtext.text = "Would you like to use x1 <br> to level?";
    }
    else if(currentstats.sentienceLevel == 9)
    {
        confirmtext.text = "Would you like to use x2 <br> to level?";
    }
    else if(currentstats.sentienceLevel == 14)
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
        if(currentstats.sentienceLevel == 4)
        {
        currentstats.sentienceLevel++;
        nutrientTracker.storedLog--;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SpeedDeselect();
        levelscript.SentienceSave = currentstats.sentienceLevel;
        }
        else if(currentstats.sentienceLevel == 9)
        {
        currentstats.sentienceLevel++;
        nutrientTracker.storedLog -= 2;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SpeedDeselect();
        levelscript.SentienceSave = currentstats.sentienceLevel;
        }
        else if(currentstats.sentienceLevel == 14)
        {
        currentstats.sentienceLevel++;
        nutrientTracker.storedLog -= 3;
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SpeedDeselect();
        levelscript.SentienceSave = currentstats.sentienceLevel;
        }
    }
    void Close()
    {
        ConfirmPanel.SetActive(false);
        levelscript.PrimalDeselect();
        levelscript.VitalityDeselect();
        levelscript.SpeedDeselect();
    }
}