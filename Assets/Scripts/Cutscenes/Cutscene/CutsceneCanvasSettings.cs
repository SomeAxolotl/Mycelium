using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutsceneCanvasSettings : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> textOrder;

    [HideInInspector] public bool isFinished = false;

    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        foreach(TMP_Text text in textOrder)
        {
            text.alpha = 0f;
        }
    }

    public void StartPlaying(float transitionTime, float textInbetweenTime, float textReadTime)
    {
        StartCoroutine(Play(transitionTime, textInbetweenTime, textReadTime));
    }

    IEnumerator Play(float transitionTime, float textInbetweenTime, float textReadTime)
    {
        yield return StartCoroutine(FadeIn(canvasGroup, transitionTime));

        foreach (TMP_Text text in textOrder)
        {
            yield return new WaitForSeconds(textInbetweenTime);
            StartCoroutine(FadeIn(text, transitionTime));
        }

        yield return new WaitForSeconds(textReadTime);

        foreach (TMP_Text text in textOrder)
        {
            yield return new WaitForSeconds(textInbetweenTime);
            StartCoroutine(FadeOut(text, transitionTime));
        }

        yield return new WaitForSeconds(textInbetweenTime);

        StartCoroutine(FadeOut(canvasGroup, transitionTime));

        yield return new WaitForSeconds(0.5f);

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

    IEnumerator FadeIn(TMP_Text text, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            text.alpha = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        text.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = 0f;
    }

    IEnumerator FadeOut(TMP_Text text, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            text.alpha = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        text.alpha = 0f;
    }
}
