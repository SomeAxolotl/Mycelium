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
    [HideInInspector] public Rigidbody rb;

    //On the trait being put on the spore
    public virtual void Start(){
        Debug.Log("Make this not the current player");
        player = GameObject.FindWithTag("currentPlayer");
        characterStats = player.GetComponent<CharacterStats>();
        hudStats = GameObject.Find("HUD").GetComponent<HUDStats>();
        
        rb = player.GetComponent<Rigidbody>();
        playerParent = player.transform.parent.gameObject;
        health = playerParent.GetComponent<PlayerHealth>();
        controller = playerParent.GetComponent<PlayerController>();
    }
}
