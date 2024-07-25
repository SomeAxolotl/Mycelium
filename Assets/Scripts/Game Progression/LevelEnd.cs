using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Linq;

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
        playerHealth.closeCall = false;
        playerHealth.currentHealth = playerHealth.maxHealth;

        nutrientTracker.KeepMaterials();
        nutrientTracker.LoseMaterials();
        profileManager.SaveOverride();
    }

    public void GoToNextLevel()
    {
        SceneChangeSetup();

        weaponStats = swapWeapon.O_curWeapon.GetComponent<WeaponStats>();

        //Save Current Weapon
        GlobalData.currentWeapon = weaponStats.weaponType.ToString() + "/" + swapWeapon.O_curWeapon.name.Replace("(Clone)", "");
        //Unequip Weapon
        List<AttributeBase> currAtt = swapWeapon.O_curWeapon.GetComponents<AttributeBase>().ToList();
        if(currAtt.Count > 0){
            foreach(AttributeBase attBase in currAtt){
                AttributeInfo newInfo = new AttributeInfo();
                newInfo.attName = attBase.GetType().Name; //Needs to get the component name cause sometimes attribute names != component name
                newInfo.attValue = attBase.specialAttNum;
                newInfo.rating = attBase.rating;
                GlobalData.currentAttribute.Add(newInfo);
                attBase.Unequipped();
                //Clears stats when a level in unloaded so the data from components is not saved
                weaponStats.ClearAllStatsFrom(attBase);
            }
        }else{
            AttributeInfo newInfo = new AttributeInfo();
            GlobalData.currentAttribute.Add(newInfo);
        }
        //Weapon Stats
        GlobalData.currentWeaponStats = weaponStats.statNums;

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

        swapWeapon.O_curWeapon.tag = "Weapon";
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject weapon in weapons)
        {
            Destroy(weapon);
        }
        GlobalData.currentWeapon = null;

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

        weaponStats = swapWeapon.O_curWeapon.GetComponent<WeaponStats>();

        //Save Current Weapon
        GlobalData.currentWeapon = weaponStats.weaponType.ToString() + "/" + swapWeapon.O_curWeapon.name.Replace("(Clone)", "");
        //Weapon Stats
        GlobalData.currentWeaponStats = weaponStats.statNums;
        //Unequip Weapon
        List<AttributeBase> currAtt = swapWeapon.O_curWeapon.GetComponents<AttributeBase>().ToList();
        if(currAtt.Count > 0){
            foreach(AttributeBase attBase in currAtt){
                AttributeInfo newInfo = new AttributeInfo();
                newInfo.attName = attBase.GetType().Name; //Needs to get the component name cause sometimes attribute names != component name
                newInfo.attValue = attBase.specialAttNum;
                newInfo.rating = attBase.rating;
                GlobalData.currentAttribute.Add(newInfo);
                attBase.Unequipped();
                //Clears stats when a level in unloaded so the data from components is not saved
                weaponStats.ClearAllStatsFrom(attBase);
            }
        }else{
            AttributeInfo newInfo = new AttributeInfo();
            GlobalData.currentAttribute.Add(newInfo);
        }

        //Save Current Stats
        GlobalData.currentSporeStats.Add(characterStats.primalLevel);
        GlobalData.currentSporeStats.Add(characterStats.speedLevel);
        GlobalData.currentSporeStats.Add(characterStats.sentienceLevel);
        GlobalData.currentSporeStats.Add(characterStats.vitalityLevel);
    }
}
