using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    private float regenRate;
    public bool isDefending;
    public bool isInvincible;
    private bool dead = false;
    private float realDmgTaken;
    [HideInInspector] public float deathTimer;
    SwapCharacter swapCharacter;
    HUDHealth hudHealth;
    HUDItem hudItem;
    SwapWeapon swapWeapon;
    NutrientTracker nutrientTracker;
    PlayerController playerController;
    [HideInInspector] public Animator animator;
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
        InvokeRepeating("Regen", 1f, 1f);
        hudHealth = GameObject.Find("HUD").GetComponent<HUDHealth>();
        hudItem = GameObject.Find("HUD").GetComponent<HUDItem>();
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        playerController = GetComponent<PlayerController>();
        animator = GameObject.Find("Spore").GetComponent<Animator>();
        profileManagerScript = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
        isDefending = false;
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(dead)
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
        if(isInvincible == true)
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

            animator = GetComponentInChildren<Animator>();
            if(animator.GetBool("Hurt") == true){
                Debug.Log("that hurt is true yo");
                GameObject.FindWithTag("currentWeapon").GetComponent<Collider>().enabled = false;
            }
            if(isDefending){
                realDmgTaken = dmgTaken / 2f;
                GameObject.FindWithTag("currentWeapon").GetComponent<WeaponCollision>().reflectBonusDamage += realDmgTaken;
            }else{
                realDmgTaken = dmgTaken;
            }
            StartCoroutine(AnimateHealthChange(currentHealth - realDmgTaken));
            UpdateHudHealthUI();
        }
        
    }
    IEnumerator AnimateHealthChange(float targetHealth)
    {
        float duration = 0.5f; // Duration of the animation in seconds
        float elapsed = 0f;
        float startingHealth = currentHealth;
        targetHealth = Mathf.Max(targetHealth, 0); // Ensure target health is not below 0
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentHealth = Mathf.Lerp(startingHealth, targetHealth, elapsed / duration);
            UpdateHudHealthUI();
            yield return null;
        }
        currentHealth = targetHealth;
        UpdateHudHealthUI();
        if (currentHealth <= 0 && !dead)
        {
            StartCoroutine(Death());
        }
    }
    public void PlayerHeal(float healAmount)
    {
        if (dead)
        {
            return; // Do not allow healing if the player is dead
        }
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
   
        UpdateHudHealthUI();
    }
    public Action Died;
    IEnumerator Death()
    {
        Died?.Invoke();

        GlobalData.isAbleToPause = false;
        GlobalData.currentLoop = 1;
        

        //Notification stuff
        string heldMaterial = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>().GetCurrentHeldMaterial();
        DesignTracker designTracker = GameObject.FindWithTag("currentPlayer").GetComponent<DesignTracker>();
        CharacterStats characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
        //string coloredSporeName = "<color=#" + ColorUtility.ToHtmlStringRGB(designTracker.bodyColor) + ">"+characterStats.sporeName+"</color>";
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
        else if(profileManagerScript.permadeathIsOn[GlobalData.profileNumber] == true)
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
        sceneLoaderScript.BeginLoadScene("The Carcass", false, diedInTutorial);

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

        SoundEffectManager.Instance.PlaySound("Damaged", player.transform);

        yield return new WaitForSeconds(timeBetweenHurtSounds);

        SoundEffectManager.Instance.PlaySound("Hurt", player.transform);
    }

    public void UpdateHudHealthUI()
    {
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
    }

    [SerializeField] private GameObject healObj;
    public void SpawnHealingOrb(Vector3 location, float healAmount = 1){
        GameObject healing = Instantiate(healObj, location, Quaternion.identity);
        healing.GetComponent<HealerScript>().O_healAmount = healAmount;
    }
}
