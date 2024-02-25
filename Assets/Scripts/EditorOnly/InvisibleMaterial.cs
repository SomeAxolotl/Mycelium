using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleMaterial : MonoBehaviour
{
    void Start()
    {
        if (Application.isPlaying)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
