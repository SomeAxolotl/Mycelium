using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using static System.TimeZoneInfo;

public class CamTracker : MonoBehaviour
{
    private Transform centerPoint;
    private GameObject currentPlayer;

    public Transform currentTarget;

    [SerializeField] private CinemachineBrain cineBrain;
    [SerializeField] private CinemachineVirtualCamera virtualResetCamera;

    private ThirdPersonActionsAsset playerInput;

    private void Awake()
    {
        playerInput = new ThirdPersonActionsAsset();
    }

    private void Start()
    {
        currentPlayer = GameObject.FindWithTag("currentPlayer");
    }

    private void Update()
    {
        //Null check for Current Player and Center Point
        if (GameObject.FindWithTag("currentPlayer").transform.Find("CenterPoint") != null)
        {
            //Camera Tracker position is set to Center Point position
            centerPoint = GameObject.FindWithTag("currentPlayer").transform.Find("CenterPoint");
            transform.position = centerPoint.position;

            //Make sure we have the correct reference to the Current Player and then rotate the Camera Tracker
            if (currentPlayer.tag != "currentPlayer")
            {
                currentPlayer = GameObject.FindWithTag("currentPlayer");
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPlayer.transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if(playerInput.Player.ResetCamera.WasPressedThisFrame() && cineBrain.IsBlending == false)
        {
            StartCoroutine(ResetCameraPosition());
        }
    }

    IEnumerator ResetCameraPosition()
    {
        float elapsedTime = 0f;

        virtualResetCamera.enabled = true;

        while (elapsedTime < cineBrain.m_DefaultBlend.m_Time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualResetCamera.enabled = false;
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
