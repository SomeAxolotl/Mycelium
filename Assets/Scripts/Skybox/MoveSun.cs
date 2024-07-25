using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSun : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 45f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.RightArrow))
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;

            // Get the current rotation
            Quaternion currentRotation = transform.rotation;

            // Calculate the new Y rotation
            float newRotation = currentRotation.eulerAngles.y + rotationAmount;

            // Apply the new rotation
            transform.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, newRotation, 0f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;

            // Get the current rotation
            Quaternion currentRotation = transform.rotation;

            // Calculate the new Y rotation
            float newRotation = currentRotation.eulerAngles.y - rotationAmount;

            // Apply the new rotation
            transform.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, newRotation, 0f);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;

            // Get the current rotation
            Quaternion currentRotation = transform.rotation;

            // Calculate the new Y rotation
            float newRotation = currentRotation.eulerAngles.x + rotationAmount;

            // Apply the new rotation
            transform.rotation = Quaternion.Euler(newRotation, currentRotation.eulerAngles.y, 0f);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;

            // Get the current rotation
            Quaternion currentRotation = transform.rotation;

            // Calculate the new Y rotation
            float newRotation = currentRotation.eulerAngles.x - rotationAmount;

            // Apply the new rotation
            transform.rotation = Quaternion.Euler(newRotation, currentRotation.eulerAngles.y, 0f);
        }*/
    }
}
