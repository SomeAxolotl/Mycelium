using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    NutrientTracker nutrientTracker;
    SceneLoader sceneLoaderScript;
    void Start()
    {
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
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
            sceneLoaderScript.BeginLoadScene(1, true);
        }
    }
}
