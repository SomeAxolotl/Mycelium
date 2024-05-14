using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RonaldSunglassesEmoji.Interaction;

public class WeaponInteraction : MonoBehaviour, IInteractable
{
    SwapWeapon swapWeapon;
    [SerializeField] private int nutrientsSalvaged = 200;
    NutrientTracker nutrientTracker;
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
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

        ApplyWeaponPositionAndRotation();

        TooltipManager.Instance.DestroyTooltip();
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
        Transform weapon = interactObject.transform;
        Transform weaponHolder = swapWeapon.weaponHolder;

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
        TooltipManager.Instance.CreateTooltip
            (
                weapon.gameObject, 
                weaponName, 
                "Type: " + newStats.weaponType.ToString() + 
                "\nDamage: " + newStats.wpnBaseDmg.ToString("F1") + damageComparisonText + 
                "\nKnockback: " + newStats.wpnKnockback.ToString("F1") + knockbackComparisonText, 
                "Press "+interactText+" to Swap",
                "Press "+salvageText+" to Salvage"
            );
    }

    public void DestroyTooltip(GameObject interactObject)
    {
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
