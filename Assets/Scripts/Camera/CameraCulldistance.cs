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

        //this has nothing to do with culling its actually just to adjust the x position of the fancy cinemachine camera thing. -ryan
        if(SceneManager.GetActiveScene().name == "New Tutorial")
        {
            StartCoroutine(LookLeft(GetComponent<CinemachineFreeLook>()));
        }
    }

    IEnumerator LookLeft(CinemachineFreeLook freeLook)
    {
        GetComponent<CinemachineInputProvider>().enabled = false;

        for(int i = 0; i < 200; i++)
        {
            freeLook.m_XAxis.Value += 0.6f;
            yield return null;
        }

        GetComponent<CinemachineInputProvider>().enabled = true;
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
