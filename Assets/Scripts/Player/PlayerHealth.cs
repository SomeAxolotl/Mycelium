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
    CamTracker camTracker;
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
        camTracker = GameObject.Find("CameraTracker").GetComponent<CamTracker>();
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
        if (currentHealth > 0 && !playerController.isInvincible)
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
        dead = true;
        swapWeapon.curWeapon.GetComponent<Collider>().enabled = false;
        CancelInvoke("Regen");
        hudHealth.UpdateHealthUI(0, maxHealth);
        if (camTracker.isLockedOn)
        {
            camTracker.ToggleLockOn();
        }
        if (animator.GetBool("Death") == false)
        {
            animator.SetBool("Death", true);
        }
        yield return new WaitForSeconds(3f);
        Debug.Log("BUILD INDEX: " + SceneManager.GetActiveScene().buildIndex);
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            profileManagerScript.tutorialIsDone = true;

            cutscenePlayer.StartCutscene();

            //Lobotomize the Giga Beetle
            foreach (Component component in GameObject.Find("Giga Beetle").GetComponents<Component>())
            {
                if (component.GetType() != typeof(Transform) && component.GetType() != typeof(MeshRenderer) && component.GetType() != typeof(MeshFilter))
                {
                    Destroy(component);
                }
            }

            while (cutscenePlayer.isFinished == false)
            {
                yield return null;
            }
        }
        profileManagerScript.Save();
        sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        sceneLoaderScript.BeginLoadScene(2, false);
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
