using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSaver : MonoBehaviour
{
    void OnDestroy(){
        Debug.Log("Save particles!!");
        foreach(Transform child in transform){
            ParticleSystem particles = child.GetComponent<ParticleSystem>();
            if(particles != null){
                child.parent = null;
            }
        }
    }
}
