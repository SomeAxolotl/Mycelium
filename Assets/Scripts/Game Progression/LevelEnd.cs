using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    bool hasCollided = false;

    [SerializeField] int sceneIndexToGoTo;

    NutrientTracker nutrientTracker;
    SceneLoader sceneLoaderScript;
    SwapWeapon swapWeapon;
    ProfileManager profileManager;
    WeaponStats weaponStats;
    CharacterStats characterStats;
    void Start()
    {
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        swapWeapon = GameObject.Find("PlayerParent").GetComponent<SwapWeapon>();
        profileManager = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
        characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "currentPlayer")
        {
            hasCollided = true;

            other.GetComponentInParent<PlayerHealth>().currentHealth = other.GetComponentInParent<PlayerHealth>().maxHealth;

            weaponStats = swapWeapon.curWeapon.GetComponent<WeaponStats>();

            //Save Current Weapon
            GlobalData.currentWeapon = SceneManager.GetActiveScene().name + "/" + weaponStats.weaponType.ToString() + "/" + swapWeapon.curWeapon.name.Replace("(Clone)", "");
            GlobalData.currentWeaponDamage = weaponStats.wpnDamage;
            GlobalData.currentWeaponKnockback = weaponStats.wpnKnockback;

            //Save Current Stats
            GlobalData.currentSporeStats.Add(characterStats.primalLevel);
            GlobalData.currentSporeStats.Add(characterStats.speedLevel);
            GlobalData.currentSporeStats.Add(characterStats.sentienceLevel);
            GlobalData.currentSporeStats.Add(characterStats.vitalityLevel);

            if (sceneIndexToGoTo == 2)
            {
                swapWeapon.curWeapon.tag = "Weapon";
                GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
                foreach (GameObject weapon in weapons)
                Destroy(weapon);

                GlobalData.currentWeapon = null;
            }
            nutrientTracker.KeepMaterials();

            string currentHeldMaterial = nutrientTracker.GetCurrentHeldMaterial();
            string completeMessage = "<color=#7FFF00>AREA COMPLETE</color>";
            if (currentHeldMaterial != "")
            {
                Debug.Log("wtf");
                NotificationManager.Instance.Notification(completeMessage, currentHeldMaterial + " stored at the Carcass", null, currentHeldMaterial);
            }
            /*else
            {
                NotificationManager.Instance.Notification(completeMessage);
            }*/

            nutrientTracker.LoseMaterials();
            profileManager.SaveOverride();

            sceneLoaderScript.BeginLoadScene(sceneIndexToGoTo, "Dylan is a tree");
        }
    }
}
