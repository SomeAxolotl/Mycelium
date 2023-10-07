using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDWeapon : MonoBehaviour
{
    private Image weaponImage;
    private Sprite weaponSprite;

    void Start()
    {
        weaponImage = GameObject.Find("WeaponIcon").GetComponent<Image>();

        UpdateWeapon("Start");
    }

    public void UpdateWeapon(string weaponName)
    {
        if (weaponName.Contains("Start"))
        {
            weaponSprite = Resources.Load<Sprite>("TestStabIcon"); 
        }
        if (weaponName.Contains("Slash"))
        {
            weaponSprite = Resources.Load<Sprite>("TestSlashIcon"); 
        }
        else if (weaponName.Contains("Smash"))
        {
            weaponSprite = Resources.Load<Sprite>("TestSmashIcon"); 
        }

        weaponImage.sprite = weaponSprite;
    }
}
