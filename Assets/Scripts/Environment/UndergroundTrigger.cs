using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UndergroundTrigger : MonoBehaviour
{
    AudioMixerSnapshot normalSnapshot;
    [SerializeField] AudioMixerSnapshot undergroundSnapshot;
    [SerializeField] float snapshotTransitionSeconds = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "currentPlayer")
        {
            normalSnapshot = GlobalData.currentUnpausedAudioMixerSnapshot;

            undergroundSnapshot.TransitionTo(snapshotTransitionSeconds);
            GlobalData.currentUnpausedAudioMixerSnapshot = undergroundSnapshot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "currentPlayer")
        {
            normalSnapshot.TransitionTo(snapshotTransitionSeconds);
            GlobalData.currentUnpausedAudioMixerSnapshot = normalSnapshot;
        }
    }
}
