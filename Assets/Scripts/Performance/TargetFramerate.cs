using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFramerate : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }
}
