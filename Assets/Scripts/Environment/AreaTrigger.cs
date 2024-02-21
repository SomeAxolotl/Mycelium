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
    [SerializeField] private List<AudioTrigger> AudioFadeoutEvents;

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
            Destroy(this);
        }
    }

}

[System.Serializable]
public class AudioTrigger
{
    [SerializeField] private UnityEngine.AudioClip audioClip;
    [SerializeField][Range(0,2)] private float volume = 1;
    [SerializeField][Range(0,3)] private float fadeLength = 1;
    [SerializeField] private AudioSource audioSource;
    public void PlayAudioEvent(){
        if(audioClip != null){
            audioSource.PlayOneShot(audioClip);
            SlideVolume(volume);
        }
    }

    public void StopAudioEvent(){
        if(audioClip != null){
            audioSource.PlayOneShot(audioClip);
            SlideOffVolume();
        }
    }

    IEnumerator SlideVolume(float targetVolume)
    {
        for (float t = 0f; t <= 1; t += Time.deltaTime) {
			audioSource.volume = Mathf.Lerp(0, targetVolume, t);
			yield return null;
        }
    }

    IEnumerator SlideOffVolume()
    {
        float startVolume = audioSource.volume;
        for (float t = 0f; t <= 1; t += Time.deltaTime) {
			audioSource.volume = Mathf.Lerp(startVolume, 0, t);
			yield return null;
        }
    }
}

public class Teleport
{
    [SerializeField] private UnityEngine.Vector3 NewLocation;
    [SerializeField] private GameObject Object;

    public void TeleportObj(){
        Object.transform.position = NewLocation;
    }
}
