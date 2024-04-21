using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrumCurio : Curio
{
    [SerializeField] List<DanceCurio> danceCurios = new List<DanceCurio>();

    [SerializeField] float minPlayingTime = 10f;
    [SerializeField] float maxPlayingTime = 15f;
    [SerializeField] float hitTimeOffset = 35000f;
    [SerializeField] float hitTimeOffsetSpeedScalar = 0.5f;
    [SerializeField] float waveTime = 3f;

    public float playingCounter {get; private set;} = 0f;
    public float randomPlayTime {get; private set;}

    int lastBoing = -1;
    int lastHit = -1;
    float lastBoingSampledTime = -1;
    float lastHitSampledTime = -1;

    const float carcassSongBPM = 93.9f;

    public event Action OnPlayingStarted;

    void OnEnable()
    {
        foreach (DanceCurio danceCurio in danceCurios)
        {
            OnPlayingStarted += danceCurio.OpenToDancing;
            OnPlayingDone += danceCurio.CloseToDancing;
        }
    }

    void OnDisable()
    {
        foreach (DanceCurio danceCurio in danceCurios)
        {
            OnPlayingStarted -= danceCurio.OpenToDancing;
            OnPlayingDone -= danceCurio.CloseToDancing;
        }
    }

    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        wanderingSpore.rb.constraints = RigidbodyConstraints.FreezeAll;

        wanderingSpore.lookTarget = transform.position - wanderingSpore.transform.position;

        AudioSource carcassAudioSource = GameObject.Find("BackgroundMusicPlayer").GetComponent<AudioSource>();
        int speedLevel = wanderingSpore.GetComponent<CharacterStats>().speedLevel;

        OnPlayingStarted?.Invoke();

        float randomPlayTime = UnityEngine.Random.Range(minPlayingTime, maxPlayingTime);
        float playingCounter = 0f;
        while (playingCounter < randomPlayTime)
        {
            float boingSampledTime = carcassAudioSource.timeSamples / (carcassAudioSource.clip.frequency * (60f / (carcassSongBPM / 3.5f)));
            float hitSampledTime = (carcassAudioSource.timeSamples + (hitTimeOffset / (speedLevel * hitTimeOffsetSpeedScalar))) / (carcassAudioSource.clip.frequency * (60f / (carcassSongBPM / 3.5f)));

            if (Mathf.FloorToInt(boingSampledTime) != lastBoing)
            {
                Debug.Log(playingCounter);
                if (lastBoing != -1 && lastBoingSampledTime != -1 && boingSampledTime - lastBoingSampledTime > 0 && playingCounter > 0.7f)
                {
                    SoundEffectManager.Instance.PlaySound("Impact", transform.position);
                    SoundEffectManager.Instance.PlaySound("DrumSmack", transform.position);
                }

                lastBoing = Mathf.FloorToInt(boingSampledTime);
                lastBoingSampledTime = boingSampledTime;
            }

            if (Mathf.FloorToInt(hitSampledTime) != lastHit)
            {
                if (lastHit != -1 && lastHitSampledTime != -1 && hitSampledTime - lastHitSampledTime > 0)
                {   
                    wanderingSpore.animator.SetTrigger("DrumHit");
                }

                lastHit = Mathf.FloorToInt(hitSampledTime);
                lastHitSampledTime = hitSampledTime;
            }

            playingCounter += Time.deltaTime;

            yield return null;
        }

        wanderingSpore.animator.SetTrigger("Wave");

        yield return new WaitForSeconds(waveTime);
    }
}
