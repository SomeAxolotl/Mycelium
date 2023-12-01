using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NewSporeCam : MonoBehaviour
{
    public List<CinemachineFreeLook> cameras = new List<CinemachineFreeLook>();

    public void SwitchCamera(string cameraName)
    {
        //CameraName
        foreach (CinemachineFreeLook cam in cameras)
        {
            Debug.Log(cam.name);
            if (cam.name == cameraName)
            {
                cam.Priority = 10;
            }
            else
            {
                Debug.Log("running");
                cam.Priority = 0;
            }
        }

      
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchCamera("GrowCamera");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchCamera("Main Camera");
        }
    }

      
}
