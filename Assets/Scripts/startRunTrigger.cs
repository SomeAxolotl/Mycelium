using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startRunTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject characterSelectUI;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");

        if(other.CompareTag("Player"))
        {
            characterSelectUI.SetActive(true);
        }
    }
}
