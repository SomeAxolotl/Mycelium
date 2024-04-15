using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndOfLevelCanvasStuff : MonoBehaviour
{
    public static EndOfLevelCanvasStuff Instance;

    [SerializeField] CanvasGroup blackCanvas;
    [SerializeField] CanvasGroup endOfLevelCanvas;
    [SerializeField] TMP_Text endOfLevelText;

    private LevelEnd levelEndScript;
    private string defaultText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        defaultText = endOfLevelText.text;
    }

    public void StartEndOfLevel(LevelEnd end)
    {
        levelEndScript = end;

        if(levelEndScript.isCheckpoint == true)
        {
            endOfLevelText.text = "It looks like this leads back to The Carcass. Would you like to continue your adventure or return home?";
        }
        else
        {
            endOfLevelText.text = defaultText;
        }

        StartCoroutine(EndOfLevel());
    }

    public void ContinueAdventure()
    {
        if (levelEndScript.isCheckpoint == true)
        {
            StartCoroutine(EndOfCheckpoint());
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            Time.timeScale = 1;
            levelEndScript.GoToNextLevel();
        }
    }

    public void FinishAdventure()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1;
        levelEndScript.GoToTheCarcass();
    }

    IEnumerator EndOfLevel()
    {
        yield return StartCoroutine(FadeIn(blackCanvas, 0.5f));

        if (levelEndScript.isCheckpoint == false)
        {
            yield return new WaitForSecondsRealtime(NotificationManager.Instance.totalDuration);
        }

        GameObject.Find("ContinueButton").GetComponent<Button>().Select();
        StartCoroutine(FadeIn(endOfLevelCanvas, 0.5f));

        Time.timeScale = 0;
    }

    IEnumerator EndOfCheckpoint()
    {
        EventSystem.current.SetSelectedGameObject(null);

        StartCoroutine(FadeOut(blackCanvas, 0.5f));
        yield return StartCoroutine(FadeOut(endOfLevelCanvas, 0.5f));

        levelEndScript.GoBackToGameplay();

        Time.timeScale = 1;
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
