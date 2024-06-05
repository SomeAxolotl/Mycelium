using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeBase : MonoBehaviour
{
    [HideInInspector] public WeaponStats stats;
    [HideInInspector] public WeaponCollision hit;
    [HideInInspector] public WeaponInteraction interact;
    [HideInInspector] public GameObject player;
    [HideInInspector] public GameObject playerParent;
    [HideInInspector] public PlayerAttack attack;
    [HideInInspector] public SwapWeapon swap;
    [HideInInspector] public CharacterStats characterStats;
    [HideInInspector] public HUDStats hudStats;
    public bool statChange = false;

    public string attName;
    public string attDesc;

    public AttributeAssigner.Rarity rating;
    //Changes based on the attribute but is info that is transfered between scenes
    public float specialAttNum = 0;

    //These are set within scripts not inspectors
    [HideInInspector] public int primalAmount;
    [HideInInspector] public int sentienceAmount;
    [HideInInspector] public int speedAmount;
    [HideInInspector] public int vitalityAmount;

    private void Start(){
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
        if(attack != null){
            attack.StartedAttack -= StartAttack;
            attack.FinishedAttack -= StopAttack;
        }
    }

    public virtual void Initialize(){
        //Add name here
    }
    public virtual void Hit(GameObject target, float damage){
        //Put on hit things here
    }
    public virtual void Equipped(){
        playerParent = player.transform.parent.gameObject;
        attack = playerParent.GetComponent<PlayerAttack>();
        if(attack != null){
            attack.StartedAttack += StartAttack;
            attack.FinishedAttack += StopAttack;
        }else{
            Debug.Log("It was null...");
        }
        //Put on equip things here
    }
    public virtual void Unequipped(){
        //Put on unequip things here
    }
    public virtual void StartAttack(){
        //Put on start attack things here
    }
    public virtual void StopAttack(){
        //Put on stop attack things here
    }
}
