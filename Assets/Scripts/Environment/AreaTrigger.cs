using Cinemachine;
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
    // [SerializeField] private List<Teleport> TeleportEvent;
    [SerializeField] private List<AudioTrigger> AudioFadeoutEvents;

    private void OnTriggerEnter(Collider other){
        if(other.tag == "currentPlayer"){
            if(GameObjectsToActivate.Count != 0)
                foreach (GameObject gameObject in GameObjectsToActivate)
                    gameObject.SetActive(true);
            if(GameObjectsToDeactivate.Count != 0)
                foreach (GameObject gameObject in GameObjectsToDeactivate)
                    gameObject.SetActive(false);
            if(AudioTriggerEvents.Count != 0)
                foreach (AudioTrigger audioTrigger in AudioTriggerEvents){
                    audioTrigger.PlayAudioEvent();
                    StartCoroutine(audioTrigger.SlideVolume());
                }
            //if(TeleportEvent.Count != 0)
              //  foreach (Teleport teleport in TeleportEvent)
                //    teleport.TeleportObj();
            if(AudioFadeoutEvents.Count != 0)
                foreach (AudioTrigger audioFade in AudioFadeoutEvents)
                    StartCoroutine(audioFade.SlideOffVolume());

            if(GetComponent<CinemachineImpulseSource>() != null)
            {
                StartCoroutine(WaitThenShake(0.65f));
            }

            StartCoroutine(DestroySelf());
        }
    }

    private IEnumerator WaitThenShake(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        CameraShakeManager.instance.ShakeCamera(GetComponent<CinemachineImpulseSource>());
    }

    private IEnumerator DestroySelf(){
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }

}

[System.Serializable]
public class AudioTrigger
{
    [SerializeField] private UnityEngine.AudioClip audioClip;
    [SerializeField][Range(0,2)] private float volume;
    [SerializeField][Range(0,3)] private float fadeLength;
    [SerializeField] private AudioSource audioSource;
    public void PlayAudioEvent(){
        if(audioClip != null){
            audioSource.PlayOneShot(audioClip);
        }
    }

    public IEnumerator SlideVolume()
    {
        for (float t = 0f; t <= 1; t += Time.deltaTime) {
			audioSource.volume = Mathf.Lerp(0, volume, t);
			yield return null;
        }
    }

    public IEnumerator SlideOffVolume()
    {
        float startVolume = audioSource.volume;
        for (float t = 0f; t <= 1; t += Time.deltaTime) {
			audioSource.volume = Mathf.Lerp(startVolume, 0, t);
			yield return null;
        }
    }
}

// public class Teleport
// {
//     [SerializeField] private UnityEngine.Vector3 NewLocation = new UnityEngine.Vector3(0,0,0);
//     [SerializeField] private GameObject Object = null;

//     public void TeleportObj(){
//         Object.transform.position = NewLocation;
//     }
// }
