using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamTracker : MonoBehaviour
{
    private Transform currentPlayer;
    public CinemachineFreeLook freeLookCamera;
    public CinemachineTargetGroup targetGroup;
    private Transform currentTarget;
    private bool isLockedOn = false;

    void Update()
    {
        currentPlayer = GameObject.FindWithTag("currentPlayer").transform;
        transform.position = currentPlayer.position;

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
            targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
            targetGroup.enabled = false;
            freeLookCamera.LookAt = transform;
            freeLookCamera.Follow = transform;
        }
        else
        {

            currentTarget = FindClosestEnemy();
            if (currentTarget != null)
            {
                isLockedOn = true;
                targetGroup.enabled = true;
                targetGroup.AddMember(currentPlayer, 1f, 0f);
                targetGroup.AddMember(currentTarget, 1f, 0f);
                freeLookCamera.LookAt = targetGroup.transform;
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
