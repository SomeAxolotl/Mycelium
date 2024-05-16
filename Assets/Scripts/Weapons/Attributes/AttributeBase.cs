using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeBase : MonoBehaviour
{
    [HideInInspector] public WeaponStats stats;
    [HideInInspector] public WeaponCollision hit;
    [HideInInspector] public WeaponInteraction interact;
    [HideInInspector] public GameObject player;
    [HideInInspector] public CharacterStats characterStats;
    [HideInInspector] public HUDStats hudStats;
    public bool statChange = false;

    [HideInInspector] public string attName;
    [HideInInspector] public string attDesc;

    private void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        characterStats = player.GetComponent<CharacterStats>();
        hudStats = GameObject.Find("HUD").GetComponent<HUDStats>();
    }
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

    public virtual void Equipped(){
        //Put on equip things here
    }

    public virtual void Unequipped(){
        //Put on unequip things here
    }
}
