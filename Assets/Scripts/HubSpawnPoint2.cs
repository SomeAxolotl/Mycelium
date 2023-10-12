using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSpawnPoint2 : MonoBehaviour
{
    void Start()
    {
        Debug.Log("starting");
        GameObject playerObject = GameObject.FindWithTag("currentPlayer");
        playerObject.transform.position = transform.position;
    }
}