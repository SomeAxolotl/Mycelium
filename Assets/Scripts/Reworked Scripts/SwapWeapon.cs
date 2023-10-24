using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapItem = playerActionsAsset.Player.SwapItem;
        swapCharacter = GetComponent<SwapCharacter>();
        GameObject currentCharacter = swapCharacter.characters[swapCharacter.currentCharacterIndex];
        Transform weaponHolder =  currentCharacter.transform.GetChild(0);
        if(GameObject.FindWithTag("currentWeapon") == null)
        {
        Instantiate(Resources.Load("StartWeapon"), GameObject.FindWithTag("currentPlayer").transform);
        GameObject.FindWithTag("currentWeapon").transform.position = weaponHolder.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentCharacter = swapCharacter.characters[swapCharacter.currentCharacterIndex];
        weaponHolder = currentCharacter.transform.GetChild(0);
        GameObject.FindWithTag("currentWeapon").transform.position = weaponHolder.position;
        GameObject.FindWithTag("currentWeapon").transform.rotation = currentCharacter.transform.rotation;
        weaponColliders = Physics.OverlapSphere(currentCharacter.transform.position, 4f, weaponLayer);
        foreach (var weaponCollider in weaponColliders)
        {
            weapon = weaponCollider.transform;
            Vector3 dirToWeapon = (weapon.position - currentCharacter.transform.position).normalized;
            if (Vector3.Angle(currentCharacter.transform.forward, dirToWeapon) <= 40 && swapItem.triggered)
            {
                Debug.Log("swap");
                GameObject.FindWithTag("currentWeapon").transform.position = weapon.position;
                GameObject.FindWithTag("currentWeapon").transform.rotation = Quaternion.Euler(-25, 0, 0);
                GameObject.FindWithTag("currentWeapon").GetComponent<Collider>().enabled = true;
                weapon.position = weaponHolder.position;
                GameObject.FindWithTag("currentWeapon").layer = LayerMask.NameToLayer("Weapon");
                GameObject.FindWithTag("currentWeapon").tag = "Weapon";
                weapon.gameObject.layer = LayerMask.NameToLayer("currentWeapon");
                weapon.GetComponent<Collider>().enabled = false;
                weapon.tag = "currentWeapon";
                transform.parent = GameObject.FindWithTag("currentPlayer").transform.parent;
            }
        }
    }
    /*void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentCharacter.transform.position, 5f);
    }*/
}
