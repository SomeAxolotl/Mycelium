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
    private bool dead = false;
    private float realDmgTaken;
    [HideInInspector] public float deathTimer;
    SwapCharacter swapCharacter;
    HUDHealth hudHealth;
    HUDItem hudItem;
    SwapWeapon swapWeapon;
    NutrientTracker nutrientTracker;
    PlayerController playerController;
    Animator animator;
    SceneLoader sceneLoaderScript;
    ProfileManager profileManagerScript;

    [SerializeField] private float timeBetweenHurtSounds = 0.25f;
    [SerializeField] private CutscenePlayer cutscenePlayer;

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
        animator = GameObject.Find("Spore").GetComponent<Animator>();
        profileManagerScript = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
        isDefending = false;
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
    public void PlayerTakeDamage(float dmgTaken)
    {
        if (currentHealth > 0)
        {
            StartCoroutine(HurtSound());

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
            UpdateHudHealthUI();
        }
        if (currentHealth <= 0 && !dead)
        {
            StartCoroutine(Death());
        }
    }
    public void PlayerHeal(float healAmount)
    {
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
    IEnumerator Death()
    {
        //Notification stuff
        string heldMaterial = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>().GetCurrentHeldMaterial();
        string deathMessage = "<color=#8B0000>YOU DIED</color>";
        if (heldMaterial != "")
        {
            NotificationManager.Instance.Notification(deathMessage, heldMaterial + " dropped!", null, heldMaterial);
        }
        else
        {
            NotificationManager.Instance.Notification(deathMessage);
        }

        //Setup and Animation stuff
        dead = true;
        swapWeapon.curWeapon.GetComponent<Collider>().enabled = false;
        CancelInvoke("Regen");
        hudHealth.UpdateHealthUI(0, maxHealth);
        if (animator.GetBool("Death") == false)
        {
            animator.SetBool("Death", true);
        }

        //Wait a bit
        yield return new WaitForSeconds(3f);

        //See what scene we are in
        if (SceneManager.GetActiveScene().name == "New Tutorial")
        {
            profileManagerScript.tutorialIsDone[GlobalData.profileNumber] = true;

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
        sceneLoaderScript.BeginLoadScene("The Carcass", false);

        //Wait until the scene finishes loading and then do some final stuff
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(2).isLoaded);
        currentHealth = maxHealth;
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
        swapWeapon.curWeapon.tag = "Weapon";
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

        SoundEffectManager.Instance.PlaySound("Damaged", player.transform.position);

        yield return new WaitForSeconds(timeBetweenHurtSounds);

        SoundEffectManager.Instance.PlaySound("Hurt", player.transform.position);
    }

    public void UpdateHudHealthUI()
    {
        hudHealth.UpdateHealthUI(currentHealth, maxHealth);
    }
}
