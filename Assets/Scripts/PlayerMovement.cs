using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour

{
    public float speed = 5.0f;
    public float Jumpforce = 5.0f;
    public bool isOnGround = true;
    private float horizontalInput;
    private float forwardInput;
    public Transform warp;
    private Rigidbody playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
            isOnGround = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }

        if (collision.gameObject.CompareTag("Warp"))
        {
            transform.position=new Vector3(warp.position.x,warp.position.y,warp.position.z);
        }

        if (collision.gameObject.CompareTag("Title"))
        {
            SceneManager.LoadScene("Title");
        }

        if (collision.gameObject.CompareTag("Hard"))
        {
            SceneManager.LoadScene("HardLevel");
        }

        if (collision.gameObject.CompareTag("Title2"))
        {
            SceneManager.LoadScene("Title2");
        }

        if (collision.gameObject.CompareTag("Win"))
        {
            SceneManager.LoadScene("Win");
        }
    }
}
