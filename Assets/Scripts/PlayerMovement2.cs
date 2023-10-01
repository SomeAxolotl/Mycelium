using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement2 : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Player movement speed
    public float jumpForce = 8.0f; // Jump force
    public float gravity = -9.81f; // Gravity value

    private CharacterController controller;
    private Vector3 playerVelocity;
    public Transform warp;

    private bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2.0f; // Apply slight downward force when grounded
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2.0f * gravity);
            transform.rotation = Quaternion.identity;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Warp"))
        {
            transform.position=new Vector3(warp.position.x,warp.position.y,warp.position.z);
        }

        if (collision.gameObject.CompareTag("Title"))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
