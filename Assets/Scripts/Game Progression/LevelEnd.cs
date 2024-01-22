using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    NutrientTracker nutrientTracker;
    LevelLoader levelLoaderScript;
    void Start()
    {
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        levelLoaderScript = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "currentPlayer")
        {
            other.GetComponentInParent<PlayerHealth>().currentHealth = other.GetComponentInParent<PlayerHealth>().maxHealth;
            DontDestroyOnLoad(other.GetComponentInParent<SwapWeapon>().curWeapon);
            GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
            foreach (GameObject weapon in weapons)
            Destroy(weapon);
            nutrientTracker.KeepMaterials();
            nutrientTracker.LoseMaterials();
            //SceneManager.LoadScene(1);
            levelLoaderScript.BeginLoadScene(1);
        }
    }
}
