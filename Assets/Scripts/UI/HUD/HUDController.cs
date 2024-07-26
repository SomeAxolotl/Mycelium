using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    [SerializeField] private float hudFadeTransitionTime = 0.25f;
    [SerializeField] private float slideTransitionTime = 0.25f;
    [SerializeField] private float blackCanvasFadeTransitionTime = 0.25f;

    Coroutine currentFadeCoroutine = null;
    Coroutine currentBlackFadeCoroutine = null;

    [SerializeField] CanvasGroup blackCanvasGroup;

    Dictionary<string, Coroutine> slideCoroutines = new Dictionary<string, Coroutine>();

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Playground")
        {
            GetComponent<CanvasGroup>().alpha = 0f;
        }
    }

    public void SlideHUDElement(string coroutineKey, RectTransform element, RectTransform toTarget)
    {
        if (slideCoroutines.TryGetValue(coroutineKey, out Coroutine currentCoroutine))
        {
            StopCoroutine(currentCoroutine);
            slideCoroutines.Remove(coroutineKey);
        }

        Coroutine newCoroutine = StartCoroutine(SlideHUDElementCoroutine(element, toTarget));
        slideCoroutines[coroutineKey] = newCoroutine;
    }

    void OnEnable()
    {
        StartCoroutine(SubscribeOnDelay());
    }
    IEnumerator SubscribeOnDelay()
    {
        yield return null;

        SceneLoader.Instance.OnTitleCardFinished += FadeHUDAfterTitleCard;
    }
    void OnDisable()
    {
        SceneLoader.Instance.OnTitleCardFinished -= FadeHUDAfterTitleCard;
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

    void FadeHUDAfterTitleCard()
    {
        FadeHUD(true);
    }

    public void FadeHUD(bool doesFadeIn, float transitionTime = -1f)
    {
        if (transitionTime < 0)
        {
            transitionTime = hudFadeTransitionTime;
        }

        CanvasGroup hudCanvasGroup = GetComponent<CanvasGroup>();

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeCanvas(hudCanvasGroup, doesFadeIn, transitionTime));
    }

    public IEnumerator BlackFade(bool doesFadeIn, float transitionTime = -1f)
    {
        if (transitionTime < 0)
        {
            transitionTime = blackCanvasFadeTransitionTime;
        }

        if (currentBlackFadeCoroutine != null)
        {
            StopCoroutine(currentBlackFadeCoroutine);
        }

        currentBlackFadeCoroutine = StartCoroutine(FadeCanvas(blackCanvasGroup, doesFadeIn, transitionTime));

        yield return currentBlackFadeCoroutine;
    }

    IEnumerator FadeCanvas(CanvasGroup canvasGroup, bool doesFadeIn, float transitionTime)
    {
        if (doesFadeIn)
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
        }
        else
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
}
