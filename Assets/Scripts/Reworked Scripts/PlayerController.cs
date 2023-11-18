using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    private Vector3 forceDirection = Vector3.zero;
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
    bool canDodge = true;
    bool activeDodge = false;
    public bool isInvincible = false;
    bool playerSwapping;
    SwapCharacter swapCharacter;
    NewPlayerAttack newPlayerAttack;
    NewPlayerHealth newPlayerHealth;
    SkillManager skillManager;
    public bool canAct = true;

    public float dodgeCooldown = 1f;
    public float dodgeIFrames = 0.15f;

    private HUDSkills hudSkills;

    // Start is called before the first frame update
    private void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        newPlayerAttack = GetComponent<NewPlayerAttack>();
        newPlayerHealth = GetComponent<NewPlayerHealth>();
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

        //HUDSkills Reference
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
    }

    // Update is called once per frame
    private void Update()
    {
        SpeedControl();
        //Debug.Log("velocity: " + rb.velocity);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(dodge.triggered && canDodge == true)
        {
            StartCoroutine("Dodging");
            StartCoroutine("IFrames");
        }
<<<<<<< Updated upstream
        
        if (newPlayerAttack.attacking == true || newPlayerHealth.currentHealth <= 0)
        {
            playerActionsAsset.Player.Disable();
        }
        else
        {
            playerActionsAsset.Player.Enable();
        }
=======
>>>>>>> Stashed changes

        if (subspecies_skill.triggered && canAct == true)
        {
            GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
            Skill subskill = skillLoadout.transform.GetChild(0).gameObject.GetComponent<Skill>();
            if (subskill.canSkill)
            {
                subskill.ActivateSkill(0);
            }
        }

        if (stat_skill_1.triggered && canAct == true)
        {
            GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
            Skill skill1 = skillLoadout.transform.GetChild(1).gameObject.GetComponent<Skill>();
            if (skill1.canSkill)
            {
                skill1.ActivateSkill(1);
            }
        }

        if (stat_skill_2.triggered && canAct == true)
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
        moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;
    }
    IEnumerator Dodging()
    {
        canDodge = false;
        activeDodge = true;
        rb.AddForce(transform.forward * 3f, ForceMode.Impulse);
        //Dust Particle for Dodging
        ParticleManager.Instance.SpawnParticles("Dust", GameObject.FindWithTag("currentPlayer").transform.position, Quaternion.identity);
        //HUD Dodge Cooldown
        hudSkills.StartCooldownUI(4, dodgeCooldown);
        yield return new WaitForSeconds(.15f);
        activeDodge = false;
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
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
        playerActionsAsset.Player.Enable();
    }

    public void DisableController()
    {
        playerActionsAsset.Player.Disable();
    }
}
