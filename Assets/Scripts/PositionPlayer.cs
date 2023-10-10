using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPlayer : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");
        if (player)
        {
           player.transform.position = new Vector3(0, 2, 0);
           Debug.Log(player.transform.position);
        }
    }
}
