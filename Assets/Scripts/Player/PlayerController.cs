using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 forceDirection = Vector3.zero;
    public float moveSpeed;
    [SerializeField] float gravityForce = -20;
    Vector3 gravity;
    [SerializeField] private Camera playerCamera;
    public bool looking = true;

    //Input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;
    private InputAction dodge;
    private InputAction stat_skill_1;
    private InputAction stat_skill_2;
    private InputAction subspecies_skill;
    public bool canUseDodge = true;
    public bool canUseAttack = true;
    public bool canUseSkill = true;
    public bool canAct = true;
    public bool activeDodge = false;
    public bool isInvincible = false;
    bool playerSwapping;
    SwapCharacter swapCharacter;
    PlayerAttack playerAttack;
    PlayerHealth playerHealth;
    SkillManager skillManager;

    public float dodgeCooldown = 1f;
    public float dodgeIFrames = 0.15f;

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
        gravity = new Vector3(0f, gravityForce, 0f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
    }

    // Update is called once per frame
    private void Update()
    {
        SpeedControl();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.forward, Color.red);
        if (dodge.triggered && canUseDodge == true)
        {
            if (canUseAttack == false)
            {
                StopCoroutine(playerAttack.attackstart);
                StopCoroutine(playerAttack.lunge);
                playerAttack.animator.Rebind();
                moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;
                playerAttack.curWeapon.GetComponent<Collider>().enabled = false;
            }

            StartCoroutine(Dodging());
            StartCoroutine(IFrames());
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
    }

    private void FixedUpdate()
    {
        LookAt();
        
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * moveSpeed;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * moveSpeed;

        animator.SetBool("Walk", forceDirection.magnitude > 0f);
        
        rb.AddForce(forceDirection, ForceMode.Impulse);
        rb.AddForce(gravity, ForceMode.Acceleration);
        forceDirection = Vector3.zero;

        if (move.ReadValue<Vector2>() == Vector2.zero)
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
        rb = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;
    }
    IEnumerator Dodging()
    {
        canUseDodge = false;
        canUseAttack = false;
        canUseSkill = false;
        activeDodge = true;
        rb.AddForce(forceDirection * 6f, ForceMode.Impulse);
        //Dust Particle for Dodging
        ParticleManager.Instance.SpawnParticles("Dust", GameObject.FindWithTag("currentPlayer").transform.position, Quaternion.identity);
        //HUD Dodge Cooldown
        hudSkills.StartCooldownUI(4, dodgeCooldown);
        playerHealth.ActivateInvincibility();
        yield return new WaitForSeconds(.2f);
        playerHealth.DeactivateInvincibility();
        activeDodge = false;
        canUseDodge = true;
        canUseAttack = true;
        yield return new WaitForSeconds(dodgeCooldown);
        canUseSkill = true;
    }

    IEnumerator IFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(dodgeIFrames);
        isInvincible = false;
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
        if (looking)
        {
            Vector3 direction = rb.velocity;
            direction.y = 0f;

            if(move.ReadValue<Vector2>().sqrMagnitude != 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 15f * Time.deltaTime);
            }
            else
            {
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limits player velocity
        if(flatVel.magnitude > moveSpeed && activeDodge == false)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void EnableController()
    {
        canAct = true;
        canUseAttack = true;
        canUseDodge = true;
        canUseSkill = true;
        playerActionsAsset.Player.Enable();
    }

    public void DisableController()
    {
        canAct = false;
        canUseAttack = false;
        canUseDodge = false;
        canUseSkill = false;
        playerActionsAsset.Player.Disable();
    }
    public void Knockback(GameObject obj, float knockbackForce)
    {
        isInvincible = true;
        Vector3 dirFromobject = (new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(obj.transform.position.x, 0f, obj.transform.position.z)).normalized;
        StartCoroutine(StartKnockback(dirFromobject, knockbackForce));
    }
    IEnumerator StartKnockback(Vector3 direction, float force)
    {
        Vector3 knockbackForce = direction * force;
        knockbackForce += Vector3.up * 1.5f;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
        //DisableController();
        yield return new WaitForSeconds(.15f);
        //EnableController();
        isInvincible = false;
    }
}
