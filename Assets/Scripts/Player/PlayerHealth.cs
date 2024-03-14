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
    CamTracker camTracker;
    public float deathTimer;
    private Animator animator;
    private SceneLoader sceneLoaderScript;
    private ProfileManager profileManagerScript;

    [SerializeField] private float timeBetweenHurtSounds = 0.25f;

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
        profileManagerScript = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
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
                    profileManagerScript.tutorialIsDone = true;
                }
                profileManagerScript.Save();
                sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
                //GameManager.Instance.OnPlayerDeath();
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
        StartCoroutine(HurtSound());

        animator = GetComponentInChildren<Animator>();
        if(animator.GetBool("Hurt") == true)
        {
            Debug.Log("that hurt is true yo");
            GameObject.FindWithTag("currentWeapon").GetComponent<Collider>().enabled = false;
        }
        if(isDefending)
        {
            realDmgTaken = dmgTaken/2f;
            GameObject.FindWithTag("currentWeapon").GetComponent<WeaponCollision>().reflectBonusDamage += realDmgTaken;
        }
        else
        {
            realDmgTaken = dmgTaken;
        }
        currentHealth -= realDmgTaken;
        UpdateHudHealthUI();
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
        UpdateHudHealthUI();
    }
    IEnumerator Death()
    {
        deathTimer = -5; //changed to a negative number beacuse dying was happening twice -ryan
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(2).isLoaded);
        currentHealth = maxHealth;
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
        animator.Rebind();
        animator.SetBool("Death", false);
        InvokeRepeating("Regen", 1f, 1f);
        swapWeapon.curWeapon.tag = "Weapon";
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
        UpdateHudHealthUI();
    }

    IEnumerator HurtSound()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");

        SoundEffectManager.Instance.PlaySound("Damaged", player.transform.position);

        yield return new WaitForSeconds(timeBetweenHurtSounds);

        SoundEffectManager.Instance.PlaySound("Hurt", player.transform.position);
    }

    public void UpdateHudHealthUI()
    {
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
    }
}
