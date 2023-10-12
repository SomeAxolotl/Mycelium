using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSpawnPoint : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");
        Debug.Log(transform.position);
        player.transform.position = transform.position;
    }
}
