using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRandomizer : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().material.SetFloat("_FishSeed", Vector3.Distance(transform.position, transform.position - transform.position));
    }
}
