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
        SwapCharacter swapCharacter = GameObject.Find("PlayerParent").GetComponent<SwapCharacter>();

        int characterIndex = swapCharacter.GetCharacterIndex(interactObject);
        swapCharacter.SwitchCharacterGrowMenu(characterIndex);

        DestroyTooltip(gameObject);
    }

    public void Salvage(GameObject interactObject)
    {
        //buh
    }

    public void CreateTooltip(GameObject interactObject)
    {
        Color capColor = designTracker.capColor;
        //coloredSporeName = "<color=#" + ColorUtility.ToHtmlStringRGB(capColor) + ">"+characterStats.sporeName+"</color>";

        string buttonText = "<color=#3cdb4e>A</color>";
        int subspeciesSkillIconIndex = 4;
        switch (characterStats.equippedSkills[0])
        {
            case "FungalMight":
                subspeciesSkillIconIndex = 4;
                break;
            case "DeathBlossom":
                subspeciesSkillIconIndex = 5;
                break;
            case "FairyRing":
                subspeciesSkillIconIndex = 6;
                break;
            case "Zombify":
                subspeciesSkillIconIndex = 7;
                break;
        }
        TooltipManager.Instance.CreateTooltip
        (
            gameObject, 
            "<sprite=" + subspeciesSkillIconIndex + ">  " + characterStats.sporeName + "  <sprite=" + subspeciesSkillIconIndex + ">", 
            "<sprite=0> " + characterStats.primalLevel + 
            "   <sprite=1> " + characterStats.speedLevel + 
            "   <sprite=2> " + characterStats.sentienceLevel + 
            "   <sprite=3> " + characterStats.vitalityLevel, 
            "Press " + buttonText + " to Swap",
            "",
            true,
            1f
        );
    }

    public void DestroyTooltip(GameObject interactObject)
    {
        TooltipManager.Instance.DestroyTooltip();
    }
}
