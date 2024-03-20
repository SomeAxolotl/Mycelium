using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackCavnas;
    [SerializeField] private List<CutsceneCanvasSettings> canvasOrder;

    [SerializeField] private float transitionTime;
    [SerializeField] private float textInbetweenTime;
    [SerializeField] private float textReadTime;

    [HideInInspector] public bool isFinished = false;

    private void Start()
    {
        
    }

    public void StartCutscene()
    {
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(FadeIn(blackCavnas, 1f));

        yield return new WaitForSeconds(0.5f);

        foreach (CutsceneCanvasSettings ccs in canvasOrder)
        {
            ccs.StartPlaying(transitionTime, textInbetweenTime, textReadTime);
            while (ccs.isFinished == false)
            {
                yield return null;
            }
        }

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
}
