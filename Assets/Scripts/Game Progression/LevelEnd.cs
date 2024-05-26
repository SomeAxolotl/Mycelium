using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    bool hasCollided = false;

    [SerializeField] private int sceneIndexToGoTo;
    [SerializeField] public bool isCheckpoint;

    NutrientTracker nutrientTracker;
    SwapWeapon swapWeapon;
    ProfileManager profileManager;
    WeaponStats weaponStats;
    CharacterStats characterStats;
    PlayerHealth playerHealth;
    PlayerController playerController;

    void Start()
    {
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        swapWeapon = GameObject.Find("PlayerParent").GetComponent<SwapWeapon>();
        profileManager = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
        characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        playerHealth = GameObject.Find("PlayerParent").GetComponent<PlayerHealth>();
        playerController = GameObject.Find("PlayerParent").GetComponent<PlayerController>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "currentPlayer" && !hasCollided)
        {
            hasCollided = true;
            GlobalData.isAbleToPause = false;

            playerHealth.isInvincible = true;
            playerController.DisableController();            

            if(isCheckpoint == true)
            {
                EndOfLevelCanvasStuff.Instance.StartEndOfLevel(this);
            }
            else
            {
                //notification stuff
                string currentHeldMaterial = nutrientTracker.GetCurrentHeldMaterial();
                string completeMessage = "<color=#7FFF00>AREA COMPLETE</color>";
                if (currentHeldMaterial != "")
                {
                    NotificationManager.Instance.Notification(completeMessage, currentHeldMaterial + " stored at the Carcass", null, currentHeldMaterial);
                }
                else
                {
                    NotificationManager.Instance.Notification(completeMessage);
                }

                //Increment Area Completion Count
                GlobalData.areaCleared = true;
                GlobalData.areasClearedThisRun ++;

                EndOfLevelCanvasStuff.Instance.StartEndOfLevel(this);
            }
        }
    }

    public void SceneChangeSetup()
    {
        playerHealth.currentHealth = playerHealth.maxHealth;

        nutrientTracker.KeepMaterials();
        nutrientTracker.LoseMaterials();
        profileManager.SaveOverride();
    }

    public void GoToNextLevel()
    {
        SceneChangeSetup();

        weaponStats = swapWeapon.curWeapon.GetComponent<WeaponStats>();

        //Save Current Weapon
        GlobalData.currentWeapon = weaponStats.weaponType.ToString() + "/" + swapWeapon.curWeapon.name.Replace("(Clone)", "");
        //Weapon Stats
        GlobalData.currentWeaponDamage = weaponStats.wpnBaseDmg;
        GlobalData.currentWeaponMult = weaponStats.wpnMult;
        GlobalData.currentWeaponKnockback = weaponStats.wpnKnockback;
        //Unequip Weapon
        AttributeBase currAtt = swapWeapon.curWeapon.GetComponent<AttributeBase>();
        if(currAtt != null){
            GlobalData.specialAttNum = currAtt.specialAttNum;
            currAtt.Unequipped();
            GlobalData.currentAttribute = currAtt.GetType().Name;
        }else{
            GlobalData.currentAttribute = "Nothing";
        }

        //Save Current Stats
        GlobalData.currentSporeStats.Add(characterStats.primalLevel);
        GlobalData.currentSporeStats.Add(characterStats.speedLevel);
        GlobalData.currentSporeStats.Add(characterStats.sentienceLevel);
        GlobalData.currentSporeStats.Add(characterStats.vitalityLevel);

        SceneLoader.Instance.BeginLoadScene(sceneIndexToGoTo, "Dylan is a tree");
    }

    public void GoToTheCarcass()
    {
        SceneChangeSetup();

        swapWeapon.curWeapon.tag = "Weapon";
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject weapon in weapons)
        {
            Destroy(weapon);
        }
        GlobalData.currentWeapon = null;
        GlobalData.currentLoop = 1;
        

        SceneLoader.Instance.BeginLoadScene("The Carcass", "Dylan is a tree");
    }

    public void GoBackToGameplay()
    {
        hasCollided = false;
        GlobalData.isAbleToPause = true;

        playerHealth.isInvincible = false;
        playerController.EnableController();
    }

    public void SpecialCreditsLoopFunction()
    {
        //This is only meant to be used by the credits player

        nutrientTracker.KeepMaterials();
        nutrientTracker.LoseMaterials();
        profileManager.SaveOverride();

        weaponStats = swapWeapon.curWeapon.GetComponent<WeaponStats>();

        //Save Current Weapon
        GlobalData.currentWeapon = weaponStats.weaponType.ToString() + "/" + swapWeapon.curWeapon.name.Replace("(Clone)", "");
        //Weapon Stats
        GlobalData.currentWeaponDamage = weaponStats.wpnBaseDmg;
        GlobalData.currentWeaponMult = weaponStats.wpnMult;
        GlobalData.currentWeaponKnockback = weaponStats.wpnKnockback;
        //Unequip Weapon
        AttributeBase currAtt = swapWeapon.curWeapon.GetComponent<AttributeBase>();
        if(currAtt != null){
            currAtt.Unequipped();
            GlobalData.currentAttribute = currAtt.GetType().Name;
        }else{
            GlobalData.currentAttribute = "Nothing";
        }

        //Save Current Stats
        GlobalData.currentSporeStats.Add(characterStats.primalLevel);
        GlobalData.currentSporeStats.Add(characterStats.speedLevel);
        GlobalData.currentSporeStats.Add(characterStats.sentienceLevel);
        GlobalData.currentSporeStats.Add(characterStats.vitalityLevel);
    }
}
