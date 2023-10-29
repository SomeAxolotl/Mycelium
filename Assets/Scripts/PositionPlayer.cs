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
           player.transform.position = new Vector3(-1.5f, 1.5f, 15f);
            player.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            //Debug.Log(player.transform.position);
        }
    }
}
