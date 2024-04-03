using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private CanvasGroup blackCavnas;
    [SerializeField] private CanvasGroup skipCanvas;
    [SerializeField] private List<CutsceneCanvasSettings> canvasOrder;

    [SerializeField] private float canvasFadeTime;
    [SerializeField] private float canvasInbetweenTime;
    [SerializeField] private float textFadeTime;
    [SerializeField] private float textInbetweenTime;
    [SerializeField] private float textReadTime;
    [SerializeField] private float audioFadeTime;
    [SerializeField] private float cutsceneVolume;

    [HideInInspector] public bool isFinished = false;
    private AudioSource cutsceneMusic;
    private PlayerController playerController;
    private ThirdPersonActionsAsset playerInput;
    private bool cutsceneIsOn;
    private bool askSkipIsOn;

    private void Start()
    {
        cutsceneMusic = GetComponent<AudioSource>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerInput = InputManager.actionAsset;
    }

    private void Update()
    {
        if (cutsceneIsOn == false)
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

    public void StartCutscene()
    {
        GlobalData.isAbleToPause = false;
        playerController.DisableController();
        playerInput.Enable();

        StartCoroutine(Cutscene());
    }

    private void ConfirmSkip()
    {
        StopAllCoroutines();

        isFinished = true;
        cutsceneIsOn = false;
    }

    IEnumerator Cutscene()
    {
        cutsceneIsOn = true;

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
        cutsceneIsOn = false;
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

            audioSource.volume = Mathf.Lerp(0f, cutsceneVolume, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        audioSource.volume = cutsceneVolume;
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

    IEnumerator FadeOut(AudioSource audioSource, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        audioSource.volume = cutsceneVolume;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            audioSource.volume = Mathf.Lerp(cutsceneVolume, 0f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        audioSource.volume = 0f;
    }
}
