using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.TimeZoneInfo;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("--Black Canvas Section--")]
    [SerializeField] private CanvasGroup blackCanvasGroup;

    [Header("--Startup Canvas Section--")]
    [SerializeField] private CanvasGroup startupCanvasGroup;
    [SerializeField] private float delayStartupCanvasTime;

    [Header("--Loading Canvas Section--")]
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private Image loadBar;
    [SerializeField] private TMP_Text funText;

    [Header("--Death Canvas Section--")]
    [SerializeField] private CanvasGroup deathCanvasGroup;
    [SerializeField] private float deathCanvasTime;

    [Header("")]
    public bool isLoading = false;

    private float defaultTransitionTime = 1f;
    private PlayerHealth playerHealthScript;

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
        StartCoroutine(FinishLoadScene(defaultTransitionTime, GlobalData.gameIsStarting));

        playerHealthScript = GameObject.Find("PlayerParent").GetComponent<PlayerHealth>();
        playerHealthScript.deathTimer = 0;
    }

    public void BeginLoadScene(int sceneIndex, bool doGoodTransition)
    {
        if (doGoodTransition == true)
        {
            StartCoroutine(LoadSceneGood(sceneIndex, defaultTransitionTime));
        }
        else
        {
            StartCoroutine(LoadSceneBad(sceneIndex, defaultTransitionTime));
        }
    }

    public void BeginLoadScene(int sceneIndex, bool doGoodTransition, float transitionTime)
    {
        if (doGoodTransition == true)
        {
            StartCoroutine(LoadSceneGood(sceneIndex, transitionTime));
        }
        else
        {
            StartCoroutine(LoadSceneBad(sceneIndex, transitionTime));
        }
    }

    IEnumerator FinishLoadScene(float transitionTime, bool isOnStartup)
    {
        if (isOnStartup == true)
        {
            GlobalData.gameIsStarting = false;

            blackCanvasGroup.alpha = 1f;

            yield return new WaitForSecondsRealtime(delayStartupCanvasTime);

            yield return StartCoroutine(FadeCanvasIn(startupCanvasGroup, transitionTime));

            yield return new WaitForSecondsRealtime(0.5f); //time before it all goes away

            StartCoroutine(FadeCanvasOut(blackCanvasGroup, transitionTime));
            StartCoroutine(FadeCanvasOut(startupCanvasGroup, transitionTime));

        }
        else
        {
            loadBar.fillAmount = 1;
            funText.text = GlobalData.currentFunText;

            loadingCanvasGroup.alpha = 1f;

            yield return new WaitForSecondsRealtime(0.5f);

            StartCoroutine(FadeCanvasOut(loadingCanvasGroup, transitionTime));
        }
    }

    IEnumerator LoadSceneGood(int sceneIndex, float transitionTime)
    {
        isLoading = true;

        loadBar.fillAmount = 0;
        ChangeFunText(funText);

        yield return StartCoroutine(FadeCanvasIn(loadingCanvasGroup, transitionTime));

        StartCoroutine(StartLoading(sceneIndex));
    }

    IEnumerator LoadSceneBad(int sceneIndex, float transitionTime)
    {
        isLoading = true;

        loadBar.fillAmount = 0;
        ChangeFunText(funText);

        StartCoroutine(FadeCanvasIn(blackCanvasGroup, transitionTime));
        yield return StartCoroutine(FadeCanvasIn(deathCanvasGroup, transitionTime));

        yield return new WaitForSecondsRealtime(deathCanvasTime);

        yield return StartCoroutine(FadeCanvasIn(loadingCanvasGroup, transitionTime));

        StartCoroutine(StartLoading(sceneIndex));
    }

    void ChangeFunText(TMP_Text funText)
    {
        string[] funTexts =
        {
            "Gearing up Spore",
            "Preparing Forage",
            "Finalizing Fungi",
            "Fortifying Fungus",
            "Enhancing Evolution",
            "Strengthening Spores"
        };

        funText.text = funTexts[Random.Range(0, funTexts.Length)];
        GlobalData.currentFunText = funText.text;
    }

    IEnumerator FadeCanvasIn(CanvasGroup canvasGroup, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    IEnumerator FadeCanvasOut(CanvasGroup canvasGroup, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    IEnumerator StartLoading(int sceneIndex)
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        do
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadBar.fillAmount = progress;

            yield return null;
        }
        while (!operation.isDone);

        Application.backgroundLoadingPriority = ThreadPriority.Normal;

        isLoading = false;
    }
}