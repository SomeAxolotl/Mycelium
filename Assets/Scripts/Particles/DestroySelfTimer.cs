using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfTimer : MonoBehaviour
{
    [SerializeField] private float Timer = 1.2f;
    void Start()
    {
        Destroy(gameObject, Timer);
    }
}
