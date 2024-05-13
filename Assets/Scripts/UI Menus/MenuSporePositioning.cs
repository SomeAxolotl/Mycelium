using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSporePositioning : MonoBehaviour
{
    Camera uiCamera;
    public float horizontalOffsetScalar = 1f;
    public float verticalOffsetScalar = 1f;
    const float zzzzz = -411.6f;

    void Start()
    {
        uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    //Totally legit and not ChatGPTd script
    void Update()
    {
        // Check if UI camera is assigned
        if (uiCamera != null)
        {
            // Get the screen position of the object in pixels
            Vector2 screenPos = uiCamera.WorldToScreenPoint(transform.position);

            // Normalize screen position to range [0, 1]
            //Vector2 normalizedScreenPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
            
            // Calculate the desired position relative to the camera
            Vector3 worldPoint = uiCamera.ScreenToWorldPoint(new Vector3(
                                            Screen.width * horizontalOffsetScalar,
                                            Screen.height * verticalOffsetScalar,
                                            uiCamera.transform.position.z));
                                            
            Vector3 desiredPosition = new Vector3(worldPoint.x, worldPoint.y, zzzzz);

            // Set the object's position to the desired position
            transform.position = desiredPosition;
        }
        else
        {
            Debug.LogWarning("UI Camera not assigned!");
        }
    }
}
