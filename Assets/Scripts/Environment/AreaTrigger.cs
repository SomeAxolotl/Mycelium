using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;

public class AreaTrigger : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class AudioEventThing
{
    [SerializeField] private UnityEngine.AudioClip audioClip;
    [SerializeField][Range(0,2)] private float volume;

    [SerializeField] private AudioSource audioSource;

    public void PlayAudioEvent(){
        audioSource.PlayOneShot(audioClip, volume);
    }

    
}

[System.Serializable]
public class Teleport
{
    [SerializeField] private UnityEngine.Vector3 NewLocation;
    [SerializeField] private GameObject Object;

    public void TeleportObj(){
        Object.transform.position = NewLocation;
    }
}
