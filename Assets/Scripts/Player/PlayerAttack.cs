using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerAttack : MonoBehaviour
{
    public GameObject curWeapon;
    [HideInInspector] public ThirdPersonActionsAsset playerActionsAsset;
    private InputAction attack;
    private SwapCharacter swapCharacter;
    public float dmgDealt;
    private float fungalMightBonus = 1f;
    private float lungeDuration = 0.4f;
    [SerializeField] private float lungeForce = 20f;
    [SerializeField][Tooltip("Is multiplied by the attack animation speed")] private float lungeDurationScalar = 0.25f;
    private HUDSkills hudSkills;
    [HideInInspector] public Animator animator;
    GameObject player;
    PlayerController playerController;

    [HideInInspector] public string attackAnimation;

    public IEnumerator attackstart;

    private float windupMoveSpeed = 1f;
    public GameObject rangedEnemyProjectilePrefab;
    public Material negativeVelocityMaterial;
    [SerializeField] private string stickbugSpearName = "Stickbug Spear";

    private bool isLunging = false;
    private float lungeStartTime;
    private Vector3 lungeDirection;

    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapCharacter = GetComponent<SwapCharacter>();
        attack = playerActionsAsset.Player.Attack;
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("currentPlayer");
        playerController = GetComponent<PlayerController>();
        attackstart = this.Attack(curWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        if (attack.triggered && playerController.canUseAttack && !animator.GetBool("Hurt") && !animator.GetBool("Death"))
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        if (GameObject.FindWithTag("currentWeapon") != null)
        {
            curWeapon = GameObject.FindWithTag("currentWeapon");
            WeaponCollision weaponCollision = curWeapon.GetComponent<WeaponCollision>();
            weaponCollision.ClearEnemyList();
            player = GameObject.FindWithTag("currentPlayer");
            dmgDealt = (swapCharacter.currentCharacterStats.primalDmg) * fungalMightBonus * curWeapon.GetComponent<WeaponStats>().statNums.advDamage.MultValue; //took out base weapon for now
            animator = GetComponentInChildren<Animator>();
            StartCoroutine(Attack(curWeapon));
            StartLunge();
        }
        else
        {
            return;
        }
    }

    public Action StartedAttack;
    public Action FinishedAttack;
    private IEnumerator Attack(GameObject curWeapon)
    {
        StartedAttack?.Invoke();
        playerController.canUseSkill = false;
        playerController.canUseAttack = false;
        playerController.moveSpeed = windupMoveSpeed;
        float originalAnimatorSpeed = animator.speed;
        animator.speed *= curWeapon.GetComponent<WeaponStats>().wpnAttackSpeedModifier;
        if (curWeapon.GetComponent<WeaponStats>().weaponType == WeaponStats.WeaponTypes.Slash)
        {
            animator.Play("Slash");
            attackAnimation = "Slash";
        }
        if (curWeapon.GetComponent<WeaponStats>().weaponType == WeaponStats.WeaponTypes.Smash)
        {
            animator.Play("Smash");
            attackAnimation = "Smash";
        }
        if (curWeapon.GetComponent<WeaponStats>().weaponType == WeaponStats.WeaponTypes.Stab)
        {
            animator.Play("Stab");
            attackAnimation = "Stab";
        }
        yield return null;

        float currentAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        lungeDuration = (animator.GetCurrentAnimatorStateInfo(0).length * animator.speed) * lungeDurationScalar;
        float percentUntilWindupDone = curWeapon.GetComponent<WeaponStats>().percentUntilWindupDone;
        float percentUntilSwingDone = curWeapon.GetComponent<WeaponStats>().percentUntilSwingDone;
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilWindupDone);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilWindupDone)
        {
            curWeapon.GetComponent<Collider>().enabled = true;
            playerController.playerActionsAsset.Player.Disable();

            // Check if the weapon is the Stickbug Spear before instantiating the projectile
            if (curWeapon.GetComponent<WeaponStats>().weaponType == WeaponStats.WeaponTypes.Stab &&
                curWeapon.name == stickbugSpearName)
            {
                InstantiateRangedProjectile();
            }
        }
        playerController.moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;

        SpeedChange speedChange = animator.GetComponent<SpeedChange>();
        if (speedChange != null)
        {
            speedChange.SpeedUpdate();
        }

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilSwingDone);
        curWeapon.GetComponent<Collider>().enabled = false;
        ClearAllFungalMights();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f);
        animator.Rebind();
        playerController.EnableController();
        animator.speed = originalAnimatorSpeed;
        FinishedAttack?.Invoke();
    }

    private void StartLunge()
    {
        isLunging = true;
        lungeStartTime = Time.time;
        lungeDirection = player.transform.forward;
    }

    private void FixedUpdate()
    {
        if (isLunging)
        {
            float elapsedTime = Time.time - lungeStartTime;
            if (elapsedTime < lungeDuration)
            {
                float forcePerSecond = lungeForce / lungeDuration;
                player.GetComponent<Rigidbody>().AddForce(lungeDirection * forcePerSecond * Time.fixedDeltaTime, ForceMode.Impulse);
            }
            else
            {
                isLunging = false;
            }
        }
    }

    //Fungal Might for Attacking
    public void ActivateFungalMight(float fungalMightValue)
    {
        fungalMightBonus = fungalMightValue;
    }
    public void DeactivateFungalMight()
    {
        fungalMightBonus = 1f;
    }

    //Clears Fungal Might for attacking and skills
    public void ClearAllFungalMights()
    {
        GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
        foreach (Transform child in skillLoadout.transform)
        {
            Skill skill = child.gameObject.GetComponent<Skill>();
            skill.DeactivateFungalMight();
        }

        DeactivateFungalMight();

        GameObject[] fungalMightParticles = GameObject.FindGameObjectsWithTag("FungalMightParticles");
        foreach (GameObject particle in fungalMightParticles)
        {
            particle.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void EnableAttack()
    {
        playerActionsAsset.Enable();
    }
    public void DisableAttack()
    {
        playerActionsAsset.Disable();
    }

    private void OnDisable()
    {
        if (playerActionsAsset != null)
        {
            playerActionsAsset.Disable();
        }
    }

    private void InstantiateRangedProjectile()
    {
        Vector3 spawnPosition = player.transform.position + player.transform.forward * 4f + player.transform.up * 1f + player.transform.right * 1f; // Adjust the forward, upward, and right offsets as needed
        GameObject projectile = Instantiate(rangedEnemyProjectilePrefab, spawnPosition, Quaternion.identity);

        // Set the projectile's tag and material
        projectile.tag = "ReversedProjectile";
        Renderer renderer = projectile.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = negativeVelocityMaterial;
        }

        // Apply initial velocity or any other necessary settings for the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = player.transform.forward * 10f; // Adjust the projectile speed as needed
        }
    }
}
