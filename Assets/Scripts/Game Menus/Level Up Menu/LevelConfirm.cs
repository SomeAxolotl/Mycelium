using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

public class LevelConfirm : MonoBehaviour
{
    ThirdPersonActionsAsset controls;
    public CharacterStats currentstats;
    private NutrientTracker nutrientTracker;
    public GameObject ConfirmPanel;
    public GameObject LevelMenu;
    private LevelUpManagerNew levelscript;
    public Button Lock;
    public Button Primal;
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
    Primal.Select();
   }
   void SetText()
   {
    if(currentstats.primalLevel == 4)
    {
        confirmtext.text = "Would you like to use x1 <br> to level?";
    }
    else if(currentstats.primalLevel == 9)
    {
        confirmtext.text = "Would you like to use x2 <br> to level?";
    }
    else if(currentstats.primalLevel == 14)
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
        if(currentstats.primalLevel == 4)
        {
        currentstats.primalLevel++;
        nutrientTracker.storedLog--;
        Debug.Log("Leveled Primal");
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.SpeedDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.PrimalSave = currentstats.primalLevel;
        }
        else if(currentstats.primalLevel == 9)
        {
        currentstats.primalLevel++;
        nutrientTracker.storedLog -= 2;
        Debug.Log("Leveled Primal");
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.SpeedDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.PrimalSave = currentstats.primalLevel;
        }
        else if(currentstats.primalLevel == 14)
        {
        currentstats.primalLevel++;
        nutrientTracker.storedLog -= 3;
        Debug.Log("Leveled Primal");
        currentstats.StartCalculateAttributes();
        currentstats.UpdateLevel();
        levelscript.UIUpdate();
        ConfirmPanel.SetActive(false);
        levelscript.SpeedDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
        levelscript.PrimalSave = currentstats.primalLevel;
        }
    }
    void Close()
    {
        ConfirmPanel.SetActive(false);
        levelscript.SpeedDeselect();
        levelscript.VitalityDeselect();
        levelscript.SentienceDeselect();
    }
}