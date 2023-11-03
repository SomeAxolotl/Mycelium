using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewPlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    float regenRate;
    private SwapCharacter swapCharacter;
    private HUDHealth hudHealth;
    float deathTimer;

    // Start is called before the first frame update
    void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        GetHealthStats();
        currentHealth = maxHealth;
        InvokeRepeating("Regen", 1f, 1f);

        hudHealth = GameObject.Find("HUD").GetComponent<HUDHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, -100, maxHealth);
        
        if(currentHealth <= 0)
        {
            currentHealth = -100;
            hudHealth.UpdateHealthUI(0, maxHealth);
            deathTimer += Time.deltaTime;
            Death();
        }
        //Debug.Log("Player Health: " +  currentHealth);
        Debug.Log("timer" + deathTimer);
    }
    public void GetHealthStats()
    {
        maxHealth = swapCharacter.currentCharacterStats.baseHealth;
        regenRate = swapCharacter.currentCharacterStats.baseRegen;
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
        //Debug.Log("you died!");
        swapCharacter.characters[swapCharacter.currentCharacterIndex].transform.Rotate(new Vector3(0, 5f, 0));
        
        if (deathTimer >= 3f)
        {
            currentHealth = maxHealth;
            SceneManager.LoadScene(0);
            swapCharacter.characters[swapCharacter.currentCharacterIndex].transform.rotation = Quaternion.identity;
            swapCharacter.characters[swapCharacter.currentCharacterIndex].transform.position = new Vector3(0, 1.4f, 0);
            deathTimer = 0;
        }
    }
    void Regen()
    {
        PlayerHeal(regenRate);
    }
}
