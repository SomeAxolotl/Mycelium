using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private float hudFadeTransitionTime = 0.25f;
    [SerializeField] private float slideTransitionTime = 0.25f;

    public void SlideHUDElement(RectTransform element, RectTransform toTarget)
    {
        StopAllCoroutines();
        StartCoroutine(SlideHUDElementCoroutine(element, toTarget));
    }

    IEnumerator SlideHUDElementCoroutine(RectTransform element, RectTransform toTarget)
    {
        float elapsedTime = 0f;
        float t;

        while (elapsedTime < slideTransitionTime)
        {
            t = DylanTree.EaseOutQuart(elapsedTime / slideTransitionTime);

            element.localPosition = Vector3.Lerp(element.localPosition, toTarget.localPosition, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        element.localPosition = toTarget.localPosition;
    }


    public void FadeInHUD()
    {
        CanvasGroup hudCanvasGroup = GetComponent<CanvasGroup>();

        StopAllCoroutines();
        StartCoroutine(FadeCanvasIn(hudCanvasGroup, hudFadeTransitionTime));
    }

    public void FadeOutHUD()
    {
        CanvasGroup hudCanvasGroup = GetComponent<CanvasGroup>();

        StopAllCoroutines();
        StartCoroutine(FadeCanvasOut(hudCanvasGroup, hudFadeTransitionTime));
    }

    IEnumerator FadeCanvasIn(CanvasGroup canvasGroup, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        float currentCanvasAlpha = canvasGroup.alpha;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            canvasGroup.alpha = Mathf.Lerp(currentCanvasAlpha, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        /*try
        {
            GameObject.Find("PauseMenuCanvas").GetComponent<PauseMenu>().Resume();
        }
        catch
        {
            
        }*/
    }

    IEnumerator FadeCanvasOut(CanvasGroup canvasGroup, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        float currentCanvasAlpha = canvasGroup.alpha;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            canvasGroup.alpha = Mathf.Lerp(currentCanvasAlpha, 0f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
