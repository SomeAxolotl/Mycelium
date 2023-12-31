using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    bool invincible;
    float regenRate;
    SwapCharacter swapCharacter;
    HUDHealth hudHealth;
    HUDItem hudItem;
    SwapWeapon swapWeapon;
    NutrientTracker nutrientTracker;
    PlayerController playerController;
    CamTracker camTracker;
    float deathTimer;


    // Start is called before the first frame update
    void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        swapWeapon = GetComponent<SwapWeapon>();
        GetHealthStats();
        currentHealth = maxHealth;
        InvokeRepeating("Regen", 1f, 1f);
        hudHealth = GameObject.Find("HUD").GetComponent<HUDHealth>();
        hudItem = GameObject.Find("HUD").GetComponent<HUDItem>();
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        playerController = GetComponent<PlayerController>();
        camTracker = GameObject.Find("CameraTracker").GetComponent<CamTracker>();
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
            if (camTracker.isLockedOn)
            {
                camTracker.ToggleLockOn();
            }
            Death();
        }
    }
    public void GetHealthStats()
    {
        maxHealth = swapCharacter.currentCharacterStats.baseHealth;
        regenRate = swapCharacter.currentCharacterStats.baseRegen;
    }
    public void PlayerTakeDamage(float dmgTaken)
    {
        if (!invincible)
        {
            currentHealth -= dmgTaken;
            hudHealth.UpdateHealthUI(currentHealth, maxHealth);
        }
    }
    public void PlayerHeal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
    }
    void Death()
    {
        swapCharacter.characters[swapCharacter.currentCharacterIndex].transform.Rotate(new Vector3(0, 5f, 0));
        playerController.DisableController();
        playerController.isInvincible = true;
        if (deathTimer >= 3f)
        {
            if(camTracker.isLockedOn)
            {
                camTracker.ToggleLockOn();
            }
            currentHealth = maxHealth;
            swapWeapon.curWeapon.tag = "Weapon";
            Instantiate(Resources.Load("Weapons/StartWeapon"), GameObject.FindWithTag("WeaponSlot").transform);
            swapWeapon.UpdateCharacter(GameObject.FindWithTag("currentPlayer"));
            GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
            foreach (GameObject weapon in weapons)
            Destroy(weapon);
            nutrientTracker.LoseMaterials();
            if(swapWeapon.curWeapon != null)
            {
                SceneManager.LoadScene(1);
            }
            deathTimer = 0;
            playerController.isInvincible = false;
            playerController.EnableController();
        }
    }
    void Regen()
    {
        PlayerHeal(regenRate);
    }

    public void ResetHealth()
    {
        GetHealthStats();
        currentHealth = maxHealth;
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
    }
    public void ActivateInvincibility()
    {
        invincible = true;
    }

    public void DeactivateInvincibility()
    {
        invincible = false;
    }
}
