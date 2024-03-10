using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RonaldSunglassesEmoji.Interaction;

public class WeaponInteraction : MonoBehaviour, IInteractable
{
    SwapWeapon swapWeapon;

    void Start()
    {
        swapWeapon = GameObject.Find("PlayerParent").GetComponent<SwapWeapon>();
    }

    public void Interact(GameObject interactObject)
    {
        GameObject curWeapon = swapWeapon.curWeapon;
        Transform weapon = interactObject.transform;
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
        
        swapWeapon.curWeapon = GameObject.FindWithTag("currentWeapon");
        curWeapon = swapWeapon.curWeapon;
        
        curWeapon.transform.parent = weaponHolder.transform;
        curWeapon.transform.rotation = weaponHolder.transform.rotation;

        Vector3 positionOffset = curWeapon.GetComponent<WeaponStats>().holdPositionOffset;
        curWeapon.transform.localPosition = positionOffset;
        Vector3 rotationOffset = curWeapon.GetComponent<WeaponStats>().holdRotationOffset;
        weaponHolder.transform.localEulerAngles = rotationOffset;

        TooltipManager.Instance.DestroyTooltip();
    }

    public void Salvage(GameObject interactObject)
    {
        //buh
    }

    public void CreateTooltip(GameObject interactObject)
    {
        GameObject curWeapon = swapWeapon.curWeapon;
        Transform weapon = interactObject.transform;
        Transform weaponHolder = swapWeapon.weaponHolder;

        Color betterStatColor = swapWeapon.betterStatColor;
        Color worseStatColor = swapWeapon.worseStatColor;
        Color evenStatColor = swapWeapon.evenStatColor;

        weapon = interactObject.transform;
        WeaponStats oldStats = curWeapon.GetComponent<WeaponStats>();
        WeaponStats newStats = weapon.gameObject.GetComponent<WeaponStats>();
        string damageComparisonText;
        if (newStats.wpnDamage > oldStats.wpnDamage)
        {
            damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +</color>";
        }
        else if (newStats.wpnDamage < oldStats.wpnDamage)
        {
            damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(worseStatColor) + "> -</color>";
        }
        else
        {
            damageComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(evenStatColor) + "> +-</color>";
        }
        string knockbackComparisonText;
        if (newStats.wpnKnockback > oldStats.wpnKnockback)
        {
            knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(betterStatColor) + "> +</color>";
        }
        else if (newStats.wpnKnockback < oldStats.wpnKnockback)
        {
            knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(worseStatColor) + "> -</color>";
        }
        else
        {
            knockbackComparisonText = "<color=#" + ColorUtility.ToHtmlStringRGB(evenStatColor) + "> +-</color>";
        }

        string weaponName = newStats.wpnName;
        string buttonText = "<color=#3cdb4e>A</color>";
        TooltipManager.Instance.CreateTooltip
            (weapon.gameObject, 
            weaponName, 
            "Speed: " + newStats.wpnSpeed + 
            "\nDamage: " + newStats.wpnDamage.ToString("F1") + damageComparisonText + 
            "\nKnockback: " + newStats.wpnKnockback.ToString("F1") + knockbackComparisonText, 
            "Press "+buttonText+" to Swap"
            );
    }

    public void DestroyTooltip(GameObject interactObject)
    {
        TooltipManager.Instance.DestroyTooltip();
    }
}