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

    [Header("--Enter Section--")]
    [SerializeField] private Canvas enterSceneCanvas;
    [SerializeField] private Image enterSceneLoadBar;
    [SerializeField] private TMP_Text enterSceneFunText;
    [SerializeField] private GameObject mainMenuStartupImage;
    [SerializeField] private GameObject defaultImage;
    [SerializeField] private GameObject defaultLoadPanel;

    [Header("--Good Exit Section--")]
    [SerializeField] private Canvas goodExitSceneCanvas;
    [SerializeField] private Image goodExitSceneLoadBar;
    [SerializeField] private TMP_Text goodExitSceneFunText;
    
    [Header("--Bad Exit Section--")]
    [SerializeField] private Canvas badExitSceneCanvasPart1;
    [SerializeField] private Canvas badExitSceneCanvasPart2;
    [SerializeField] private Image badExitSceneLoadBar;
    [SerializeField] private TMP_Text badExitSceneFunText;

    [Header("")]
    public bool isLoading = false;

    private CanvasGroup enterSceneCanvasGroup;
    private CanvasGroup goodExitSceneCanvasGroup;
    private CanvasGroup badExitSceneCanvasGroupPart1;
    private CanvasGroup badExitSceneCanvasGroupPart2;

    float defaultTransitionTime = 1f;

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

        enterSceneCanvasGroup = enterSceneCanvas.GetComponent<CanvasGroup>();
        goodExitSceneCanvasGroup = goodExitSceneCanvas.GetComponent<CanvasGroup>();
        badExitSceneCanvasGroupPart1 = badExitSceneCanvasPart1.GetComponent<CanvasGroup>();
        badExitSceneCanvasGroupPart2 = badExitSceneCanvasPart2.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
            StartCoroutine(EnterScene(defaultTransitionTime, GlobalData.gameIsStarting));
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

    IEnumerator EnterScene(float transitionTime, bool isOnStartup)
    {
        float elapsedTime = 0f;
        float t = 0f;
        float i = 0f;

        if (isOnStartup == true)
        {
            mainMenuStartupImage.SetActive(true);
            defaultImage.SetActive(false);
            defaultLoadPanel.SetActive(false);

            GlobalData.gameIsStarting = false;
        }

        enterSceneLoadBar.fillAmount = 1;
        ChangeFunText(enterSceneFunText, 1);

        enterSceneCanvasGroup.alpha = 1f;
        enterSceneCanvasGroup.blocksRaycasts = true;
        enterSceneCanvasGroup.interactable = true;

        while (i < 0.5f)
        {
            i += Time.deltaTime;

            yield return null;
        }

        while (elapsedTime < transitionTime)
        {
            t = elapsedTime / transitionTime;

            enterSceneCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        enterSceneCanvasGroup.alpha = 0f;
        enterSceneCanvasGroup.blocksRaycasts = false;
        enterSceneCanvasGroup.interactable = false;

        Debug.Log("Canvas Faded Out");
    }

    IEnumerator LoadSceneGood(int sceneIndex, float transitionTime)
    {
        isLoading = true;

        yield return StartCoroutine(FadeCanvasIn(goodExitSceneCanvasGroup, transitionTime));
        Debug.Log("Good Canvas Faded In");

        Application.backgroundLoadingPriority = ThreadPriority.Low;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        do
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            goodExitSceneLoadBar.fillAmount = progress;
            ChangeFunText(goodExitSceneFunText, progress);

            yield return null;
        }
        while (!operation.isDone);

        Application.backgroundLoadingPriority = ThreadPriority.Normal;

        isLoading = false;
    }

    IEnumerator LoadSceneBad(int sceneIndex, float transitionTime)
    {
        isLoading = true;

        yield return StartCoroutine(FadeCanvasIn(badExitSceneCanvasGroupPart1, transitionTime));
        Debug.Log("Bad Canvas Part 1 Faded In");

        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(FadeCanvasIn(badExitSceneCanvasGroupPart2, transitionTime));
        Debug.Log("Bad Canvas Part 2 Faded In");

        Application.backgroundLoadingPriority = ThreadPriority.Low;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        do
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            badExitSceneLoadBar.fillAmount = progress;
            ChangeFunText(badExitSceneFunText, progress);

            yield return null;
        }
        while (!operation.isDone);

        Application.backgroundLoadingPriority = ThreadPriority.Normal;

        isLoading = false;
    }

    void ChangeFunText(TMP_Text funText, float progress)
    {
        string[] funTexts =
        {
            "Gearing up Spore",
            "Preparing Forage",
            "Finalizing Fungi"
        };

        int funTextArrayCount = Mathf.FloorToInt(progress * (funTexts.Length - 1));
        funTextArrayCount = Mathf.Clamp(funTextArrayCount, 0, funTexts.Length - 1);

        funText.text = funTexts[funTextArrayCount];
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
}
