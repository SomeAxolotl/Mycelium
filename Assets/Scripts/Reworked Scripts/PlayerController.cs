using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    private Vector3 forceDirection = Vector3.zero;
    public float moveSpeed;
    Vector3 gravity;
    [SerializeField] private Camera playerCamera;

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
    // Start is called before the first frame update
    private void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        newPlayerAttack = GetComponent<NewPlayerAttack>();
        newPlayerHealth = GetComponent<NewPlayerHealth>();
        GetStats();
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        move = playerActionsAsset.Player.Move;
        dodge = playerActionsAsset.Player.Dodge;
        stat_skill_1 = playerActionsAsset.Player.Stat_Skill_1;
        stat_skill_2 = playerActionsAsset.Player.Stat_Skill_2;
        subspecies_skill = playerActionsAsset.Player.Subspecies_Skill;
        gravity = new Vector3(0f, -20f, 0f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        SpeedControl();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(dodge.triggered && canDodge == true)
        {
            StartCoroutine("Dodging");
            StartCoroutine("IFrames");
        }
        
        if(newPlayerHealth.currentHealth <= 0)
        {
            //newPlayerAttack.attacking == true ||
            playerActionsAsset.Player.Disable();
        }
        else
        {
            playerActionsAsset.Player.Enable();
        }

        if (stat_skill_1.triggered)
        {
            Skill skill1 = GameObject.Find("SkillLoadout").transform.GetChild(0).gameObject.GetComponent<Skill>();
            if (skill1.canSkill)
            {
                skill1.ActivateSkill(0);
            }
        }

        if (stat_skill_2.triggered)
        {
            Skill skill2 = GameObject.Find("SkillLoadout").transform.GetChild(1).gameObject.GetComponent<Skill>();
            if (skill2.canSkill)
            {
                skill2.ActivateSkill(1);
            }
        }

        if (subspecies_skill.triggered)
        {
            Skill subskill = GameObject.Find("SkillLoadout").transform.GetChild(2).gameObject.GetComponent<Skill>();
            if (subskill.canSkill)
            {
                subskill.ActivateSkill(2);
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
        yield return new WaitForSeconds(.15f);
        activeDodge = false;
        yield return new WaitForSeconds(1f);
        canDodge = true;
    }
    IEnumerator IFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(.25f);
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
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            //this.rb.rotation = Quaternion.RotateTowards(this.rb.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 1000);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 15f * Time.deltaTime);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
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
