using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class MushroomPlayerSensorController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Renderer SampleMaterial;
    [SerializeField] private List<Renderer> Renderers;
    [SerializeField] private Light thisLight;
    [Header("Configuration")]
    [SerializeField] private float DeactivateSpeed = 1;
    [SerializeField] private float ActivateSpeed = 1;
    [SerializeField] private bool DisableOnExit = true;
    private Color StartGlow;
    private float startLightIntensity;
    bool entered = false;


    private void Start(){
        StartGlow = SampleMaterial.material.GetColor("_Glow_Color");
        if(thisLight!=null){
            startLightIntensity = thisLight.intensity;
            thisLight.intensity = 0;
        }
        foreach(Renderer renderer in Renderers){
            renderer.material.SetColor("_Glow_Color", new Color(0,0,0));
        }
    }
    private void OnTriggerExit(Collider other){
        if(DisableOnExit && other.gameObject.tag == "currentPlayer"){
            StartCoroutine(Darken());
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "currentPlayer" && entered == false){
            StartCoroutine(Illuminate());
            entered = true;
        }

    }
    IEnumerator Darken(){
        float t = 0f;
        Color currentGlow;
        float currentModifier = 0;
        float darkenLightIntensity=0;
        if(thisLight!=null)
            darkenLightIntensity = thisLight.intensity;
        while (t < DeactivateSpeed) 
        {   
            currentModifier = (t/DeactivateSpeed);
            currentGlow = new Color(StartGlow.r - StartGlow.r*currentModifier, StartGlow.g - StartGlow.g*currentModifier, StartGlow.b - StartGlow.b*currentModifier);
            foreach(Renderer renderer in Renderers)
            {
                renderer.material.SetColor("_Glow_Color", currentGlow);
            }
            if(thisLight!=null)
                thisLight.intensity = Mathf.Lerp(darkenLightIntensity,0,t);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
    }
    IEnumerator Illuminate(){
        float t = 0f;
        Color currentGlow;
        float currentModifier = 0;
        while (t < ActivateSpeed) 
        {   
            currentModifier = (t/ActivateSpeed);
            currentGlow = new Color(StartGlow.r*currentModifier, StartGlow.g*currentModifier, StartGlow.b*currentModifier);
            foreach(Renderer renderer in Renderers)
            {
                renderer.material.SetColor("_Glow_Color", currentGlow);
            }
            if(thisLight!=null)
                thisLight.intensity = Mathf.Lerp(0,startLightIntensity,t);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
