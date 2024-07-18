using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float regenRate;
    public bool isDefending;
    public bool isInvincible;
    private bool dead = false;
    private float realDmgTaken;
    private float healingReduction = 0f; // Healing reduction percentage
    private bool canRegen = true; // New flag to control regeneration
    private bool isRegenDisabled = false; // New flag to track if regen is disabled due to disease collider

    [HideInInspector] public float deathTimer;
    SwapCharacter swapCharacter;
    HUDHealth hudHealth;
    HUDItem hudItem;
    SwapWeapon swapWeapon;
    NutrientTracker nutrientTracker;
    PlayerController playerController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public DesignTracker designTracker;
    SceneLoader sceneLoaderScript;
    ProfileManager profileManagerScript;

    [SerializeField] private float timeBetweenHurtSounds = 0.25f;
    [SerializeField] private CutscenePlayer cutscenePlayer;

    void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        swapWeapon = GetComponent<SwapWeapon>();
        GetHealthStats();
        currentHealth = maxHealth;
        displayHealth = currentHealth;
        InvokeRepeating("Regen", 1f, 1f);
        hudHealth = GameObject.Find("HUD").GetComponent<HUDHealth>();
        hudItem = GameObject.Find("HUD").GetComponent<HUDItem>();
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        playerController = GetComponent<PlayerController>();
        animator = GameObject.Find("Spore").GetComponent<Animator>();
        designTracker = GameObject.FindWithTag("currentPlayer").GetComponent<DesignTracker>();
        profileManagerScript = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
        isDefending = false;
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            playerController.DisableController();
            playerController.isInvincible = true;
        }
    }

    public void GetHealthStats()
    {
        maxHealth = swapCharacter.currentCharacterStats.baseHealth;
        regenRate = swapCharacter.currentCharacterStats.baseRegen;
    }

    public Action<float> TakeDamage;
    public float dmgTaken;
    public void PlayerTakeDamage(float damage)
    {
        if (isInvincible == true)
        {
            return;
        }
        //Save current damage taken
        dmgTaken = damage;
        //Call action to modify damage
        TakeDamage?.Invoke(dmgTaken);

        if (currentHealth > 0)
        {
            StartCoroutine(HurtSound());
            designTracker.StartCoroutine(designTracker.Blink());

            animator = GetComponentInChildren<Animator>();
            if (animator.GetBool("Hurt") == true)
            {
                Debug.Log("that hurt is true yo");
                GameObject.FindWithTag("currentWeapon").GetComponent<Collider>().enabled = false;
            }
            if (isDefending)
            {
                realDmgTaken = dmgTaken / 2f;
                GameObject.FindWithTag("currentWeapon").GetComponent<WeaponCollision>().reflectBonusDamage += realDmgTaken;
            }
            else
            {
                realDmgTaken = dmgTaken;
            }
            currentHealth -= realDmgTaken;
            AnimateHealth(-realDmgTaken);
            // Check for Blinding attribute
            Blinding blindingAttribute = GetComponent<Blinding>();
            if (blindingAttribute != null)
            {
                blindingAttribute.ApplyBlindingEffect();
            }
            if (currentHealth <= 0 && !dead)
            {
                StartCoroutine(Death());
            }
        }

    }
    private void AnimateHealth(float healthChangeAmount)
    {
        if (healthChange == null)
        {
            healthChange = AnimateHealthChange(healthChangeAmount);
            StartCoroutine(healthChange);
        }
    }
    private IEnumerator healthChange;
    [SerializeField] private float displayHealth;
    IEnumerator AnimateHealthChange(float healthChangeAmount)
    {
        float fillSpeed = 8f; //How fast the bar fills up 
        displayHealth = Mathf.Clamp(currentHealth - healthChangeAmount, 0, maxHealth); //Finds the health before this happened

        while (displayHealth != currentHealth)
        {
            displayHealth = Mathf.Lerp(displayHealth, currentHealth, Time.deltaTime * fillSpeed);
            displayHealth = Mathf.Clamp(displayHealth, 0, maxHealth);
           // if (Mathf.Abs(displayHealth - currentHealth) < 0.1f) { displayHealth = currentHealth; } //If the display amount is super low just set it to 0
            UpdateHudHealthUI();
            yield return null;
        }
        UpdateHudHealthUI();
        healthChange = null;
    }

    public void PlayerHeal(float healAmount)
    {
        if (dead || currentHealth <= 0)
        {
            return; // Do not allow healing if the player is dead, player is only "dead" after the animation starts
        }
        healAmount = healAmount * (1 - healingReduction); // Apply healing reduction
        animator = GetComponentInChildren<Animator>();
        currentHealth += healAmount;

        if (animator.GetBool("Hurt") == true)
        {
            animator.SetBool("Hurt", false);
            //Debug.Log("no more hurty");
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        AnimateHealth(healAmount);
    }
    public Action Died;
    IEnumerator Death()
    {
        Died?.Invoke();

        GlobalData.isAbleToPause = false;

        //Notification stuff
        string heldMaterial = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>().GetCurrentHeldMaterial();
        CharacterStats characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        string coloredSporeName = characterStats.GetColoredSporeName();
        string deathMessage = "<color=#8B0000>YOU DIED</color>";
        string permaDeathMessage = "<color=#8B0000> has died for good.</color>";
        if (profileManagerScript.permadeathIsOn[GlobalData.profileNumber] == true)
        {
            NotificationManager.Instance.Notification(coloredSporeName + permaDeathMessage);
            GlobalData.sporePermaDied = characterStats.sporeName;
        }
        else if (heldMaterial != "") //&& SceneManager.GetActiveScene().name != "New Tutorial"
        {
            NotificationManager.Instance.Notification(deathMessage, heldMaterial + " dropped!", null, heldMaterial);
            if (SceneManager.GetActiveScene().name != "New Tutorial")
            {
                GlobalData.sporeDied = characterStats.sporeName;
            }
        }
        else
        {
            NotificationManager.Instance.Notification(deathMessage);
            if (SceneManager.GetActiveScene().name != "New Tutorial")
            {
                GlobalData.sporeDied = characterStats.sporeName;
            }
        }

        //Setup and Animation stuff
        dead = true;
        swapWeapon.O_curWeapon.GetComponent<Collider>().enabled = false;
        CancelInvoke("Regen");
        hudHealth.UpdateHealthUI(0, maxHealth);
        animator = GetComponentInChildren<Animator>();

        designTracker.canBlink = false;
        designTracker.CloseEyes();

        //thought rebinding might help with standing up bug. it still happened tho
        animator.Rebind();
        animator.SetTrigger("Death");

        //Wait a bit
        yield return new WaitForSeconds(3f);

        //See what scene we are in
        bool diedInTutorial = false;
        if (SceneManager.GetActiveScene().name == "New Tutorial")
        {
            profileManagerScript.tutorialIsDone[GlobalData.profileNumber] = true;
            diedInTutorial = true;
            cutscenePlayer.StartCutscene();

            //Lobotomize the Giga Beetle
            if (GameObject.Find("Giga Beetle") != null)
            {
                foreach (Component component in GameObject.Find("Giga Beetle").GetComponents<Component>())
                {
                    if (component.GetType() != typeof(Transform) && component.GetType() != typeof(MeshRenderer) && component.GetType() != typeof(MeshFilter))
                    {
                        Destroy(component);
                    }
                }
            }

            //Wait until the cutscene finishes
            while (cutscenePlayer.isFinished == false)
            {
                yield return null;
            }
        }
        else if (profileManagerScript.permadeathIsOn[GlobalData.profileNumber] == true)
        {
            //If we died we must be in one of the levels
            LoadCurrentPlayer.Instance.DeleteCurrentPlayerSpore();

            //wait one frame
            yield return null;
        }

        //Save Profile Stuff
        profileManagerScript.Save();

        //Start loading the next scene
        sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        if (SceneManager.GetActiveScene().name == "Playground")
        {
            sceneLoaderScript.BeginLoadScene("Playground", false);
        }
        else
        {
            sceneLoaderScript.BeginLoadScene("The Carcass", false, diedInTutorial);
        }

        //Wait until the scene finishes loading and then do some final stuff
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(2).isLoaded);
        currentHealth = maxHealth;
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
        swapWeapon.O_curWeapon.tag = "Weapon";
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject weapon in weapons)
            Destroy(weapon);
        nutrientTracker.LoseMaterials();
    }
    void Regen()
    {
        if (!canRegen)
        {
            return;
        }
        else if (canRegen)
        {
            PlayerHeal(regenRate);
        }
    }

    public void EnableRegen()
    {
        canRegen = true;
        isRegenDisabled = false;
        hudHealth.UpdateHealthNumberText(currentHealth, maxHealth, hudHealth.originalHealthNumberColor); // Fade back to the original color
    }

    public void DisableRegen()
    {
        canRegen = false;
        isRegenDisabled = true;
        hudHealth.UpdateHealthNumberText(currentHealth, maxHealth, Color.red); // Fade to red
    }


    public void ResetHealth()
    {
        GetHealthStats();
        currentHealth = maxHealth;
        displayHealth = currentHealth;
        UpdateHudHealthUI();
    }

    IEnumerator HurtSound()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");

        SoundEffectManager.Instance.PlaySound("Damaged", player.transform);

        yield return new WaitForSeconds(timeBetweenHurtSounds);

        SoundEffectManager.Instance.PlaySound("Hurt", player.transform);
    }

    public void SetHealingReduction(float reduction)
    {
        healingReduction = Mathf.Clamp(reduction, 0f, 1f); // Ensure reduction is between 0 and 1
        Debug.Log("Healing reduction set to: " + (healingReduction * 100) + "%");
    }
    public void UpdateHudHealthUI()
    {
        Color currentHealthColor = isRegenDisabled ? Color.red : hudHealth.originalHealthNumberColor;
        hudHealth.UpdateHealthUI(displayHealth, maxHealth); // Update health bar and health number text
        hudHealth.UpdateHealthNumberText(displayHealth, maxHealth, currentHealthColor);
    }

    [SerializeField] private GameObject healObj;
    public void SpawnHealingOrb(Vector3 location, float healAmount = 1)
    {
        GameObject healing = Instantiate(healObj, location, Quaternion.identity);
        healing.GetComponent<HealerScript>().O_healAmount = healAmount;
    }
}
