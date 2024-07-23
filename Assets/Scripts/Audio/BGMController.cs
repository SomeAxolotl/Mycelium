using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMController : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource ambienceSource;
    [SerializeField] float fadeInTime = 0.25f;
    [SerializeField] float fadeOutTime = 2.5f;

    [SerializeField][Tooltip("Defaults to its starting volume (-1). Can be changed")] float musicFadeInTargetValue = -1;
    [SerializeField][Tooltip("Defaults to its starting volume (-1). Can be changed")] float ambienceFadeInTargetValue = -1;

    void Start()
    {
        if (musicFadeInTargetValue == -1)
        {
            musicFadeInTargetValue = musicSource.volume;
        }
        if (ambienceFadeInTargetValue == -1)
        {
            ambienceFadeInTargetValue = ambienceSource.volume;
        }

        StartCoroutine(FadeInMusicCoroutine());
        StartCoroutine(FadeInAmbienceCoroutine());
    }

    //Music
    public IEnumerator FadeInMusicCoroutine()
    {
        musicSource.volume = 0f;

        float fadeTimer = 0f;
        while (fadeTimer < fadeInTime)
        {
            float t = fadeTimer / fadeInTime;

            musicSource.volume = Mathf.Lerp(0f, musicFadeInTargetValue, t);

            fadeTimer += Time.deltaTime;

            yield return null;
        }

        musicSource.volume = musicFadeInTargetValue;
    }

    public IEnumerator FadeOutMusicCoroutine()
    {
        float originalSourceVolume = musicSource.volume;

        float fadeTimer = 0f;
        while (fadeTimer < fadeOutTime)
        {
            float t = fadeTimer / fadeOutTime;

            musicSource.volume = Mathf.Lerp(originalSourceVolume, 0f, t);

            fadeTimer += Time.deltaTime;

            yield return null;
        }

        musicSource.volume = 0f;
    }

    //Ambience
    IEnumerator FadeInAmbienceCoroutine()
    {
        ambienceSource.volume = 0f;

        float fadeTimer = 0f;
        while (fadeTimer < fadeInTime)
        {
            float t = fadeTimer / fadeInTime;

            ambienceSource.volume = Mathf.Lerp(0f, ambienceFadeInTargetValue, t);

            fadeTimer += Time.deltaTime;

            yield return null;
        }

        ambienceSource.volume = ambienceFadeInTargetValue;
    }

    IEnumerator FadeOutAmbienceCoroutine()
    {
        float originalSourceVolume = ambienceSource.volume;

        float fadeTimer = 0f;
        while (fadeTimer < fadeOutTime)
        {
            float t = fadeTimer / fadeOutTime;

            ambienceSource.volume = Mathf.Lerp(originalSourceVolume, 0f, t);

            fadeTimer += Time.deltaTime;

            yield return null;
        }

        ambienceSource.volume = 0f;
    }
}
