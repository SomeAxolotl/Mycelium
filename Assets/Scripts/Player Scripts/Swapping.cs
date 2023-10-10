using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Swapping : MonoBehaviour
{
    Rigidbody rb;
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction swapItem;
    private Transform weaponHolder;
    public bool swapping = false;
    public bool playerSwapping = false;
    private HUDWeapon hudWeaponScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapItem = playerActionsAsset.Player.SwapItem;
        weaponHolder = this.gameObject.transform.GetChild(2);
        if(GameObject.FindWithTag("currentWeapon") == null)
        {
        Instantiate(Resources.Load("StartWeapon"));
        GameObject.FindWithTag("currentWeapon").transform.position = weaponHolder.position;
        }

        hudWeaponScript = GameObject.Find("HUD").GetComponent<HUDWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.FindWithTag("currentWeapon").transform.position = weaponHolder.position;
        GameObject.FindWithTag("currentWeapon").transform.rotation = transform.rotation;
        
        Vector3 direction = rb.velocity;
        if(Mathf.Approximately(rb.velocity.x, 0) && Mathf.Approximately(rb.velocity.z, 0))
        {
            direction = transform.forward * 5f;
        }
        direction.y = 0f;
        
        Debug.DrawRay(transform.position, direction, Color.red, 2);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), direction, Color.blue, 2);
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), direction, Color.green, 2);
        
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, 8f) || 
        Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), direction, out hit, 8f) || 
        Physics.Raycast(new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), direction, out hit, 8f))
        {
            if(hit.transform.CompareTag("Player") && swapItem.triggered)
            {
                //Swaps your current player
                this.transform.gameObject.tag = "Player";
                hit.transform.gameObject.tag = "currentPlayer";
                this.transform.GetChild(1).gameObject.SetActive(false);
                this.transform.GetChild(2).gameObject.SetActive(false);
                hit.transform.GetChild(1).gameObject.SetActive(true);
                hit.transform.GetChild(2).gameObject.SetActive(true);
                hit.transform.gameObject.GetComponent<PlayerController>().enabled = true;
                hit.transform.gameObject.GetComponent<MeleeAttack>().enabled = true;
                hit.transform.gameObject.GetComponent<Swapping>().enabled = true;
                hit.transform.gameObject.GetComponent<StatTracker>().enabled = true;
                hit.transform.gameObject.GetComponent<PlayerHealth>().enabled = true;
                this.transform.gameObject.GetComponent<PlayerController>().enabled = false;
                this.transform.gameObject.GetComponent<MeleeAttack>().enabled = false;
                this.transform.gameObject.GetComponent<StatTracker>().enabled = false;
                this.transform.gameObject.GetComponent<PlayerHealth>().enabled = false;
                this.transform.gameObject.GetComponent<Swapping>().enabled = false;
                playerSwapping = true;
            }
            else
            {
                playerSwapping = false;
            }
            
            if(hit.transform.CompareTag("Weapon") && swapItem.triggered)
            {
                //Swaps your current weapon
                GameObject.FindWithTag("currentWeapon").transform.position = hit.transform.position;
                GameObject.FindWithTag("currentWeapon").GetComponent<Collider>().enabled = true;
                GameObject.FindWithTag("currentWeapon").transform.rotation = Quaternion.Euler(-25, 0, 0);
                hit.transform.position = weaponHolder.position;
                GameObject.FindWithTag("currentWeapon").tag = "Weapon";
                hit.collider.gameObject.tag = "currentWeapon";
                transform.parent = GameObject.FindWithTag("currentPlayer").transform.parent;
                swapping = true;

                //Updates HUD weapon icon
                hudWeaponScript.UpdateWeapon(hit.collider.gameObject.name);
            }
            else
            {
                swapping = false;
            }
        }
    }
}
