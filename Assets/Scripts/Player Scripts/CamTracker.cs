using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamTracker : MonoBehaviour
{
    private GameObject currentPlayer;
    public CinemachineFreeLook freeLookCamera;
    private Transform currentTarget;
    private bool isLockedOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        currentPlayer = GameObject.FindWithTag("currentPlayer");
        transform.position = currentPlayer.transform.position;
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ToggleLockOn();
        }
    }
    void ToggleLockOn()
    {
        if (isLockedOn)
        {
            // Unlock the camera
            isLockedOn = false;
            freeLookCamera.LookAt = currentPlayer.transform;
        }
        else
        {
            FindClosestEnemy();
            // Lock onto the nearest enemy
            currentTarget = FindClosestEnemy();
            if (currentTarget != null)
            {
                isLockedOn = true;
                freeLookCamera.LookAt = currentTarget; // Stop following the player
            }
        }
    }
    Transform FindClosestEnemy()
    {
        // Raycast from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object is an enemy (you may need to adjust the tag or layer)
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Boss"))
            {
                return hit.transform;
            }
        }
        return null;
    }
}
