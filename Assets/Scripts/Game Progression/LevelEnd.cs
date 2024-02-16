using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    NutrientTracker nutrientTracker;
    SceneLoader sceneLoaderScript;
    SwapWeapon swapWeapon;
    void Start()
    {
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        swapWeapon = GameObject.Find("PlayerParent").GetComponent<SwapWeapon>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "currentPlayer")
        {
            other.GetComponentInParent<PlayerHealth>().currentHealth = other.GetComponentInParent<PlayerHealth>().maxHealth;
            swapWeapon.curWeapon.tag = "Weapon";
            GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
            foreach (GameObject weapon in weapons)
            Destroy(weapon);
            nutrientTracker.KeepMaterials();
            nutrientTracker.LoseMaterials();
            sceneLoaderScript.BeginLoadScene(2, true);
        }
    }
}
