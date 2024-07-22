using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraCulldistance : MonoBehaviour
{
    public float renderDistance = 150;
    private float[] distances = new float[32];

    [SerializeField] List<int> layersToSkip = new List<int>();

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

        for(int i = 0; i<32; i++)
        {
            if(layersToSkip.Contains(i) == false)
            {
                distances[i] = renderDistance;
            }
        }

        camera.layerCullDistances = distances;
    }
}
