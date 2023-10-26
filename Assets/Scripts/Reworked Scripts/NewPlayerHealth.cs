using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerHealth : MonoBehaviour
{
    float maxHealth;
    public float currentHealth;
    float regenRate;
    private SwapCharacter swapCharacter;
    // Start is called before the first frame update
    void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        maxHealth = swapCharacter.currentCharacterStats.baseHealth;
        regenRate = swapCharacter.currentCharacterStats.baseRegen;
        currentHealth = maxHealth;
        InvokeRepeating("Regen", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = swapCharacter.currentCharacterStats.baseHealth;
        regenRate = swapCharacter.currentCharacterStats.baseRegen;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if(currentHealth <= 0)
        {
            Death();
        }
        Debug.Log("Player Health: " +  currentHealth);
    }
    public void PlayerTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;
    }
    void Death()
    {
        Debug.Log("you died!");
    }
    void Regen()
    {
        currentHealth += regenRate;
    }
}
