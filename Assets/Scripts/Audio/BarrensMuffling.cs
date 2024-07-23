using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BarrensMuffling : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] BGMController bgmController;
    [SerializeField] AudioSource musicSource;

    [Header("Fields")]
    [SerializeField] AudioMixerSnapshot musicMuffleSnapshot;
    [SerializeField] AudioMixerSnapshot bossSnapshot;

    [SerializeField] int playbackSamples;
    [SerializeField] float snapshotTransitionSeconds;

    //for testing
    public int musicTimeSamples;

    IEnumerator Start()
    {
        yield return null;

        musicMuffleSnapshot.TransitionTo(0f);
        GlobalData.currentUnpausedAudioMixerSnapshot = musicMuffleSnapshot;
    }

    public void StopMuffledMusic()
    {
        bgmController.StartCoroutine(bgmController.FadeOutMusicCoroutine());
    }

    public void StartBossMusic()
    {
        Debug.Log("starting boss music");

        musicSource.timeSamples = playbackSamples;
        musicSource.Play();

        bgmController.StartCoroutine(bgmController.FadeInMusicCoroutine());

        bossSnapshot.TransitionTo(snapshotTransitionSeconds);
        GlobalData.currentUnpausedAudioMixerSnapshot = bossSnapshot;
    }

    void Update()
    {
        musicTimeSamples = musicSource.timeSamples;
    }
}
