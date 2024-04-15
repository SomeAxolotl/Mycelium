using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
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

    [Header("==End Of Run Section==")]
    [SerializeField] private CanvasGroup endOfRunCanvas;
    [SerializeField] private TMP_Text difficultyText;
    [SerializeField] private float fadeTimeEndOfRunCanvas;

    private PlayerController playerController;
    private GameObject camTracker;
    private bool creditsIsOn = false;
    private bool askSkipIsOn = false;

    private void Start()
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        camTracker = GameObject.FindWithTag("Camtracker");
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

        difficultyText.text = "Enemy Stats +" + (GlobalData.currentLoop + 1) * 100 + "%";

        StartCoroutine(EndOfRun());
    }

    private void ConfirmSkip()
    {
        StopAllCoroutines();

        SceneLoader.Instance.BeginLoadScene("The Carcass", true);
    }

    public void LoopRun(int currentLoop)
    {
        EventSystem.current.SetSelectedGameObject(null);
        GlobalData.currentLoop++;
        PlayerPrefs.SetInt("CurrentLoop", GlobalData.currentLoop);
        PlayerPrefs.Save();

        SceneLoader.Instance.BeginLoadScene("Daybreak Arboretum", true);
        creditsIsOn = false;
    }

    public void FinishRun()
    {
        EventSystem.current.SetSelectedGameObject(null);
        GlobalData.currentLoop = 1;
        PlayerPrefs.SetInt("CurrentLoop", GlobalData.currentLoop);
        PlayerPrefs.Save();
        StartCoroutine(PlayCredits());
    }

    public void PlaySelectSound()
    {
        SoundEffectManager.Instance.PlaySound("UISelect", camTracker.transform.position);
    }

    public void PlayMoveSound()
    {
        SoundEffectManager.Instance.PlaySound("UIMove", camTracker.transform.position);
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

        yield return StartCoroutine(FadeOut(endOfRunCanvas, fadeTimeEndOfRunCanvas));

        yield return new WaitForSecondsRealtime(pauseTime);

        yield return StartCoroutine(FadeIn(creditsCanvas, fadeTimeCreditsCanvas));

        yield return new WaitForSecondsRealtime(pauseTime);

        yield return StartCoroutine(MoveText());

        yield return new WaitForSecondsRealtime(pauseTime);

        yield return StartCoroutine(FadeOut(creditsCanvas, fadeTimeCreditsCanvas));

        yield return new WaitForSecondsRealtime(1f);

        SceneLoader.Instance.BeginLoadScene("The Carcass", true);

        creditsIsOn = false;
    }

    IEnumerator EndOfRun()
    {
        yield return StartCoroutine(FadeIn(blackCanvas, fadeTimeBlackCanvas));

        yield return new WaitForSeconds(1f);

        GameObject.Find("ContinueButton").GetComponent<Button>().Select();
        yield return StartCoroutine(FadeIn(endOfRunCanvas, fadeTimeEndOfRunCanvas));
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
