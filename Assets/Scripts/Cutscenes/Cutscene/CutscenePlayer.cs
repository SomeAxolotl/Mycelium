using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackCavnas;
    [SerializeField] private List<CutsceneCanvasSettings> canvasOrder;

    [SerializeField] private float canvasFadeTime;
    [SerializeField] private float canvasInbetweenTime;
    [SerializeField] private float textFadeTime;
    [SerializeField] private float textInbetweenTime;
    [SerializeField] private float textReadTime;
    [SerializeField] private float audioFadeTime;

    [HideInInspector] public bool isFinished = false;
    private AudioSource cutsceneMusic;

    private IEnumerator Start()
    {
        cutsceneMusic = GetComponent<AudioSource>();

        yield return new WaitForSeconds(5f);
        //StartCutscene();
    }

    public void StartCutscene()
    {
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene()
    {
        GlobalData.isAbleToPause = false;

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(FadeIn(blackCavnas, 1f));

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(FadeIn(cutsceneMusic, audioFadeTime));

        foreach (CutsceneCanvasSettings ccs in canvasOrder)
        {
            ccs.StartPlaying(canvasFadeTime, canvasInbetweenTime, textFadeTime, textInbetweenTime, textReadTime);
            while (ccs.isFinished == false)
            {
                yield return null;
            }
        }

        StartCoroutine(FadeOut(cutsceneMusic, audioFadeTime));

        isFinished = true;
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
    
    IEnumerator FadeIn(AudioSource audioSource, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        audioSource.Play();
        audioSource.volume = 0f;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            audioSource.volume = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }

    IEnumerator FadeOut(AudioSource audioSource, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        audioSource.volume = 1f;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            audioSource.volume = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        audioSource.volume = 0f;
    }
}
