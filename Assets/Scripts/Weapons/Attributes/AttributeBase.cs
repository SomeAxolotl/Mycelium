using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeBase : MonoBehaviour
{
    private WeaponStats stats;
    private WeaponCollision hit;

    [SerializeField] private string attName;

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

    private void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Nasty";
        stats.wpnName = attName + " " + stats.wpnName;
    }

    private void Hit(GameObject target, float damage){
        Debug.Log("Target was hit");
        hit.dmgDealt = 0;
        Poison poisonEffect = target.AddComponent<Poison>();
        poisonEffect.PoisonStats(damage / 2.5f);
    }
}
