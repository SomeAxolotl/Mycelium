using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AreaTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> GameObjectsToActivate;
    [SerializeField] private List<GameObject> GameObjectsToDeactivate;
    [SerializeField] private List<AudioTrigger> AudioTriggerEvents;
    [SerializeField] private List<Teleport> TeleportEvent;

    private void OnTriggerEnter(Collider other){
        if(other.tag == "currentPlayer"){
            foreach (GameObject gameObject in GameObjectsToActivate)
                gameObject.SetActive(true);
            foreach (GameObject gameObject in GameObjectsToDeactivate)
                gameObject.SetActive(false);
            foreach (AudioTrigger audioTrigger in AudioTriggerEvents)
                audioTrigger.PlayAudioEvent();
            foreach (Teleport teleport in TeleportEvent)
                teleport.TeleportObj();
        }
    }

}

[System.Serializable]
public class AudioTrigger
{
    [SerializeField] private UnityEngine.AudioClip audioClip;
    [SerializeField][Range(0,2)] private float volume = 1;
    [SerializeField] private AudioSource audioSource;
    public void PlayAudioEvent(){
        if(audioClip != null)
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
