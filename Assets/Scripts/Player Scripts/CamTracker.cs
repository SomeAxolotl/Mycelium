using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTracker : MonoBehaviour
{
    private GameObject currentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayer = GameObject.FindWithTag("currentPlayer");
        transform.position = currentPlayer.transform.position;
    }
}
