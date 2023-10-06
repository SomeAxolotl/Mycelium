using System.Collections;
using System.Collections.Generic;
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
    
    [SerializeField]
    private Camera playerCamera;

    //Input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;
    bool playerSwapping;
    bool fetchedStats = false;

    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        move = playerActionsAsset.Player.Move;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        MyInput();
        SpeedControl();
        
        playerSwapping = gameObject.GetComponent<Swapping>().playerSwapping;

        if(fetchedStats == false || playerSwapping == true)
        {
            StartCoroutine("FetchStats");
        }
    }
    IEnumerator FetchStats()
    {
        yield return new WaitForEndOfFrame();
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
        if(flatVel.magnitude > finalMoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * finalMoveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
