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

    [Header("--Loading Canvas Section--")]
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private Image loadBar;
    [SerializeField] private TMP_Text funText;

    [Header("--Startup Canvas Section--")]
    [SerializeField] private CanvasGroup startupCanvasGroup;
    [SerializeField] private float delayStartupCanvasTime;
    [SerializeField] private float startupCanvasTime;

    [Header("--Death Canvas Section--")]
    [SerializeField] private CanvasGroup deathCanvasGroup;
    [SerializeField] private float deathCanvasTime;

    [Header("--Title Canvas Section--")]
    [SerializeField] private CanvasGroup titleCanvasGroup;
    [SerializeField] private GameObject titleTextGameObject;
    [SerializeField] private Color carcassTextColor;
    [SerializeField] private Color daybreakTextColor;
    [SerializeField] private Color deltaTextColor;
    [SerializeField] private Color bossTestTextColor;
    [SerializeField] private float titleFadoutTime = 1f;

    private TMP_Text titleText;
    private RectTransform titleTextRectTransform;

    [HideInInspector()] public bool isLoading = false;

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

        titleText = titleTextGameObject.GetComponent<TMP_Text>();
        titleTextRectTransform = titleTextGameObject.GetComponent<RectTransform>();

        try
        {
            playerHealthScript = GameObject.Find("PlayerParent").GetComponent<PlayerHealth>();
            playerHealthScript.deathTimer = 0;
        }
        catch
        {
            Debug.Log("PlayerParent does not currently exist");
        }
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
        //GameObject.Find("PauseMenuCanvas").GetComponent<PauseMenu>().Resume();

        if (isOnStartup == true)
        {
            GlobalData.gameIsStarting = false;

            blackCanvasGroup.alpha = 1f;

            yield return new WaitForSecondsRealtime(delayStartupCanvasTime);

            yield return StartCoroutine(FadeCanvasIn(startupCanvasGroup, transitionTime));

            yield return new WaitForSecondsRealtime(startupCanvasTime);

            StartCoroutine(FadeCanvasOut(blackCanvasGroup, transitionTime));
            yield return StartCoroutine(FadeCanvasOut(startupCanvasGroup, transitionTime));
        }
        else
        {
            loadBar.fillAmount = 1;
            funText.text = GlobalData.currentFunText;

            loadingCanvasGroup.alpha = 1f;

            yield return new WaitForSecondsRealtime(0.5f);

            yield return StartCoroutine(FadeCanvasOut(loadingCanvasGroup, transitionTime));
        }

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                titleText.text = "The Carcass";
                titleText.color = carcassTextColor;
                break;

            case 3:
                titleText.text = "The Daybreak Arboretum";
                titleText.color = daybreakTextColor;
                break;

            case 4:
                titleText.text = "The Delta Crag";
                titleText.color = deltaTextColor;
                break;

            case 5:
                titleText.text = "The Boss";
                titleText.color = bossTestTextColor;
                break;

            default:
                yield break;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        StartCoroutine(ActivateTitleCanvas(5f));
        yield return StartCoroutine(FadeCanvasIn(titleCanvasGroup, transitionTime));
        yield return new WaitForSecondsRealtime(titleFadoutTime);
        yield return StartCoroutine(FadeCanvasOut(titleCanvasGroup, transitionTime));

        GameObject.Find("HUD").GetComponent<HUDController>().FadeInHUD();
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

    IEnumerator ActivateTitleCanvas(float time)
    {
        float elapsedTime = 0f;
        float t = 0f;
        float originalY = titleTextRectTransform.localPosition.y;
        float originalScale = 1f;

        while (elapsedTime < time)
        {
            t = elapsedTime / time;

            titleTextRectTransform.localPosition = new Vector3(0, Mathf.Lerp(originalY, originalY + 50, t), 0);
            titleTextRectTransform.localScale = new Vector3(Mathf.Lerp(originalScale, originalScale + 0.2f, t), Mathf.Lerp(originalScale, originalScale + 0.2f, t), 1f);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        //Debug.Log("DONE WITH TITLE CARD");
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
}