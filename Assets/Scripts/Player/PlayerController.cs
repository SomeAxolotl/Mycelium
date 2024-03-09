using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using RonaldSunglassesEmoji.Interaction;

public class PlayerController : MonoBehaviour
{
    public float baseDodgeCooldown = 3f;
    [SerializeField] float dodgeCooldownIncrement = -0.15f;
    private float finalDodgeCooldown;

    private GameObject currentPlayer;
    public Rigidbody rb;
    public Vector3 forceDirection = Vector3.zero;
    [HideInInspector] public float moveSpeed;
    private float gravityForce = -25f;
    Vector3 gravity;
    [SerializeField] private Camera playerCamera;
    public bool looking = true;
    [HideInInspector] public Vector3 inputDirection;
    [HideInInspector] public Vector3 targetVelocity;
    public AnimationClip rollClip;
    private float clipLength;

    //Input fields
    [HideInInspector] public ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;
    private InputAction dodge;
    private InputAction stat_skill_1;
    private InputAction stat_skill_2;
    private InputAction subspecies_skill;
    private InputAction interact;
    private InputAction salvage;
    public bool canUseDodge = true;
    public bool canUseAttack = true;
    public bool canUseSkill = true;
    public bool canAct = true;
    public bool activeDodge = false;
    public bool isInvincible = false;
    SwapCharacter swapCharacter;
    PlayerAttack playerAttack;
    PlayerHealth playerHealth;
    SkillManager skillManager;

    private HUDSkills hudSkills;
    Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
        skillManager = GetComponent<SkillManager>();
        GetStats();
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        move = playerActionsAsset.Player.Move;
        dodge = playerActionsAsset.Player.Dodge;
        stat_skill_1 = playerActionsAsset.Player.Stat_Skill_1;
        stat_skill_2 = playerActionsAsset.Player.Stat_Skill_2;
        subspecies_skill = playerActionsAsset.Player.Subspecies_Skill;
        interact = playerActionsAsset.Player.Interact;
        salvage = playerActionsAsset.Player.Salvage;
        gravity = new Vector3(0f, gravityForce, 0f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        clipLength = rollClip.length / 6f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (dodge.triggered && canUseDodge && canAct)
        {
            if (!canUseAttack && animator.GetCurrentAnimatorStateInfo(0).IsName(playerAttack.attackAnimation))
            {
                StopCoroutine(playerAttack.attackstart);
                StopCoroutine(playerAttack.lunge);
                playerAttack.animator.Rebind();
                moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;
                playerAttack.curWeapon.GetComponent<Collider>().enabled = false;
            }
            StartCoroutine(Dodging());
        }

        if (subspecies_skill.triggered && canUseSkill == true)
        {
            GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
            Skill subskill = skillLoadout.transform.GetChild(0).gameObject.GetComponent<Skill>();
            if (subskill.canSkill)
            {
                subskill.ActivateSkill(0);
            }
        }

        if (stat_skill_1.triggered && canUseSkill == true)
        {
            GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
            Skill skill1 = skillLoadout.transform.GetChild(1).gameObject.GetComponent<Skill>();
            if (skill1.canSkill)
            {
                skill1.ActivateSkill(1);
            }
        }

        if (stat_skill_2.triggered && canUseSkill == true)
        {
            GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
            Skill skill2 = skillLoadout.transform.GetChild(2).gameObject.GetComponent<Skill>();
            if (skill2.canSkill)
            {
                skill2.ActivateSkill(2);
            }
        }

        if (interact.triggered && canAct)
        {
            SporeInteractableFinder sporeInteractableFinder = GameObject.FindWithTag("currentPlayer").GetComponent<SporeInteractableFinder>();
            GameObject closestInteractableObject = sporeInteractableFinder.closestInteractableObject;
            if (closestInteractableObject != null)
            {
                closestInteractableObject.GetComponent<IInteractable>().Interact(closestInteractableObject);
                sporeInteractableFinder.OnTriggerExit(closestInteractableObject.GetComponent<Collider>());
            }
        }

        if (salvage.triggered && canAct)
        {
            SporeInteractableFinder sporeInteractableFinder = GameObject.FindWithTag("currentPlayer").GetComponent<SporeInteractableFinder>();
            GameObject closestInteractableObject = sporeInteractableFinder.closestInteractableObject;
            if (closestInteractableObject != null)
            {
                closestInteractableObject.GetComponent<IInteractable>().Salvage(closestInteractableObject);
                sporeInteractableFinder.OnTriggerExit(closestInteractableObject.GetComponent<Collider>());
            }
        }
    }

    private void FixedUpdate()
    {
        LookAt();

        rb.AddForce(gravity, ForceMode.Acceleration);

        inputDirection = new Vector3(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y);
        targetVelocity = (GetCameraRight(playerCamera) * inputDirection.x + GetCameraForward(playerCamera) * inputDirection.z) * moveSpeed;
        targetVelocity.y = rb.velocity.y;
        if (!activeDodge)
        {
            rb.velocity = targetVelocity;
        }

        animator.SetBool("Walk", rb.velocity.magnitude > 0.01f && inputDirection.magnitude > 0f);

        if (inputDirection == Vector3.zero)
        {
            //Deceleration
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.x *= 0.8f;
            currentVelocity.z *= 0.8f;
            rb.velocity = currentVelocity;
        }
    }
    public void GetStats()
    {
        currentPlayer = GameObject.FindWithTag("currentPlayer");
        rb = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;
    }

    public void CalculateDodgeCooldown()
    {
        finalDodgeCooldown = baseDodgeCooldown + (GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>().speedLevel * dodgeCooldownIncrement);
    }
    
    IEnumerator Dodging()
    {
        CalculateDodgeCooldown();
        canUseDodge = false;
        canUseAttack = false;
        canUseSkill = false;
        activeDodge = true;
        isInvincible = true;
        looking = false;
        float storedAnimSpeed = swapCharacter.currentCharacterStats.animatorSpeed;
        animator.speed = 1.5f;
        animator.SetBool("Roll", true);
        animator.Play("Roll");
        Vector3 rollDirection = rb.transform.forward * 20f;
        float rollTimer = 0f;
        while (rollTimer < clipLength)
        {
            rollTimer += Time.deltaTime;
            rollDirection.y = rb.velocity.y;
            rb.velocity = rollDirection;
            yield return null;
        }
        ParticleManager.Instance.SpawnParticles("Dust", GameObject.FindWithTag("currentPlayer").transform.position, Quaternion.identity);
        SoundEffectManager.Instance.PlaySound("Stab", GameObject.FindWithTag("currentPlayer").transform.position);
        hudSkills.StartCooldownUI(4, (finalDodgeCooldown + clipLength));
        yield return new WaitForSeconds(clipLength);
        isInvincible = false;
        activeDodge = false;
        canUseAttack = true;
        canUseSkill = true;
        looking = true;
        animator.SetBool("Roll", false);
        animator.SetBool("Walk", true);
        animator.speed = storedAnimSpeed;
        yield return new WaitForSeconds(finalDodgeCooldown);
        canUseDodge = true;
    }
    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void LookAt()
    {
        if(looking)
        {
            if (rb.velocity.magnitude > 0.01f && inputDirection.magnitude > 0f)
            {
                Vector3 lookDirection = GetCameraForward(playerCamera) * inputDirection.z + GetCameraRight(playerCamera) * inputDirection.x;

                if (lookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 12f * Time.deltaTime));
                }
            }
        }
    }
    public void EnableController()
    {
        canAct = true;
        canUseAttack = true;
        canUseSkill = true;
        playerActionsAsset.Player.Enable();
    }

    public void DisableController()
    {
        canAct = false;
        canUseAttack = false;
        canUseSkill = false;
        playerActionsAsset.Player.Disable();
    }
    public void Knockback(GameObject obj, float knockbackForce)
    {
        isInvincible = true;
        DisableController();
        Vector3 dirFromobject = (new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(obj.transform.position.x, 0f, obj.transform.position.z)).normalized;
        StartCoroutine(StartKnockback(dirFromobject, knockbackForce));
    }
    IEnumerator StartKnockback(Vector3 direction, float force)
    {
        Vector3 knockbackForce = direction * force;
        knockbackForce += -Vector3.forward * 3f;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
        animator.SetBool("Hurt", true);
        animator.Play("Hurt");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .5f);
        animator.SetBool("Hurt", false);
        EnableController();
        isInvincible = false;
    }
    public void ApplyAttractionForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

}
