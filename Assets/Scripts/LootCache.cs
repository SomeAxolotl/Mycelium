using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;


public class LootCache : MonoBehaviour
{
    float distance;
    float holdTime;
    [SerializeField] private List<GameObject> items;
    PlayerController controller;
    GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        if (Input.GetKey(KeyCode.E) && distance < 3)
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
        }
    }
    public void GetLoot()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber <= 34)
        {
            Instantiate(items[0], transform.position, Quaternion.identity);
        }
        if (randomNumber >= 35 && randomNumber <= 39)
        {
            Instantiate(items[1], transform.position, Quaternion.identity);
        }
        if (randomNumber >= 40 && randomNumber <= 54)
        {
            Instantiate(items[2], transform.position, Quaternion.identity);
        }
        if (randomNumber >= 55 && randomNumber <= 69)
        {
            Instantiate(items[3], transform.position, Quaternion.identity);
        }
        if (randomNumber >= 70 && randomNumber <= 79)
        {
            Instantiate(items[4], transform.position, Quaternion.identity);
        }
        if (randomNumber >= 80 && randomNumber <= 99)
        {
            Instantiate(items[5], transform.position, Quaternion.identity);
        }
    }

   
}
