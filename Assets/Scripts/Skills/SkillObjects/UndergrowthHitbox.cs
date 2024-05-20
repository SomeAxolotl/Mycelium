using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergrowthHitbox : MonoBehaviour
{
    public UndergrowthManager manager;
    [HideInInspector] public List<Collider> hitTargets;

    void OnTriggerEnter(Collider other){
        if(!hitTargets.Contains(other) && other.isTrigger){
            manager.ProcessTarget(other);  
        }
        hitTargets.Add(other);
    }
}
