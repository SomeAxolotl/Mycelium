using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    NutrientTracker nutrientTracker;
    void Start()
    {
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "currentPlayer")
        {
            other.GetComponentInParent<NewPlayerHealth>().currentHealth = other.GetComponentInParent<NewPlayerHealth>().maxHealth;
            DontDestroyOnLoad(other.GetComponentInParent<SwapWeapon>().curWeapon);
            GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
            foreach (GameObject weapon in weapons)
            Destroy(weapon);
            nutrientTracker.KeepMaterials();
            nutrientTracker.LoseMaterials();
            SceneManager.LoadScene(1);
        }
    }
}
