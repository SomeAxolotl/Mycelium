using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class CreditsPlayer : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackCanvas;
    [SerializeField] private CanvasGroup creditsCanvas;
    [SerializeField] private CanvasGroup skipCanvas;

    [SerializeField] private RectTransform movePoint;

    [SerializeField] private float fadeTimeBlackCanvas;
    [SerializeField] private float fadeTimeCreditsCanvas;
    [SerializeField][Tooltip("Time in seconds")] private float creditsTime;
    [SerializeField] private float pauseTime;

    private PlayerController playerController;
    private Coroutine credits;
    private Coroutine askSkip;
    private ThirdPersonActionsAsset playerInput;

    private void Start()
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerInput = new ThirdPersonActionsAsset();
        playerInput.Disable();
    }
    
    private void Update()
    {
        if(credits == null)
        {
            return;
        }

        if (askSkip != null)
        {
            return;
        }

        if (playerInput.Cutscene.Skip.WasPressedThisFrame())
        {
            askSkip = StartCoroutine(AskSkip());
        }
    }

    public void StartPlayCredits()
    {
        GlobalData.isAbleToPause = false;
        playerController.DisableController();
        playerInput.Enable();

        credits = StartCoroutine(PlayCredits());
    }

    private void ConfirmSkip()
    {
        StopAllCoroutines();

        SceneLoader.Instance.BeginLoadScene("The Carcass", true);
    }

    IEnumerator PlayCredits()
    {
        yield return StartCoroutine(FadeIn(blackCanvas, fadeTimeBlackCanvas));

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeIn(creditsCanvas, fadeTimeCreditsCanvas));

        yield return new WaitForSeconds(pauseTime);

        yield return StartCoroutine(MoveText());

        yield return new WaitForSeconds(pauseTime);

        yield return StartCoroutine(FadeOut(creditsCanvas, fadeTimeCreditsCanvas));

        yield return new WaitForSeconds(1f);

        SceneLoader.Instance.BeginLoadScene("The Carcass", true);
    }

    IEnumerator AskSkip()
    {
        yield return StartCoroutine(FadeIn(skipCanvas, 0.3f));

        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            if (playerInput.Cutscene.Skip.WasPressedThisFrame())
            {
                ConfirmSkip();
            }

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        StartCoroutine(FadeOut(skipCanvas, 0.3f));

        askSkip = null;
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

        yield break;
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
