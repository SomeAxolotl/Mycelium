using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    float regenRate;
    private SwapCharacter swapCharacter;

    private HUDHealth hudHealth;

    // Start is called before the first frame update
    void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        maxHealth = swapCharacter.currentCharacterStats.baseHealth;
        regenRate = swapCharacter.currentCharacterStats.baseRegen;
        currentHealth = maxHealth;
        InvokeRepeating("Regen", 1f, 1f);

        hudHealth = GameObject.Find("HUD").GetComponent<HUDHealth>();
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
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
    }
    public void PlayerHeal(float healAmount)
    {
        currentHealth += healAmount;
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
    }
    void Death()
    {
        Debug.Log("you died!");
    }
    void Regen()
    {
        PlayerHeal(regenRate);
    }
}
