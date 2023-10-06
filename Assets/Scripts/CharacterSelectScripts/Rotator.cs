using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
   public float rotationspeed = 50;
    void Update()
    {
        transform.Rotate(0, rotationspeed * Time.deltaTime, 0);
    }
}
