using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MushroomPlayerSensorController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Renderer SampleMaterial;
    [SerializeField] private List<Renderer> Renderers;
    [Header("Configuration")]
    [SerializeField] private float DeactivateSpeed = 1;
    [SerializeField] private float ActivateSpeed = 1;
    private Color StartGlow;

    private void Start(){
        StartGlow = SampleMaterial.material.GetColor("_Glow_Color");
        foreach(Renderer renderer in Renderers)
            {
                renderer.material.SetColor("_Glow_Color", new Color(0,0,0));
            }
    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "currentPlayer"){
            StartCoroutine(Darken());
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "currentPlayer"){
            StartCoroutine(Illuminate());
        }

    }
    IEnumerator Darken(){
        float t = 0f;
        Color currentGlow;
        float currentModifier = 0;
        while (t < ActivateSpeed) 
        {   
            currentModifier = (t/ActivateSpeed);
            currentGlow = new Color(StartGlow.r - StartGlow.r*currentModifier, StartGlow.g - StartGlow.g*currentModifier, StartGlow.b - StartGlow.b*currentModifier);
            foreach(Renderer renderer in Renderers)
            {
                renderer.material.SetColor("_Glow_Color", currentGlow);
            }
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
            t += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
