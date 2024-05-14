using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeBase : MonoBehaviour
{
    [HideInInspector] public WeaponStats stats;
    [HideInInspector] public WeaponCollision hit;

    [SerializeField] public string attName;

    private void OnEnable(){
        stats = GetComponent<WeaponStats>();
        hit = GetComponent<WeaponCollision>();
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
