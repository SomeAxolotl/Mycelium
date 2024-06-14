using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SensitivityManager : MonoBehaviour
{
    CinemachineFreeLook cinemachineFreeLook;

    public float sensitivity {private get; set;} = 0.5f;

    void Awake()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
    }

    public void UpdateCamera()
    {
        InputManager.Controller controller = InputManager.Instance.GetLatestController();

        if (controller.usesMouse)
        {
            SwitchToMouseCamera();
        }
        else
        {
            SwitchToControllerCamera();
        }
    }

    void SwitchToMouseCamera()
    {
        cinemachineFreeLook.m_YAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
        cinemachineFreeLook.m_XAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;

        cinemachineFreeLook.m_XAxis.m_MaxSpeed = Mathf.Lerp(0.025f, 0.25f, sensitivity);
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = cinemachineFreeLook.m_XAxis.m_MaxSpeed / 80;

        cinemachineFreeLook.m_XAxis.m_AccelTime = 0;
        cinemachineFreeLook.m_XAxis.m_DecelTime = 0;
        cinemachineFreeLook.m_YAxis.m_AccelTime = 0;
        cinemachineFreeLook.m_YAxis.m_DecelTime = 0;
    }

    void SwitchToControllerCamera()
    {
        cinemachineFreeLook.m_XAxis.m_SpeedMode = AxisState.SpeedMode.MaxSpeed;
        cinemachineFreeLook.m_YAxis.m_SpeedMode = AxisState.SpeedMode.MaxSpeed;

        cinemachineFreeLook.m_XAxis.m_MaxSpeed = Mathf.Lerp(100, 300, sensitivity);
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = 2;

        cinemachineFreeLook.m_XAxis.m_AccelTime = 0.2f;
        cinemachineFreeLook.m_XAxis.m_DecelTime = 0.15f;
        cinemachineFreeLook.m_YAxis.m_AccelTime = 0.3f;
        cinemachineFreeLook.m_YAxis.m_DecelTime = 0.3f;
    }
}
