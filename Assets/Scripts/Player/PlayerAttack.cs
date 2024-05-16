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
    public IEnumerator lunge;

    private float windupMoveSpeed = 1f;

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
        lunge = this.Lunge();
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
        if(GameObject.FindWithTag("currentWeapon") != null)
        {
            curWeapon = GameObject.FindWithTag("currentWeapon");
            WeaponCollision weaponCollision = curWeapon.GetComponent<WeaponCollision>();
            weaponCollision.ClearEnemyList();
            player = GameObject.FindWithTag("currentPlayer");
            dmgDealt = (swapCharacter.currentCharacterStats.primalDmg + curWeapon.GetComponent<WeaponStats>().wpnBaseDmg) * fungalMightBonus * curWeapon.GetComponent<WeaponStats>().wpnMult;
            animator = GetComponentInChildren<Animator>();
            StartCoroutine(Attack(curWeapon));
            StartCoroutine(Lunge());
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
        //hudSkills.StartCooldownUI(3, currentAnimationLength);

        lungeDuration = (animator.GetCurrentAnimatorStateInfo(0).length * animator.speed) * lungeDurationScalar;
        float percentUntilWindupDone = curWeapon.GetComponent<WeaponStats>().percentUntilWindupDone;
        float percentUntilSwingDone = curWeapon.GetComponent<WeaponStats>().percentUntilSwingDone;
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil (() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilWindupDone);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilWindupDone)
        {
            curWeapon.GetComponent<Collider>().enabled = true;
            playerController.playerActionsAsset.Player.Disable();
        }
        playerController.moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;
        yield return new WaitUntil (() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilSwingDone);
        curWeapon.GetComponent<Collider>().enabled = false;
        ClearAllFungalMights();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f);
        animator.Rebind();
        playerController.EnableController();
        animator.speed = originalAnimatorSpeed;
        FinishedAttack?.Invoke();
    }
    private IEnumerator Lunge()
    {
        float percentUntilWindupDone = curWeapon.GetComponent<WeaponStats>().percentUntilWindupDone;
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > percentUntilWindupDone);
        Vector3 lungeDirection = player.transform.forward;
        float forcePerSecond = lungeForce / lungeDuration;
        float elapsedTime = 0f;
        while (elapsedTime < lungeDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            player.GetComponent<Rigidbody>().AddForce(lungeDirection * forcePerSecond * Time.deltaTime, ForceMode.Impulse);
            yield return new WaitForFixedUpdate();
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
}
