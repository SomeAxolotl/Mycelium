using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmackableMushroom : MonoBehaviour
{
    public GameObject hiddenSpore; // Reference to the hidden spore (enemy) GameObject

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player hits the smackable mushroom
        if (other.CompareTag("currentWeapon"))
        {
            Debug.Log("SmackableMushroom hit by currentWeapon"); // Debug statement

            // Trigger the hidden spore's activation method
            hiddenSpore.GetComponent<HiddenSpore>().ActivateHiddenSpore();

            // Destroy the smackable mushroom
            Destroy(gameObject);
        }
    }
}
