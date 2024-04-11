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
        coloredSporeName = "<color=#" + ColorUtility.ToHtmlStringRGB(bodyColor) + ">"+characterStats.sporeName+"</color>";

        string buttonText = "<color=#3cdb4e>A</color>";
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
        
        Tooltip sporeTooltip = TooltipManager.Instance.CreateTooltip
        (
            gameObject, 
            "<sprite=" + subspeciesSkillIconIndex + ">  " + coloredSporeName + "  <sprite=" + subspeciesSkillIconIndex + ">", 
            "<sprite=0> " + characterStats.primalLevel + 
            "   <sprite=1> " + characterStats.speedLevel + 
            "   <sprite=2> " + characterStats.sentienceLevel + 
            "   <sprite=3> " + characterStats.vitalityLevel, 
            "Press " + buttonText + " to Swap",
            "",
            true,
            1f,
            false
        );
        if (sporeTooltip != null)
        {  
            sporeTooltip.ShowHappiness(characterStats.sporeHappiness);
        }
    }

    public void DestroyTooltip(GameObject interactObject)
    {
        TooltipManager.Instance.DestroyTooltip();
    }
}
