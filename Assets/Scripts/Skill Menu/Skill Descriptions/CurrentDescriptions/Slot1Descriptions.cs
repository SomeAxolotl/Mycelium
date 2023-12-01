using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Slot1Descriptions : MonoBehaviour, ISelectHandler
{
    public TMP_Text SkillDesc;
    public GameObject SkillDescriptionPanel;
    public CharacterStats currentstats;
    public SkillManager skillManager;
        void OnEnable()
    {
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        skillManager = GameObject.FindWithTag("PlayerParent").GetComponent<SkillManager>();
    }
    public void OnSelect(BaseEventData eventData)
    {
           
        SkillDescriptionPanel.SetActive(true);
        Descriptions();
        
        
    }
    void Descriptions()
    {
        switch(currentstats.equippedSkills[1])
        { 
            case "Eruption":
                SkillDesc.text = "Eruption: <br> <size=25>Stomp the ground with primal strength<br> dealing damage to all enemies around you. <br>Deals more damage to enemies closer to you.";
                break;
            case "LivingCyclone":
                SkillDesc.text ="Living Cyclone: <br> <size=25>Spin relentlessly striking all enemies<br> around you with your currently equipped weapon. <br> You are able to move while Living Cyclone is active.";
                break;
            case "RelentlessFury":
                SkillDesc.text = "Relentless Fury: <br> <size=25>Go into a frenzy gaining 30% attack speed. While active lose 5% of current health every second. Attacks restore health equal to 25%<br> of weapon damage.";
                break;
            case "Blitz":
                SkillDesc.text = "Blitz: <br> <size=25>Dash in a straight line damaging all enemies hit.<br> If an enemy is hit the cooldown of Blitz<br> is reduced by 50%.";
                break;
            case "TrophicCascade":
                SkillDesc.text = "Trophic Cascade: <br> <size=25>Release a flurry of attacks slashing<br> all enemies around you.";
                break;
            case "Mycotoxins":
                SkillDesc.text = "Mycotoxins: <br> <size=25>Gain 50% bonus movement speed and release a trail of spores behind you. Enemies hit by these spores are damaged.";
                break;
            case "Spineshot":
                SkillDesc.text = "Spineshot: <br><br> <size=25>Fire out a spine damaging the first enemy hit.";
                break;
            case "Unstablepuffball":
                SkillDesc.text = "Unstable Puffball: <br><br><size=25>Fires a puffball that explodes and damages all enemies upon contact.";
                break;
            case "Undergrowth":
                SkillDesc.text = "Undergrowth: <br><br><size=25> An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit.";
                break;
            case "LeechingSpore":
                SkillDesc.text = "Leeching Spores: <br><br> <size=25>Infest a nearby enemy with a leeching spore. The spore steals health every second from the enemy and restores it to you.";
                break;
            case "Sporeburst":
                SkillDesc.text = "Sporeburst: <br><br> <size=25>Spores explode from you stunning and damaging all enemies caught in its radius. Heal for 50% of all damage dealt.";
                break;
            case "DefenseMechanism":
                SkillDesc.text = "Defense Mechanism: <br><size=25>Reduces damage taken by 50% for 1 second. Attacks against you while Defense Mechanism is active is stored as bonus damage on your next attack equal to 50% of the damage absorbed.";
                break;
            default:
                SkillDesc.text = "No Skill Currently Equipped.";
                break;
        }
         
    }
}
