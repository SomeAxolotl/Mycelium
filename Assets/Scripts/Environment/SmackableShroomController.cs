using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class SmackableShroomController : MonoBehaviour
{
    private Boolean Attackable = true;
    [Header("Components")]
    //[SerializeField] private CapsuleCollider GoOutCollider;
    //[SerializeField] private CapsuleCollider Hitbox;
    [SerializeField] private Renderer SampleMaterial;
    [SerializeField] private Animator Anim;
    [SerializeField] private List<Renderer> Renderers;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [Header("Configuration")]
    [SerializeField] private float HitGlowUpDuration = 1;
    [SerializeField] private float HitGlowDownDuration = 2;
    [SerializeField] private float HitGlowAmount = 1;

    private void Start(){
        audioSource.pitch = 1/((gameObject.transform.localScale.x + gameObject.transform.localScale.z)/2);
    }
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "currentWeapon" && Attackable == true)
        {
            Debug.Log("Smacked!");
            Anim.SetTrigger("Bounce");
            audioSource.PlayOneShot(audioClip);
            StartCoroutine(GlowAttacked());
            Attackable = false;
        }
    }
    IEnumerator GlowAttacked(){
        float t = 0f;
        Color startGlow = SampleMaterial.material.GetColor("_Glow_Color");
        Color currentGlow;
        float currentModifier = 0;
        while (t < HitGlowUpDuration) 
        {   
            currentModifier = (t/HitGlowUpDuration)*HitGlowAmount;
            currentGlow = new Color(startGlow.r+currentModifier, startGlow.g+currentModifier, startGlow.b+currentModifier);
            foreach(Renderer renderer in Renderers)
            {
                renderer.material.SetColor("_Glow_Color", currentGlow);
            }
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        startGlow = SampleMaterial.material.GetColor("_Glow_Color");
        while (t < HitGlowDownDuration) 
        {   
            currentModifier = (t/HitGlowDownDuration)*HitGlowAmount;
            currentGlow = new Color(startGlow.r-currentModifier, startGlow.g-currentModifier, startGlow.b-currentModifier);
            foreach(Renderer renderer in Renderers)
            {
                renderer.material.SetColor("_Glow_Color", currentGlow);
            }
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        Attackable = true;
    }
}
