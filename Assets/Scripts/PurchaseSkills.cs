using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
public class PurchaseSkills : MonoBehaviour
{
    public CharacterStats currentstats;
    public NutrientTracker nutrientTracker;

    public int eruptionNutrientCost = 2500;
    public int eruptionMaterialCost = 1;

    public int cycloneNutrientCost = 5000;
    public int cycloneMaterialCost = 2;

    public int furyNutrientCost = 10000;
    public int furyMaterialCost = 3;

    public int blitzNutrientCost = 2500;
    public int blitzMaterialCost = 1;

    public int cascadeNutrientCost = 5000;
    public int cascadeMaterialCost = 2;

    public int mycotoxinsNutrientCost = 10000;
    public int mycotoxinsMaterialCost = 3;

    public int spineshotNutrientCost = 2500;
    public int spineshotMaterialCost = 1;

    public int puffballNutrientCost = 5000;
    public int puffballMaterialCost = 2;

    public int undergrowthNutrientCost = 10000;
    public int undergrowthMaterialCost = 3;

    public int leechingSporeNutrientCost = 2500;
    public int leechingSporeMaterialCost = 1;

    public int sporeburstNutrientCost = 5000;
    public int sporeburstMaterialCost = 2;

    public int defenseMechanismNutrientCost = 10000;
    public int defenseMechanismMaterialCost = 3;
    public TMP_Text SkillDesc;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        currentstats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        nutrientTracker = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void PurchaseEruption()
    {
        if (currentstats.primalLevel >= 5 && currentstats.skillEquippables["Eruption"] == false && nutrientTracker.currentNutrients >= eruptionNutrientCost && nutrientTracker.storedLog >= eruptionMaterialCost)
        {
            nutrientTracker.SubtractNutrients(eruptionNutrientCost);
            nutrientTracker.SpendLog(eruptionMaterialCost);
            currentstats.skillEquippables["Eruption"] = true;
            SkillDesc.text = "Eruption: <br> <size=25>Stomp the ground with primal strength <br> dealing damage to all enemies around you.<br> Deals additional damage to enemies closer to you.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseCyclone()
    {
        if (currentstats.primalLevel >= 10 && currentstats.skillEquippables["LivingCyclone"] == false && nutrientTracker.currentNutrients >= cycloneNutrientCost && nutrientTracker.storedLog >= cycloneMaterialCost)
        {
            nutrientTracker.SubtractNutrients(cycloneNutrientCost);
            nutrientTracker.SpendLog(cycloneMaterialCost);
            currentstats.skillEquippables["LivingCyclone"] = true;
            SkillDesc.text = "Living Cyclone: <br> <size=25>Spin relentlessly striking all enemies<br> around you with your currently equipped weapon. <br> You are able to move while Living Cyclone is active.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseFury()
    {
        if (currentstats.primalLevel >= 15 && currentstats.skillEquippables["RelentlessFury"] == false && nutrientTracker.currentNutrients >= furyNutrientCost && nutrientTracker.storedLog >= furyMaterialCost)
        {
            nutrientTracker.SubtractNutrients(furyNutrientCost);
            nutrientTracker.SpendLog(furyMaterialCost);
            currentstats.skillEquippables["RelentlessFury"] = true;
            SkillDesc.text = "Relentless Fury: <br> <size=25>Go into a frenzy gaining 30% attack speed. While active lose 5% of current health every second. Attacks restore health equal to 25%<br> of weapon damage.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseBlitz()
    {
        if (currentstats.speedLevel >= 5 && currentstats.skillEquippables["Blitz"] == false && nutrientTracker.currentNutrients >= blitzNutrientCost && nutrientTracker.storedExoskeleton >= blitzMaterialCost)
        {
            nutrientTracker.SubtractNutrients(blitzNutrientCost);
            nutrientTracker.SpendExoskeleton(blitzMaterialCost);
            currentstats.skillEquippables["Blitz"] = true;
            SkillDesc.text = "Blitz: <br> <size=25>Dash in a straight line damaging all enemies hit.<br> If an enemy is hit the cooldown of Blitz<br> is reduced by 50%.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseCascade()
    {
        if (currentstats.speedLevel >= 10 && currentstats.skillEquippables["TrophicCascade"] == false && nutrientTracker.currentNutrients >= cascadeNutrientCost && nutrientTracker.storedExoskeleton >= cascadeMaterialCost)
        {
            nutrientTracker.SubtractNutrients(cascadeNutrientCost);
            nutrientTracker.SpendExoskeleton(cascadeMaterialCost);
            currentstats.skillEquippables["TrophicCascade"] = true;
            SkillDesc.text = "Trophic Cascade: <br> <size=25>Release a flurry of attacks slashing<br> all enemies around you.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseMycotoxins()
    {
        if (currentstats.speedLevel >= 15 && currentstats.skillEquippables["Mycotoxins"] == false && nutrientTracker.currentNutrients >= mycotoxinsNutrientCost && nutrientTracker.storedExoskeleton >= mycotoxinsMaterialCost)
        {
            nutrientTracker.SubtractNutrients(mycotoxinsNutrientCost);
            nutrientTracker.SpendExoskeleton(mycotoxinsMaterialCost);
            currentstats.skillEquippables["Mycotoxins"] = true;
            SkillDesc.text = "Mycotoxins: <br> <size=25>Gain 50% bonus movement speed and release a trail of spores behind you. Enemies hit by these spores are damaged.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseSpineshot()
    {
        if (currentstats.sentienceLevel >= 5 && currentstats.skillEquippables["Spineshot"] == false && nutrientTracker.currentNutrients >= spineshotNutrientCost && nutrientTracker.storedCalcite >= spineshotMaterialCost)
        {
            nutrientTracker.SubtractNutrients(spineshotNutrientCost);
            nutrientTracker.SpendCalcite(spineshotMaterialCost);
            currentstats.skillEquippables["Spineshot"] = true;
            SkillDesc.text = "Spineshot: <br><br> <size=25>Fire out a spine damaging the first enemy hit.";
        }
        else
        {
            return;
        }
    }
    public void PurchasePuffball()
    {
        if (currentstats.sentienceLevel >= 10 && currentstats.skillEquippables["UnstablePuffball"] == false && nutrientTracker.currentNutrients >= puffballNutrientCost && nutrientTracker.storedCalcite >= puffballMaterialCost)
        {
            nutrientTracker.SubtractNutrients(puffballNutrientCost);
            nutrientTracker.SpendCalcite(puffballMaterialCost);
            currentstats.skillEquippables["UnstablePuffball"] = true;
            SkillDesc.text = "Unstable Puffball: <br><br><size=25>Fires a puffball that explodes and damages all enemies upon contact.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseUndergrowth()
    {
        if (currentstats.sentienceLevel >= 15 && currentstats.skillEquippables["Undergrowth"] == false && nutrientTracker.currentNutrients >= undergrowthNutrientCost && nutrientTracker.storedCalcite >= undergrowthMaterialCost)
        {
            nutrientTracker.SubtractNutrients(undergrowthNutrientCost);
            nutrientTracker.SpendCalcite(undergrowthMaterialCost);
            currentstats.skillEquippables["Undergrowth"] = true;
            SkillDesc.text = "Undergrowth: <br><br><size=25> An entangling line of mycelium grows in a line in front of you damaging and rooting any enemies hit.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseLeechingSpore()
    {
        if (currentstats.vitalityLevel >= 5 && currentstats.skillEquippables["LeechingSpore"] == false && nutrientTracker.currentNutrients >= leechingSporeNutrientCost && nutrientTracker.storedFlesh >= leechingSporeMaterialCost)
        {
            nutrientTracker.SubtractNutrients(leechingSporeNutrientCost);
            nutrientTracker.SpendFlesh(leechingSporeMaterialCost);
            currentstats.skillEquippables["LeechingSpore"] = true;
            SkillDesc.text = "Leeching Spores: <br><br> <size=25>Infest a nearby enemy with a leeching spore. The spore steals health every second from the enemy and restores it to you.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseSporeburst()
    {
        if (currentstats.vitalityLevel >= 10 && currentstats.skillEquippables["Sporeburst"] == false && nutrientTracker.currentNutrients >= sporeburstNutrientCost && nutrientTracker.storedFlesh >= sporeburstMaterialCost)
        {
            nutrientTracker.SubtractNutrients(sporeburstNutrientCost);
            nutrientTracker.SpendFlesh(sporeburstMaterialCost);
            currentstats.skillEquippables["Sporeburst"] = true;
            SkillDesc.text = "Sporeburst: <br><br> <size=25>Spores explode from you stunning and damaging all enemies caught in its radius. Heal for 50% of all damage dealt.";
        }
        else
        {
            return;
        }
    }
    public void PurchaseDefenseMechanism()
    {
        if (currentstats.vitalityLevel >= 15 && currentstats.skillEquippables["DefenseMechanism"] == false && nutrientTracker.currentNutrients >= defenseMechanismNutrientCost && nutrientTracker.storedFlesh >= defenseMechanismMaterialCost)
        {
            nutrientTracker.SubtractNutrients(defenseMechanismNutrientCost);
            nutrientTracker.SpendFlesh(defenseMechanismMaterialCost);
            currentstats.skillEquippables["DefenseMechanism"] = true;
            SkillDesc.text = "Defense Mechanism: <br><size=25>Reduces damage taken by 50% for 1 second. Attacks against you while Defense Mechanism is active is stored as bonus damage on your next attack equal to 50% of the damage absorbed.";
        }
        else
        {
            return;
        }
    }
}
