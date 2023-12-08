using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NewSporeCam : MonoBehaviour
{
    private List<CinemachineFreeLook> cameras = new List<CinemachineFreeLook>();

    private void Start()
    {
        cameras.Add(GameObject.FindWithTag("MainCamera").GetComponent<CinemachineFreeLook>());
        cameras.Add(GameObject.FindWithTag("GrowCamera").GetComponent<CinemachineFreeLook>());
    }
    public void SwitchCamera(string cameraName)
    {
        //CameraName
        foreach (CinemachineFreeLook cam in cameras)
        {
            if (cam.name == cameraName)
            {
                cam.Priority = 10;
            }
            else
            {
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
