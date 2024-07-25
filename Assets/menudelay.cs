using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menudelay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
    audioSource.PlayDelayed( 3 );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
