using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("currentPlayer").transform;
        transform.position = new Vector3(player.position.x, 0, player.position.z);
    }
}
