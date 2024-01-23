using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CamTracker : MonoBehaviour
{
    private Transform currentPlayer;
    public Camera mainCam;
    public CinemachineFreeLook freeLookCamera;
    public CinemachineTargetGroup targetGroup;
    public Transform currentTarget;
    public Crosshair crosshair;
    public bool isLockedOn = false;

    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction lockon;
    private void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        lockon = playerActionsAsset.Player.LockOn;
    }
    private void Update()
    {
        currentPlayer = GameObject.FindWithTag("currentPlayer").transform;
        transform.position = currentPlayer.position;

        if (lockon.triggered)
        {
            ToggleLockOn();
        }
        
        if(isLockedOn)
        {
            freeLookCamera.Follow = null;
            freeLookCamera.LookAt = transform;
            crosshair.crosshairImage.enabled = true;
            crosshair.target = currentTarget;

            Vector3 directionToTarget = currentTarget.position - currentPlayer.position;
            directionToTarget.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            currentPlayer.rotation = Quaternion.Slerp(currentPlayer.rotation, targetRotation, 15f * Time.deltaTime);

            Vector3 relativePosition = -currentPlayer.forward * 12f + currentPlayer.up * 4.5f;
            Vector3 targetPosition = currentPlayer.position + relativePosition;
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, targetPosition, 10f * Time.deltaTime);

            Quaternion targetCameraRotation = Quaternion.Euler(20f, currentPlayer.rotation.eulerAngles.y, 0f);
            mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, targetCameraRotation, 100f * Time.deltaTime);
            if (Vector3.Distance(currentPlayer.position, currentTarget.position) > 25f)
            {
                ToggleLockOn();
            }
        }
    }
    public void ToggleLockOn()
    {
        if (isLockedOn)
        {
            isLockedOn = false;
            targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
            targetGroup.enabled = false;
            crosshair.crosshairImage.enabled = false;
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
            }
        }
    }
    Transform FindClosestEnemy()
    {
        //Bounds on the screen of where to search for enemies
        float startX = 0.3f;
        float startY = 0.3f;
        float endX = 0.7f;
        float endY = 0.7f;

        int rayCountX = 20; //The number of rays in the X direction
        int rayCountY = 20; //The number of rays in the Y direction

        for (int i = 0; i < rayCountX; i++)
        {
            for (int j = 0; j < rayCountY; j++)
            {
                //Calculate the current viewport point
                float viewportX = Mathf.Lerp(startX, endX, (float)i / (rayCountX - 1));
                float viewportY = Mathf.Lerp(startY, endY, (float)j / (rayCountY - 1));

                Ray ray = Camera.main.ViewportPointToRay(new Vector3(viewportX, viewportY, 0));

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Enemy") && Vector3.Distance(currentPlayer.position, hit.collider.transform.position) < 25f || hit.collider.CompareTag("Boss") && Vector3.Distance(currentPlayer.position, hit.collider.transform.position) < 25f)
                    {
                        return hit.transform;
                    }
                }
            }
        }
        return null;
    }
}
