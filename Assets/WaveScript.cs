using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Update is called once per frame
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("currentPlayer"))
        {
            int randInt = Random.Range(0,9);
            Debug.Log("RandInt: " + randInt);
            if(randInt >= 5){
                animator.SetTrigger("Wave");}
        }
    }
}
