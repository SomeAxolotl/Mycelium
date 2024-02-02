using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCulldistance : MonoBehaviour
{
    public float renderDistance = 150;
    private float[] distances = new float[32];

    void Start()
    {
        RefreshRenderDistance();
    }

    public void SetRenderDistance(float newRenderDistance)
    {
        renderDistance = newRenderDistance;

        RefreshRenderDistance();
    }

    public void RefreshRenderDistance()
    {
        Camera camera = GetComponent<Camera>();

        for(int i = 0; i<15; i++)
            distances[i] = renderDistance;
        //Skip Layer 15 (IgnoreFog Layer)
        for(int i = 16; i<32; i++)
            distances[i] = renderDistance;
        camera.layerCullDistances = distances;
    }
}
