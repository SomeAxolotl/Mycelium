using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RonaldSunglassesEmoji.Interaction;

public class SporeInteraction : MonoBehaviour, IInteractable
{
    CharacterStats characterStats;
    DesignTracker designTracker;
    string coloredSporeName = "Gob";

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        designTracker = GetComponent<DesignTracker>();
    }

    public void Interact(GameObject interactObject)
    {
        if (!interactObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sprout"))
        {
            SwapCharacter swapCharacter = GameObject.Find("PlayerParent").GetComponent<SwapCharacter>();

            int characterIndex = swapCharacter.GetCharacterIndex(interactObject);
            swapCharacter.SwitchCharacterGrowMenu(characterIndex);

            DestroyTooltip(gameObject);
        }
    }

    public void Salvage(GameObject interactObject)
    {
        //buh
    }

    public void CreateTooltip(GameObject interactObject)
    {
        Color bodyColor = designTracker.bodyColor;
        //coloredSporeName = "<color=#" + ColorUtility.ToHtmlStringRGB(bodyColor) + ">"+characterStats.sporeName+"</color>";
        coloredSporeName = characterStats.GetColoredSporeName();

        string buttonText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        int subspeciesSkillIconIndex = 8;
        switch (characterStats.equippedSkills[0])
        {
            case "FungalMight":
                subspeciesSkillIconIndex = 8;
                break;
            case "DeathBlossom":
                subspeciesSkillIconIndex = 9;
                break;
            case "FairyRing":
                subspeciesSkillIconIndex = 10;
                break;
            case "Zombify":
                subspeciesSkillIconIndex = 11;
                break;
        }

        TraitBase trait = interactObject.GetComponent<TraitBase>();

        string tooltipTitle = "";
        tooltipTitle += $"<sprite={subspeciesSkillIconIndex}>   ";
        if (trait != null)
        {
            tooltipTitle += $"{trait.traitName} ";
        }
        tooltipTitle += $"{coloredSporeName}   <sprite={subspeciesSkillIconIndex}>";

        string tooltipDesc = "";
        tooltipDesc += "<sprite=0> " + characterStats.primalLevel;
        tooltipDesc += "   <sprite=1> " + characterStats.speedLevel;
        tooltipDesc += "   <sprite=2> " + characterStats.sentienceLevel;
        tooltipDesc += "   <sprite=3> " + characterStats.vitalityLevel;
        tooltipDesc += "\n";
        if(trait != null){
            tooltipDesc += trait.traitDesc;
        }

        int highestLoopBeaten = characterStats.O_highestLoopBeaten;
        Tooltip sporeTooltip = TooltipManager.Instance.CreateTooltip
        (
            gameObject,
            tooltipTitle,
            tooltipDesc,
            "Press " + buttonText + " to Swap",
            interactString2: GetRecordString(highestLoopBeaten),
            shouldParent: true,
            verticalOffset: GetVerticalOffsetWithSecondInteractString(highestLoopBeaten),
            true
        );
        sporeTooltip.tooltipInteract2.color = Color.Lerp
            (sporeTooltip.tooltipInteract2.color, 
            TooltipManager.Instance.maxLoopColor, 
            Mathf.Clamp(highestLoopBeaten, 0, 10) / 10f);

        sporeTooltip.ShowHappiness(characterStats.sporeHappiness);
    }

    float GetVerticalOffsetWithSecondInteractString(int highestLoop)
    {
        float verticalOffset = 1f;

        if (DoesSporeDisplayRecord(highestLoop))
        {
            verticalOffset += 0.25f;
        }

        return verticalOffset;
    }

    string GetRecordString(int highestLoop)
    {
        if (DoesSporeDisplayRecord(highestLoop))
        {
            return "Tier Record: " + highestLoop;
        }
        else
        {
            return "";
        }

        //loopRecordDisplay.color = Color.Lerp(loopRecordDisplay.color, TooltipManager.Instance.legendaryBackgroundColor, Mathf.Clamp(highestLoop, 0, 10) / 10f);
    }

    bool DoesSporeDisplayRecord(int highestLoop)
    {
        bool doesSporeDisplayRecord = highestLoop > 0 ? true : false;
        return doesSporeDisplayRecord;
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        TooltipManager.Instance.DestroyTooltip();
    }
}
