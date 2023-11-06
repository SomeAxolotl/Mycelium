using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SwapWeapon : MonoBehaviour
{
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction swapItem;
    private Collider[] weaponColliders;
    [SerializeField] private LayerMask weaponLayer;
    Transform weapon;
    GameObject currentCharacter;
    Transform weaponHolder;
    SwapCharacter swapCharacter;
    public GameObject curWeapon;
    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapItem = playerActionsAsset.Player.SwapItem;
        swapCharacter = GetComponent<SwapCharacter>();
        if(GameObject.FindWithTag("currentWeapon") == null)
        {
            Instantiate(Resources.Load("StartWeapon"), GameObject.FindWithTag("WeaponSlot").transform);
            UpdateCharacter(GameObject.FindWithTag("currentPlayer"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        curWeapon.transform.position = weaponHolder.transform.position;;
        curWeapon.transform.rotation = weaponHolder.transform.rotation;
      
        weaponColliders = Physics.OverlapSphere(currentCharacter.transform.position, 4f, weaponLayer);
        foreach (var weaponCollider in weaponColliders)
        {
            weapon = weaponCollider.transform;
            Vector3 dirToWeapon = (weapon.position - currentCharacter.transform.position).normalized;
            if (Vector3.Angle(currentCharacter.transform.forward, dirToWeapon) <= 40 && swapItem.triggered || Vector3.Distance(currentCharacter.transform.position, weapon.position) <= 1f && swapItem.triggered)
            {
                //Debug.Log("swap");
                curWeapon.transform.position = weapon.position;
                // curWeapon.transform.rotation = Quaternion.Euler(-25, 0, 0);
                curWeapon.GetComponent<Collider>().enabled = true;
                weapon.position = weaponHolder.position;
                curWeapon.layer = LayerMask.NameToLayer("Weapon");
                curWeapon.transform.parent = null;
                curWeapon.tag = "Weapon";
                weapon.gameObject.layer = LayerMask.NameToLayer("currentWeapon");
                weapon.GetComponent<Collider>().enabled = false;
                weapon.tag = "currentWeapon";
                transform.parent = weaponHolder.transform.parent;
                curWeapon = GameObject.FindWithTag("currentWeapon");
            }
        }
        //Debug.Log(weaponHolder.name);
    }
    public void UpdateCharacter(GameObject currentPlayer)
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
        DontDestroyOnLoad(curWeapon);
    }
    /*void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentCharacter.transform.position, 5f);
    }*/
}
