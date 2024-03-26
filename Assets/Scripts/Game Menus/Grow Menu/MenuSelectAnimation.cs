using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectAnimation : MonoBehaviour
{
    [SerializeField] private bool startSelected = false;
    [SerializeField] private Animator animator;
    
    private void OnEnable(){
        if(startSelected) animator.SetBool("Selected", true);
    }
    public void PlaySelectAnimation(){
        animator.SetBool("Selected", true);
    }
    public void PlayDeselectAnimation(){
        animator.SetBool("Selected", false);
    }
}
