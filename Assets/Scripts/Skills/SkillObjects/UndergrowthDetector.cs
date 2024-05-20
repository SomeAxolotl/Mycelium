using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergrowthDetector : MonoBehaviour
{
    public UndergrowthMovement movement;

    private void OnTriggerEnter(Collider other){
        movement.hitSomething = true;
    }
}
