using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "currentPlayer")
        {
            other.GetComponentInParent<NewPlayerHealth>().currentHealth = other.GetComponentInParent<NewPlayerHealth>().maxHealth;
            DontDestroyOnLoad(other.GetComponentInParent<SwapWeapon>().curWeapon);
            GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
            foreach (GameObject weapon in weapons)
            Destroy(weapon);
            SceneManager.LoadScene("HubWorldPlaceholder");
        }
        //other.gameObject.transform.position = new Vector3(0, 1.4f, 0f);
    }
}
