using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UndergroundTrigger : MonoBehaviour
{
    AudioMixerSnapshot normalSnapshot;
    [SerializeField] AudioMixerSnapshot undergroundSnapshot;
    [SerializeField] float snapshotTransitionSeconds = 0.5f;

    WeatherSettingsScript weatherSettings;

    void Start()
    {
        weatherSettings = GameObject.Find("WeatherSettings").GetComponent<WeatherSettingsScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "currentPlayer" && (weatherSettings.isRaining || SceneManager.GetActiveScene().name != "The Carcass"))
        {
            normalSnapshot = GlobalData.currentUnpausedAudioMixerSnapshot;

            undergroundSnapshot.TransitionTo(snapshotTransitionSeconds);
            GlobalData.currentUnpausedAudioMixerSnapshot = undergroundSnapshot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "currentPlayer" && (weatherSettings.isRaining || SceneManager.GetActiveScene().name != "The Carcass"))
        {
            normalSnapshot.TransitionTo(snapshotTransitionSeconds);
            GlobalData.currentUnpausedAudioMixerSnapshot = normalSnapshot;
        }
    }
}
