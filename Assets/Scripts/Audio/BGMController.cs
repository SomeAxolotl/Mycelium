using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMController : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] float fadeInTime = 0.25f;
    [SerializeField] float fadeOutTime = 2.5f;

    void Start()
    {
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeInMusicCoroutine()
    {
        float originalSourceVolume = musicSource.volume;

        musicSource.volume = 0f;

        float fadeTimer = 0f;
        while (fadeTimer < fadeInTime)
        {
            float t = fadeTimer / fadeInTime;

            musicSource.volume = Mathf.Lerp(0f, originalSourceVolume, t);

            fadeTimer += Time.deltaTime;

            yield return null;
        }

        musicSource.volume = originalSourceVolume;
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
}
