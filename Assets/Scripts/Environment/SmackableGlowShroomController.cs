using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SmackableGlowShroomController : MonoBehaviour
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
    [SerializeField] int minNutrientParticles = 2;
    [SerializeField] int maxNutrientParticles = 4;
    [SerializeField] float nutrientYOffset = 1f;

    bool wasSmacked = false;

    private void Start(){
        audioSource.pitch = Mathf.Clamp(1/((gameObject.transform.localScale.x + gameObject.transform.localScale.z)/2), 0.1f, 100f);
    }
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "currentWeapon" && Attackable == true)
        {
            Bounce();
        }
        else if (other.gameObject.tag == "currentPlayer" && other.GetComponentInParent<PlayerController>().activeDodge && Attackable == true)
        {
            Bounce();
        }
    }
    private void Bounce(){
        //Debug.Log("Smacked!");
        if (!wasSmacked ) //SceneManager.GetActiveScene().name != "New Tutorial"
        {
            int randNutrientAmount = Random.Range(minNutrientParticles, maxNutrientParticles);
            ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", randNutrientAmount, 0.1f, this.gameObject.transform.position + new Vector3(0f, nutrientYOffset, 0f), Quaternion.Euler(-90f, 0f, 0f));
            wasSmacked = true;
        }
        
        Anim.SetTrigger("Bounce");
        audioSource.PlayOneShot(audioClip);
        StartCoroutine(GlowAttacked());
        Attackable = false;
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
