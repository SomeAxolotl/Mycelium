using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    private Vector3 forceDirection = Vector3.zero;
    public float moveSpeed;
    [SerializeField] private Camera playerCamera;

    //Input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;
    private InputAction dodge;
    bool canDodge = true;
    bool activeDodge = false;
    public bool isInvincible = false;
    bool playerSwapping;
    SwapCharacter swapCharacter;
    NewPlayerAttack newPlayerAttack;

    // Start is called before the first frame update
    private void Start()
    {
        swapCharacter = GetComponent<SwapCharacter>();
        newPlayerAttack = GetComponent<NewPlayerAttack>();
        moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;
        rb = GetComponentInChildren<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        move = playerActionsAsset.Player.Move;
        dodge = playerActionsAsset.Player.Dodge;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        rb = GetComponentInChildren<Rigidbody>();
        moveSpeed = swapCharacter.currentCharacterStats.moveSpeed;

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
        
        if(newPlayerAttack.attacking == true)
        {
            playerActionsAsset.Player.Disable();
        }
        else
        {
            playerActionsAsset.Player.Enable();
        }
    }

    private void FixedUpdate()
    {
        LookAt();
        
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * moveSpeed;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * moveSpeed;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;
    }
    IEnumerator Dodging()
    {
        canDodge = false;
        activeDodge = true;
        rb.AddForce(transform.forward * 3f, ForceMode.Impulse);
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
            this.rb.rotation = Quaternion.RotateTowards(this.rb.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 1000);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
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
