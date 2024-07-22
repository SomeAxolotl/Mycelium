using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RateValue : MonoBehaviour
{
    [SerializeField] private float rateMult;
    [HideInInspector] public float O_rateMult{
    get{
        return rateMult;
    }
    set{
        rateMult = value;
        if(savedParticles != null){
            var emmision = savedParticles.emission;
            emmision.rateOverTime = (rateMult * baseRate);
        }
    }
    }
    public float baseRate = 1;
    [SerializeField] private ParticleSystem savedParticles;

    private void Awake(){
        if(savedParticles == null){
            savedParticles = GetComponent<ParticleSystem>();
        }
        O_rateMult = rateMult;
    }
}
