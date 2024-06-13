using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class SwapWeapon : MonoBehaviour
{
    private ThirdPersonActionsAsset playerActionsAsset;
    private Collider[] weaponColliders;
    [SerializeField] private LayerMask weaponLayer;
    Transform weapon;
    [HideInInspector] public GameObject currentCharacter;
    [HideInInspector] public Transform weaponHolder;
    SwapCharacter swapCharacter;
    PlayerController playerController;
    private GameObject curWeapon;
    public GameObject O_curWeapon{
        get{return curWeapon;}
        set{
            if(curWeapon != null){
                SwappedWeapon?.Invoke(curWeapon, value);
            }
            curWeapon = value; 
        }
    }
    public Action<GameObject, GameObject> SwappedWeapon;

    //[SerializeField] private float proximityRadius = 5f;
    [SerializeField] public Color betterStatColor;
    [SerializeField] public Color worseStatColor;
    [SerializeField] public Color evenStatColor;

    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapCharacter = GetComponent<SwapCharacter>();
        playerController = GetComponent<PlayerController>();
        currentCharacter = GameObject.FindWithTag("currentPlayer");
        if (GameObject.FindWithTag("currentWeapon") == null && SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameObject startingWeapon = Instantiate(Resources.Load("Slash/Stick"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            startingWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            startingWeapon.GetComponent<Collider>().enabled = false;
            curWeapon = startingWeapon;

        }
    }

    private void OnDisable()
    {
        if(playerActionsAsset != null)
        {
            playerActionsAsset.Disable();
        }
    }


    /*public void UpdateCharacter(GameObject currentPlayer)
    {
        currentCharacter = currentPlayer;
        Transform[] playerChildren = currentCharacter.GetComponentsInChildren<Transform>();
        foreach (Transform child in playerChildren)
        {
            if(child.tag == "WeaponSlot")
            {
                weaponHolder = child;
            }
        }
        curWeapon = GameObject.FindWithTag("currentWeapon");
        curWeapon.transform.parent = weaponHolder.transform.parent;
        DontDestroyOnLoad(curWeapon);
    }*/
}
