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
    private bool creditsIsOn = false;
    private bool askSkipIsOn = false;
    private ThirdPersonActionsAsset playerInput;

    private void Start()
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerInput = new ThirdPersonActionsAsset();
        playerInput.Disable();
    }
    
    private void Update()
    {
        if(creditsIsOn == false)
        {
            return;
        }

        if (askSkipIsOn == true)
        {
            return;
        }

        if (playerInput.Cutscene.Skip.WasPressedThisFrame())
        {
            StartCoroutine(AskSkip());
        }
    }

    public void StartPlayCredits()
    {
        GlobalData.isAbleToPause = false;
        playerController.DisableController();
        playerInput.Enable();

        StartCoroutine(PlayCredits());
    }

    private void ConfirmSkip()
    {
        playerInput.Disable();
        StopAllCoroutines();

        SceneLoader.Instance.BeginLoadScene("The Carcass", true);
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

        SceneLoader.Instance.BeginLoadScene("The Carcass", true);

        creditsIsOn = false;
    }

    IEnumerator AskSkip()
    {
        askSkipIsOn = true;
        float elapsedTime = 0f;

        yield return StartCoroutine(FadeIn(skipCanvas, 0.3f));

        while (elapsedTime < 2f)
        {
            if (playerInput.Cutscene.Skip.WasPressedThisFrame())
            {
                ConfirmSkip();
            }

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return StartCoroutine(FadeOut(skipCanvas, 0.3f));

        askSkipIsOn = false;
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
