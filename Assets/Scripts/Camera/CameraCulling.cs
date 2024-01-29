using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraCulling : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float[] distances = new float[32];
        //idk 
        distances[14] = 1;
        distances[15] = 1;
        camera.layerCullDistances = distances;
    }

    public void SetCameraRenderDistance(float renderDistance)
    {
        CinemachineFreeLook cinemachineFreeLook = GetComponent<CinemachineFreeLook>();

        cinemachineFreeLook.m_Lens.FarClipPlane = renderDistance;
    }
}
