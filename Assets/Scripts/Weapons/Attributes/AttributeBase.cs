using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeBase : MonoBehaviour
{
    [HideInInspector] public WeaponStats stats;
    [HideInInspector] public WeaponCollision hit;
    [HideInInspector] public WeaponInteraction interact;

    [SerializeField] public string attName;
    [SerializeField] public string attDesc;

    private void OnEnable(){
        stats = GetComponent<WeaponStats>();
        hit = GetComponent<WeaponCollision>();
        interact = GetComponent<WeaponInteraction>();
        if(stats != null){

        }
        if(hit != null){
            hit.HitEnemy += Hit;
        }
        Initialize();
    }
    private void OnDisable(){
        if(stats != null){

        }
        if(hit != null){
            hit.HitEnemy -= Hit;
        }
    }

    public virtual void Initialize(){
        //Add name here
    }

    public virtual void Hit(GameObject target, float damage){
        //Put on hit things here
    }
}
