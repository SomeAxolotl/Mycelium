using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] int sceneIndexToGoTo;

    NutrientTracker nutrientTracker;
    SceneLoader sceneLoaderScript;
    SwapWeapon swapWeapon;
    ProfileManager profileManager;
    WeaponStats weaponStats;
    void Start()
    {
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        swapWeapon = GameObject.Find("PlayerParent").GetComponent<SwapWeapon>();
        profileManager = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "currentPlayer")
        {
            other.GetComponentInParent<PlayerHealth>().currentHealth = other.GetComponentInParent<PlayerHealth>().maxHealth;

            weaponStats = swapWeapon.curWeapon.GetComponent<WeaponStats>();
            Debug.Log(weaponStats.weaponType.ToString() + "/" + swapWeapon.curWeapon.name.Replace("(Clone)", ""));
            GlobalData.currentWeapon = weaponStats.weaponType.ToString() + "/" + swapWeapon.curWeapon.name.Replace("(Clone)", "");
            Debug.Log(GlobalData.currentWeapon);

            if (sceneIndexToGoTo == 2)
            {
                swapWeapon.curWeapon.tag = "Weapon";
                GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
                foreach (GameObject weapon in weapons)
                Destroy(weapon);

                GlobalData.currentWeapon = null;
            }
            nutrientTracker.KeepMaterials();
            nutrientTracker.LoseMaterials();
            profileManager.SaveOverride();
            sceneLoaderScript.BeginLoadScene(sceneIndexToGoTo, true);
        }
    }
}
