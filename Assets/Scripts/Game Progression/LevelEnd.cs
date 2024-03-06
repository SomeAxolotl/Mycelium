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
            if (sceneIndexToGoTo == 2)
            {
                swapWeapon.curWeapon.tag = "Weapon";
                GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
                foreach (GameObject weapon in weapons)
                Destroy(weapon);
            }
            nutrientTracker.KeepMaterials();
            nutrientTracker.LoseMaterials();
            profileManager.SaveOverride();
            sceneLoaderScript.BeginLoadScene(sceneIndexToGoTo, true);
        }
    }
}
