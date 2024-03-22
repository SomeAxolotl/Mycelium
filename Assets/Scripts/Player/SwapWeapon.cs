using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public GameObject curWeapon;

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
            GameObject startingWeapon = Instantiate(Resources.Load("Daybreak Arboretum/Slash/Stick"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            startingWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            startingWeapon.GetComponent<Collider>().enabled = false;
            curWeapon = startingWeapon;

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
