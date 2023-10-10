using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DONTDESTROYONLOAD : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (GameObject.FindGameObjectsWithTag(this.gameObject.tag).Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
