using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RonaldSunglassesEmoji.Interaction;
using System;
using System.Linq;

public class WeaponInteraction : MonoBehaviour, IInteractable
{
    SwapWeapon swapWeapon;
    [SerializeField] private int nutrientsSalvaged = 200;
    NutrientTracker nutrientTracker;
    GameObject player;
    HUDStats hudStats;
    AttributeManager attManager;

    public string attributeDescription = "";
    public Action WeaponEquipped;
    public Action WeaponUnequipped;

    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        swapWeapon = GameObject.Find("PlayerParent").GetComponent<SwapWeapon>();
        hudStats = GameObject.Find("HUD").GetComponent<HUDStats>();
        attManager = GetComponent<AttributeManager>();
    }

    GameObject curWeapon;
    public void Interact(GameObject interactObject)
    {
        bool showStats = false;
        //What you have now
        GameObject curWeapon = swapWeapon.O_curWeapon;
        List<AttributeBase> currAtt = curWeapon.GetComponents<AttributeBase>().ToList();
        if(currAtt.Count > 0){
            foreach(AttributeBase attBase in currAtt){
                attBase.Unequipped();
                if(attBase.statChange){
                    showStats = true;
                }
            }
        }
        //What you swap into
        Transform weapon = interactObject.transform;

        List<AttributeBase> newAtt = weapon.GetComponents<AttributeBase>().ToList();
        if(newAtt.Count > 0){
            foreach(AttributeBase attBase in newAtt){
                attBase.Equipped();
                if(attBase.statChange){
                    showStats = true;
                }
            }
        }
        //Show stats if one of them involves stats
        if(showStats){
            hudStats.FlashHUDStats();
        }

        Transform weaponHolder = swapWeapon.weaponHolder;

        SoundEffectManager.Instance.PlaySound("Pickup", transform);

        curWeapon.transform.position = weapon.transform.position;
        curWeapon.GetComponent<Collider>().enabled = true;
        weapon.transform.position = weaponHolder.position;
        curWeapon.layer = LayerMask.NameToLayer("Weapon");
        curWeapon.transform.parent = null;

        curWeapon.tag = "Weapon";
        weapon.gameObject.layer = LayerMask.NameToLayer("currentWeapon");
        weapon.GetComponent<Collider>().enabled = false;
        weapon.tag = "currentWeapon";

        ApplyWeaponPositionAndRotation();
    }

    public void ApplyWeaponPositionAndRotation()
    {
        swapWeapon.O_curWeapon = GameObject.FindWithTag("currentWeapon");
        Transform weaponHolder = swapWeapon.weaponHolder;

        transform.parent = weaponHolder.transform;
        transform.rotation = weaponHolder.transform.rotation;

        Vector3 positionOffset = GetComponent<WeaponStats>().holdPositionOffset;
        transform.localPosition = positionOffset;
        Vector3 rotationOffset = GetComponent<WeaponStats>().holdRotationOffset;
        weaponHolder.localEulerAngles = rotationOffset;
    }

    public void Salvage(GameObject interactObject)
    {
        GameObject curWeapon = swapWeapon.O_curWeapon;
        AttributeBase curAtt = curWeapon.GetComponent<AttributeBase>();
        Transform weapon = interactObject.transform;
        AttributeBase newAtt = weapon.GetComponent<AttributeBase>();

        if (curAtt != null && curAtt.statChange)
        {
            hudStats.HideStats();
        }
        else if (newAtt != null && newAtt.statChange)
        {
            hudStats.HideStats();
        }

        if (interactObject.transform.tag == "Weapon" && interactObject.transform.tag != "currentWeapon")
        {
            SalvageNutrients(nutrientsSalvaged);
        }
    }

    public void CreateTooltip(GameObject interactObject)
    {
        StartCoroutine(CreateTooltipWithDelay(interactObject));
    }
    IEnumerator CreateTooltipWithDelay(GameObject interactObject)
    {
        yield return null;

        GameObject curWeapon = swapWeapon.O_curWeapon;
        AttributeBase curAtt = curWeapon.GetComponent<AttributeBase>();
        Transform weapon = interactObject.transform;
        AttributeBase newAtt = weapon.GetComponent<AttributeBase>();
        Transform weaponHolder = swapWeapon.weaponHolder;

        if (curAtt != null && curAtt.statChange)
        {
            hudStats.ShowStats();
        }
        else if (newAtt != null && newAtt.statChange)
        {
            hudStats.ShowStats();
        }
        else
        {
            hudStats.HideStats();
        }
        
        /*
        if(curAtt != null && newAtt != null){
            if(newAtt.statChange || curAtt.statChange){
                hudStats.ShowStats();
            }
        }else{
            if(curAtt != null && curAtt.statChange){
                hudStats.ShowStats();
            }
            if(newAtt != null && newAtt.statChange){
                hudStats.ShowStats();
            }
        }
        */

        Color betterStatColor = swapWeapon.betterStatColor;
        Color worseStatColor = swapWeapon.worseStatColor;
        Color evenStatColor = swapWeapon.evenStatColor;

        weapon = interactObject.transform;
        WeaponStats oldStats = curWeapon.GetComponent<WeaponStats>();
        WeaponStats newStats = weapon.gameObject.GetComponent<WeaponStats>();
        string damageComparisonText;

        float currentDamage = oldStats.statNums.advDamage.ReadPercentFromSource(oldStats) + 1;
        AttributeBase[] currAtts = curWeapon.GetComponents<AttributeBase>();
        foreach(AttributeBase currAtt in currAtts){currentDamage += oldStats.statNums.advDamage.ReadPercentFromSource(currAtt);}
        currentDamage *= oldStats.statNums.advDamage.ReadPercentMultFromSource(oldStats);
        Debug.Log("CURRENT DAMAGE: " + currentDamage);
        currentDamage *= 100;

        float newDamage = newStats.statNums.advDamage.ReadPercentFromSource(newStats) + 1;
        AttributeBase[] newAtts = weapon.gameObject.GetComponents<AttributeBase>();
        foreach(AttributeBase currAtt in newAtts){newDamage += newStats.statNums.advDamage.ReadPercentFromSource(currAtt);}
        //Debug.Log("CURRENT DAMAGE: " + newDamage);
        newDamage *= newStats.statNums.advDamage.ReadPercentMultFromSource(newStats);
        newDamage *= 100;

        Debug.Log("Current damage: " + currentDamage + "     New damage: " + newDamage);

        float damageDifference = newDamage - currentDamage;
        float knockDifference = newStats.statNums.advKnockback.Value - oldStats.statNums.advKnockback.Value;
        if (newDamage > currentDamage)
        {
            damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +" + "(" + (damageDifference).ToString("F0") + "%)" + "</color>";
        }
        else if (newDamage < currentDamage)
        {
            damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(worseStatColor) + "> -" + "(" + Mathf.Abs(damageDifference).ToString("F0") + "%)" + "</color>";
        }
        else
        {
            damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(evenStatColor) + "> +-</color>";
        }
        string knockbackComparisonText;
        if (newStats.statNums.advKnockback.Value > oldStats.statNums.advKnockback.Value)
        {
            knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +" + "(" + knockDifference.ToString("F1") + ")" + "</color>";
        }
        else if (newStats.statNums.advKnockback.Value < oldStats.statNums.advKnockback.Value)
        {
            knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(worseStatColor) + "> -" + "(" + Mathf.Abs(knockDifference).ToString("F1") + ")" + "</color>";
        }
        else
        {
            knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(evenStatColor) + "> +-</color>";
        }

        string weaponName = newStats.wpnName;
        string interactText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        string salvageText = InputManager.Instance.GetLatestController().salvageHint.GenerateColoredHintString();

        TooltipManager.Instance.CreateTooltip
            (
                weapon.gameObject, 
                weaponName, 
                "Type: " + newStats.weaponType.ToString() + 
                "\nDamage: " + (newStats.statNums.advDamage.PercentValue) + "%" + damageComparisonText + 
                "\nKnockback: " + newStats.statNums.advKnockback.Value.ToString("F1") + knockbackComparisonText
                + attributeDescription, 
                "Press "+interactText+" to Swap",
                "Press "+salvageText+" to Salvage",
                false,
                0,
                true,
                attManager.O_highestRarity
            );
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        GameObject curWeapon = swapWeapon.O_curWeapon;
        AttributeBase curAtt = curWeapon.GetComponent<AttributeBase>();
        Transform weapon = interactObject.transform;
        AttributeBase newAtt = weapon.GetComponent<AttributeBase>();

        if (!isFromInteracting)
        {
            if (curAtt != null && curAtt.statChange)
            {
                hudStats.HideStats();
            }
            else if (newAtt != null && newAtt.statChange)
            {
                hudStats.HideStats();
            }
        }

        TooltipManager.Instance.DestroyTooltip();
    }
    void SalvageNutrients(int nutrientAmount)
    {
        if (GlobalData.currentLoop >= 2)
        {
            nutrientAmount = (nutrientAmount * (GlobalData.currentLoop / 2));
        }
        Actions.SalvagedWeapon?.Invoke(this.gameObject);
        nutrientTracker.AddNutrients(nutrientAmount);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientAmount / 20, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        TooltipManager.Instance.DestroyTooltip();
        Destroy(this.gameObject);
    }
}
