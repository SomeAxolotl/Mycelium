using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTargetMovement : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("currentPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.5f);
    }
}
