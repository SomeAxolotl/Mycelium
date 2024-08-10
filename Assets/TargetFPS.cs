using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFPS : MonoBehaviour
{
    // Set the desired frame rate
    [SerializeField] private int targetFPS = 60;

    void Start()
    {
        // Set the application's target frame rate
        Application.targetFrameRate = targetFPS;
    }
}
