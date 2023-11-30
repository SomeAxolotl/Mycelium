using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PurchaseSkills : MonoBehaviour
{
    public CharacterStats currentstats;
    public NutrientTracker nutrientTracker;

    private int eruptionNutrientCost = 2500;
    private int eruptionMaterialCost = 1;

    private int cycloneNutrientCost = 5000;
    private int cycloneMaterialCost = 2;

    private int furyNutrientCost = 10000;
    private int furyMaterialCost = 3;

    private int blitzNutrientCost = 2500;
    private int blitzMaterialCost = 1;

    private int cascadeNutrientCost = 5000;
    private int cascadeMaterialCost = 2;

    private int mycotoxinsNutrientCost = 10000;
    private int mycotoxinsMaterialCost = 3;

    private int spineshotNutrientCost = 2500;
    private int spineshotMaterialCost = 1;

    private int puffballNutrientCost = 5000;
    private int puffballMaterialCost = 2;

    private int undergrowthNutrientCost = 10000;
    private int undergrowthMaterialCost = 3;

    private int leechingSporeNutrientCost = 2500;
    private int leechingSporeMaterialCost = 1;

    private int sporeburstNutrientCost = 5000;
    private int sporeburstMaterialCost = 2;

    private int defenseMechanismNutrientCost = 10000;
    private int defenseMechanismMaterialCost = 3;
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
        }
        else
        {
            return;
        }
    }
}
