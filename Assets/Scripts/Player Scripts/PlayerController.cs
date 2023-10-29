using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;

    //Vector3 moveDirection;
    Rigidbody rb;
    Transform player;
    private Vector3 forceDirection = Vector3.zero;
    float finalMoveSpeed;

    //private float moveSpeed = 5f;
    
    private Camera playerCamera;

    //Input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;
    private InputAction dodge;
    bool canDodge = true;
    bool activeDodge = false;
    public bool isInvincible = false;
    bool playerSwapping;
    bool fetchedStats = false;

    //Hold Button Inputs for Caches
    
    
    
    public LootCache lootCache;
    // Start is called before the first frame update
    private void Start()
    {
        playerCamera = Camera.main;
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
        EnableController();
        move = playerActionsAsset.Player.Move;
        dodge = playerActionsAsset.Player.Dodge;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        lootCache = GetComponent<LootCache>();
    }

    // Update is called once per frame
    private void Update()
    {
        MyInput();
        SpeedControl();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        playerSwapping = gameObject.GetComponent<Swapping>().playerSwapping;

        if(fetchedStats == false || playerSwapping == true)
        {
            UpdateStats();
        }
        
        if(dodge.triggered && canDodge == true)
        {
            StartCoroutine("Dodging");
            StartCoroutine("IFrames");
        }
        /*if(isInvincible)
        {
            
        }*/
        
    }

    public void UpdateStats()
    {
        Debug.Log("PC fetched stats");
        finalMoveSpeed = gameObject.GetComponent<StatTracker>().finalMoveSpeed;
        fetchedStats = true;
    }

    private void FixedUpdate()
    {
        LookAt();
        
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * finalMoveSpeed;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * finalMoveSpeed;

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

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    
    /*private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = player.forward * verticalInput + player.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }*/
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.RotateTowards(this.rb.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 1500);
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
        if(flatVel.magnitude > finalMoveSpeed && activeDodge == false)
        {
            Vector3 limitedVel = flatVel.normalized * finalMoveSpeed;
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
