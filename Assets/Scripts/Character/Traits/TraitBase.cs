using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitBase : MonoBehaviour
{
    [HideInInspector] public string traitName;
    [HideInInspector] public string traitDesc;

    [HideInInspector] public GameObject player;
    [HideInInspector] public CharacterStats characterStats;
    [HideInInspector] public HUDStats hudStats;
    [HideInInspector] public GameObject playerParent;
    [HideInInspector] public PlayerController controller;
    [HideInInspector] public PlayerHealth health;

    //On the trait being put on the spore
    public virtual void Start(){
        player = GameObject.FindWithTag("currentPlayer");
        characterStats = player.GetComponent<CharacterStats>();
        hudStats = GameObject.Find("HUD").GetComponent<HUDStats>();
        
        playerParent = player.transform.parent.gameObject;
        health = playerParent.GetComponent<PlayerHealth>();
        controller = playerParent.GetComponent<PlayerController>();
    }
}
