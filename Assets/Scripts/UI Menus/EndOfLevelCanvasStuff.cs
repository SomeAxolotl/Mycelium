using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndOfLevelCanvasStuff : MonoBehaviour
{
    public static EndOfLevelCanvasStuff Instance;

    [SerializeField] CanvasGroup blackCanvas;
    [SerializeField] CanvasGroup endOfLevelCanvas;

    private LevelEnd levelEndScript;

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

    public void StartEndOfLevel(LevelEnd end)
    {
        levelEndScript = end;
        StartCoroutine(EndOfLevel());
    }

    public void ContinueAdventure()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1;
        levelEndScript.GoToNextLevel();
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

        yield return new WaitForSecondsRealtime(NotificationManager.Instance.totalDuration);

        GameObject.Find("ContinueButton").GetComponent<Button>().Select();
        StartCoroutine(FadeIn(endOfLevelCanvas, 0.5f));

        Time.timeScale = 0;
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
