using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMovement : MonoBehaviour
{
    //!!!!This code is just for testing purposes!!!!
    
    //Basic movement speed of 2UU per second.
    public float moveSpeed = 2f;

    public float rotationSpeed = 90f;

    // Update is called once per frame
    void Update()
    {
        //Yes i know this is bad but it's just for testing

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 newPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;

            transform.position = newPosition;
        }

        if (Input.GetKey(KeyCode.A))
        {
            float newRotationY = transform.eulerAngles.y - rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Euler(0f, newRotationY, 0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            float newRotationY = transform.eulerAngles.y + rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Euler(0f, newRotationY, 0f);
        }
    }
}
