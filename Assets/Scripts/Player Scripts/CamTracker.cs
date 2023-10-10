using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTracker : MonoBehaviour
{
    private GameObject currentCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Finds an empty gameobject tagged Tracker which will always be on the active player and allows the camera to switch between them.
        currentCam = GameObject.FindWithTag("Tracker");
        transform.position = currentCam.transform.position;
    }
}
