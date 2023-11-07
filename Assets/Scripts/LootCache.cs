using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;


public class LootCache : MonoBehaviour
{
    float distance;
    float holdTime;
    [SerializeField] private List<GameObject> items;
    ThirdPersonActionsAsset playerActionsAsset;
    private InputAction interact;
    GameObject player;
    private void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        interact = playerActionsAsset.Player.Interact;
        player = GameObject.FindWithTag("currentPlayer");
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (interact.triggered && distance < 3)
        {
            GetLoot();
            Destroy(this.gameObject);
        }
        /*if (Input.GetKey(KeyCode.E) && distance < 3)
        {
            Debug.Log("Working");
            holdTime += Time.deltaTime;
            if (holdTime >= 1f)
            {
                holdTime = 0f;
                GetLoot();
                Destroy(this.gameObject);
            }
        }
        else
        {
            holdTime = 0f;
        }*/
    }
    public void GetLoot()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber <= 34)
        {
            Instantiate(items[0], new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z), Quaternion.identity);
        }
        if (randomNumber >= 35 && randomNumber <= 39)
        {
            Instantiate(items[1], new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z), Quaternion.identity);
        }
        if (randomNumber >= 40 && randomNumber <= 54)
        {
            Instantiate(items[2], new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z), Quaternion.identity);
        }
        if (randomNumber >= 55 && randomNumber <= 69)
        {
            Instantiate(items[3], new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z), Quaternion.identity);
        }
        if (randomNumber >= 70 && randomNumber <= 79)
        {
            Instantiate(items[4], new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z), Quaternion.identity);
        }
        if (randomNumber >= 80 && randomNumber <= 99)
        {
            Instantiate(items[5], new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z), Quaternion.identity);
        }
    }

   
}
