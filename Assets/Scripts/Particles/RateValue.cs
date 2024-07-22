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
        PickColorAndRate();
    }
    }
    public float baseRate = 1;
    [SerializeField] private ParticleSystem savedParticles;

    private void PickColorAndRate(){
        //Bronze tier
        if(savedParticles == null){return;}
        var emmision = savedParticles.emission;
        if(rateMult <= 0){emmision.rateOverTime = 0; return;}
        if(rateMult <= 4){
            emmision.rateOverTime = (rateMult * baseRate);
            savedParticles.startColor = new Color(0.69f, 0.55f, 0.34f, 1f);
            return;
        }
        if(rateMult <= 8){
            emmision.rateOverTime = ((rateMult - 4) * baseRate);
            savedParticles.startColor = new Color(0.66f, 0.66f, 0.66f, 1f);
            return;
        }
        emmision.rateOverTime = 3;
        savedParticles.startColor = new Color(0.83f, 0.69f, 0.22f, 1f);
    }

    private void Awake(){
        if(savedParticles == null){
            savedParticles = GetComponent<ParticleSystem>();
        }
        O_rateMult = rateMult;
    }
}
