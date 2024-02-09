using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    float regenRate;
    public bool isDefending;
    float realDmgTaken;
    SwapCharacter swapCharacter;
    HUDHealth hudHealth;
    HUDItem hudItem;
    SwapWeapon swapWeapon;
    NutrientTracker nutrientTracker;
    PlayerController playerController;
    [HideInInspector] public WeaponCollision weaponCollision;
    CamTracker camTracker;
    float deathTimer;
    private Animator animator;
    private SceneLoader sceneLoaderScript;

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
        animator = GameObject.Find("Spore").GetComponent<Animator>();
        weaponCollision = GameObject.FindWithTag("currentWeapon").GetComponent<WeaponCollision>();
        isDefending = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, -100, maxHealth);

        if (currentHealth <= 0)
        {
            playerController.DisableController();
            playerController.isInvincible = true;
            swapWeapon.curWeapon.GetComponent<Collider>().enabled = false;
            CancelInvoke("Regen");
            hudHealth.UpdateHealthUI(0, maxHealth);
            deathTimer += Time.deltaTime;
            if (camTracker.isLockedOn)
            {
                camTracker.ToggleLockOn();
            }
            if (animator.GetBool("Death") == false)
            {
                animator.SetBool("Death", true);
            }
            if (deathTimer >= 3f)
            {
                Debug.Log("BUILD INDEX: " + SceneManager.GetActiveScene().buildIndex);
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    PlayerPrefs.SetInt("IsTutorialFinished", Convert.ToInt32(true));
                }
                sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
                sceneLoaderScript.BeginLoadScene(2, false);
                StartCoroutine(Death());
            }
        }
    }
    public void GetHealthStats()
    {
        maxHealth = swapCharacter.currentCharacterStats.baseHealth;
        regenRate = swapCharacter.currentCharacterStats.baseRegen;
    }
    public void PlayerTakeDamage(float dmgTaken)
    {
        animator = GetComponentInChildren<Animator>();
        if(animator.GetBool("Hurt") == true)
        {
            Debug.Log("that hurt is true yo");
            GameObject.FindWithTag("currentWeapon").GetComponent<Collider>().enabled = false;
        }
        if(isDefending)
        {
            realDmgTaken = dmgTaken/2f;
            weaponCollision.reflectBonusDamage += realDmgTaken;
        }
        else
        {
            realDmgTaken = dmgTaken;
        }
        currentHealth -= realDmgTaken;
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
    }
    public void PlayerHeal(float healAmount)
    {
        animator = GetComponentInChildren<Animator>();
        currentHealth += healAmount;
        if (animator.GetBool("Hurt") == true)
        {
            animator.SetBool("Hurt", false);
            Debug.Log("no more hurty");
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
    }
    IEnumerator Death()
    {
        deathTimer = 0;
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(2).isLoaded);
        currentHealth = maxHealth;
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
        animator.Rebind();
        animator.SetBool("Death", false);
        InvokeRepeating("Regen", 1f, 1f);
        swapWeapon.curWeapon.tag = "Weapon";
        Instantiate(Resources.Load("Weapons/StartWeapon"), GameObject.FindWithTag("WeaponSlot").transform);
        swapWeapon.UpdateCharacter(GameObject.FindWithTag("currentPlayer"));
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject weapon in weapons)
        Destroy(weapon);
        nutrientTracker.LoseMaterials();
        playerController.isInvincible = false;
        playerController.EnableController();
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
}
