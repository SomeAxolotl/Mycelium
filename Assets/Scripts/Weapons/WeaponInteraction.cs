using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RonaldSunglassesEmoji.Interaction;
using System;

public class WeaponInteraction : MonoBehaviour, IInteractable
{
    SwapWeapon swapWeapon;
    [SerializeField] private int nutrientsSalvaged = 200;
    NutrientTracker nutrientTracker;
    GameObject player;
    HUDStats hudStats;

    public string attributeDescription = "";
    public Action WeaponEquipped;
    public Action WeaponUnequipped;

    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        swapWeapon = GameObject.Find("PlayerParent").GetComponent<SwapWeapon>();
        hudStats = GameObject.Find("HUD").GetComponent<HUDStats>();
    }

    GameObject curWeapon;
    public void Interact(GameObject interactObject)
    {
        //What you have now
        GameObject curWeapon = swapWeapon.curWeapon;
        AttributeBase curAtt = curWeapon.GetComponent<AttributeBase>();
        if(curAtt != null){curAtt.Unequipped();}
        //What you swap into
        Transform weapon = interactObject.transform;
        AttributeBase newAtt = weapon.GetComponent<AttributeBase>();
        if(newAtt != null){newAtt.Equipped();}

        if (curAtt != null && curAtt.statChange)
        {
            hudStats.FlashHUDStats();
        }
        else if (newAtt != null && newAtt.statChange)
        {
            hudStats.FlashHUDStats();
        }

        Transform weaponHolder = swapWeapon.weaponHolder;

        SoundEffectManager.Instance.PlaySound("Pickup", transform.position);

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
        swapWeapon.curWeapon = GameObject.FindWithTag("currentWeapon");
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
        if (interactObject.transform.tag == "Weapon" && interactObject.transform.tag != "currentWeapon")
        {
            SalvageNutrients(nutrientsSalvaged);
        }
    }

    public void CreateTooltip(GameObject interactObject)
    {
        GameObject curWeapon = swapWeapon.curWeapon;
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
        float damageDifference = newStats.wpnBaseDmg - oldStats.wpnBaseDmg;
        float knockDifference = newStats.wpnKnockback - oldStats.wpnKnockback;
        if (newStats.wpnBaseDmg > oldStats.wpnBaseDmg)
        {
            damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +" + "(" + damageDifference.ToString("F1") + ")" + "</color>";
        }
        else if (newStats.wpnBaseDmg < oldStats.wpnBaseDmg)
        {
            damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(worseStatColor) + "> -" + "(" + Mathf.Abs(damageDifference).ToString("F1") + ")" + "</color>";
        }
        else
        {
            damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(evenStatColor) + "> +-</color>";
        }
        string knockbackComparisonText;
        if (newStats.wpnKnockback > oldStats.wpnKnockback)
        {
            knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +" + "(" + knockDifference.ToString("F1") + ")" + "</color>";
        }
        else if (newStats.wpnKnockback < oldStats.wpnKnockback)
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
        //Displays common rarity when there is no Attribute
        AttributeAssigner.Rarity rating;
        if(weapon.GetComponent<AttributeBase>() == null){
            rating = AttributeAssigner.Rarity.Common;
        }else{
            rating = weapon.GetComponent<AttributeBase>().rating;
        }
        TooltipManager.Instance.CreateTooltip
            (
                weapon.gameObject, 
                weaponName, 
                "Type: " + newStats.weaponType.ToString() + 
                "\nDamage: " + newStats.wpnBaseDmg.ToString("F1") + damageComparisonText + 
                "\nKnockback: " + newStats.wpnKnockback.ToString("F1") + knockbackComparisonText
                + attributeDescription, 
                "Press "+interactText+" to Swap",
                "Press "+salvageText+" to Salvage",
                false,
                0,
                true,
                rating
            );
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        GameObject curWeapon = swapWeapon.curWeapon;
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
        nutrientTracker.AddNutrients(nutrientAmount);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientAmount / 20, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        TooltipManager.Instance.DestroyTooltip();
        Destroy(this.gameObject);
    }
}
