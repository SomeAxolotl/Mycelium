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
        }
    }

    private void OnResetCamera()
    {
        //Make sure we have the correct reference to the Current Player and then rotate the Camera Tracker
        if (currentPlayer.tag != "currentPlayer")
        {
            currentPlayer = GameObject.FindWithTag("currentPlayer");
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPlayer.transform.eulerAngles.y, transform.eulerAngles.z);

        if (cineBrain.IsBlending == false)
        {
            StartCoroutine(ResetCameraPosition());
        }
    }

    IEnumerator ResetCameraPosition()
    {
        float elapsedTime = 0f;

        float oldBlendTime = cineBrain.m_DefaultBlend.m_Time;
        float newBlendTime = 0.1f;

        virtualResetCamera.enabled = true;
        cineBrain.m_DefaultBlend.m_Time = newBlendTime;

        while (elapsedTime < cineBrain.m_DefaultBlend.m_Time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualResetCamera.enabled = false;
        yield return new WaitForSeconds(newBlendTime);
        cineBrain.m_DefaultBlend.m_Time = oldBlendTime;
    }
}
