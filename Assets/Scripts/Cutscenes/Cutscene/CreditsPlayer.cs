using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.TimeZoneInfo;

public class CreditsPlayer : MonoBehaviour
{
    [Header("==Credits Section==")]
    [SerializeField] private CanvasGroup blackCanvas;
    [SerializeField] private CanvasGroup creditsCanvas;
    [SerializeField] private CanvasGroup skipCanvas;

    [SerializeField] private RectTransform movePoint;

    [SerializeField] private float fadeTimeBlackCanvas;
    [SerializeField] private float fadeTimeCreditsCanvas;
    [SerializeField][Tooltip("Time in seconds")] private float creditsTime;
    [SerializeField] private float pauseTime;

    private PlayerController playerController;
    private bool creditsIsOn = false;
    private bool askSkipIsOn = false;

    [Header("==End Of Run Section==")]
    [SerializeField] private CanvasGroup endOfRunCanvas;
    [SerializeField] private float fadeTimeEndOfRunCanvas;

    private void Start()
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
    }

    private void OnSkip()
    {
        if (creditsIsOn == false)
        {
            return;
        }

        if (askSkipIsOn == true)
        {
            ConfirmSkip();
            return;
        }

        StartCoroutine(AskSkip());
    }

    public void StartPlayCredits()
    {
        GlobalData.isAbleToPause = false;
        playerController.DisableController();

        StartCoroutine(PlayCredits());
    }

    private void ConfirmSkip()
    {
        StopAllCoroutines();

        StartCoroutine(EndOfRun());
    }

    public void LoopRun()
    {

    }

    public void ReturnToCarcass()
    {
        SceneLoader.Instance.BeginLoadScene("The Carcass", true);
    }

    IEnumerator AskSkip()
    {
        askSkipIsOn = true;

        yield return StartCoroutine(FadeIn(skipCanvas, 0.3f));

        yield return new WaitForSecondsRealtime(2f);

        yield return StartCoroutine(FadeOut(skipCanvas, 0.3f));

        askSkipIsOn = false;
    }

    IEnumerator PlayCredits()
    {
        creditsIsOn = true;

        yield return StartCoroutine(FadeIn(blackCanvas, fadeTimeBlackCanvas));

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeIn(creditsCanvas, fadeTimeCreditsCanvas));

        yield return new WaitForSeconds(pauseTime);

        yield return StartCoroutine(MoveText());

        yield return new WaitForSeconds(pauseTime);

        yield return StartCoroutine(FadeOut(creditsCanvas, fadeTimeCreditsCanvas));

        yield return new WaitForSeconds(1f);

        StartCoroutine(EndOfRun());

        creditsIsOn = false;
    }

    IEnumerator EndOfRun()
    {
        if(askSkipIsOn == true)
        {
            StartCoroutine(FadeOut(creditsCanvas, 0.5f));
            yield return StartCoroutine(FadeOut(skipCanvas, 0.5f));

            creditsIsOn = false;
            askSkipIsOn = false;
        }

        yield return StartCoroutine(FadeIn(endOfRunCanvas, fadeTimeEndOfRunCanvas));
        GameObject.Find("ContinueButton").GetComponent<Button>().Select();
    }

    IEnumerator MoveText()
    {
        float elapsedTime = 0f;
        float t = 0f;

        while (elapsedTime < creditsTime)
        {
            t = elapsedTime / creditsTime;

            movePoint.localPosition = new Vector3(0f, Mathf.Lerp(0f, 2381f, t), 0f);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }
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
