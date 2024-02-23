using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class SmackableShroomController : MonoBehaviour
{
    private Boolean Attackable = true;
    [Header("Components")]
    [SerializeField] private Animator Anim;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [Header("Components")]
    [SerializeField] private float CooldownDuration = 1f;

    private void Start(){
        audioSource.pitch = 1/((gameObject.transform.localScale.x + gameObject.transform.localScale.z)/2);
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
        Anim.SetTrigger("Bounce");
            audioSource.PlayOneShot(audioClip);
            StartCoroutine(Cooldown());
            Attackable = false;
    }
    IEnumerator Cooldown(){
        float t = 0f;
        while (t < CooldownDuration) 
        {   
            t += Time.deltaTime;
            yield return null;
        }
        Attackable = true;
    }
}
