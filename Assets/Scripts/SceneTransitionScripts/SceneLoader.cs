using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

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
    [SerializeField] private GameObject titleImageGameObject;
    [SerializeField] private Sprite carcassSprite;
    [SerializeField] private Color carcassColor;
    [SerializeField] private Sprite daybreakSprite;
    [SerializeField] private Color daybreakColor;
    [SerializeField] private Sprite deltaSprite;
    [SerializeField] private Color deltaColor;
    [SerializeField] private Sprite impactSprite;
    [SerializeField] private Color impactColor;
    [SerializeField] private float titleFadoutTime = 1f;

    private Image titleImage;
    private RectTransform titleImageRectTransform;

    [HideInInspector()] public bool isLoading = false;

    private float defaultTransitionTime = 1f;
    private PlayerHealth playerHealthScript;

    //SceneUtility and SceneManager stuff
    private int totalSceneCount;

    public Action<bool> OnTitleCardFinished;

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
        titleImage = titleImageGameObject.GetComponent<Image>();
        titleImageRectTransform = titleImageGameObject.GetComponent<RectTransform>();

        if (GlobalData.gameIsStarting == true)
        {
            totalSceneCount = SceneManager.sceneCountInBuildSettings;
            //Debug.Log("TOTAL SCENE COUNT IS: " + totalSceneCount);

            for (int i = 0; i < totalSceneCount; i++)
            {
                //Debug.Log(SceneUtility.GetScenePathByBuildIndex(i) + "\t" + i);
                GlobalData.sceneNames.Add(ProcessScenePath(SceneUtility.GetScenePathByBuildIndex(i)));
            }
        }

        try
        {
            playerHealthScript = GameObject.Find("PlayerParent").GetComponent<PlayerHealth>();
            playerHealthScript.deathTimer = 0;
        }
        catch
        {
            Debug.LogWarning("PlayerParent does not currently exist", gameObject);
        }

        StartCoroutine(FinishLoadScene(defaultTransitionTime, GlobalData.gameIsStarting));
    }

    private string ProcessScenePath(string scenePath)
    {
        int highestSlashIndex = 0;

        for(int i = 0; i < scenePath.Length; i++)
        {
            if (scenePath[i] == '/' && i > highestSlashIndex)
            {
                highestSlashIndex = i;
            }
        }

        scenePath = scenePath.Remove(0, highestSlashIndex + 1);
        scenePath = scenePath.Replace(".unity", "");

        return scenePath;
    }

    public void BeginLoadScene(int sceneIndex, bool doGoodTransition, bool diedInTutorial = false)
    {
        StartCoroutine(GameObject.Find("BackgroundMusicPlayer").GetComponent<BGMController>().FadeOutMusicCoroutine());

        if (GameObject.Find("HUD") != null)
        {
            GameObject.Find("HUD").GetComponent<HUDController>().FadeHUD(false);
        }

        if (doGoodTransition == true)
        {
            StartCoroutine(LoadSceneGood(sceneIndex, defaultTransitionTime));
        }
        else
        {
            StartCoroutine(LoadSceneBad(sceneIndex, defaultTransitionTime, diedInTutorial));
        }
    }

    public void BeginLoadScene(string sceneName, bool doGoodTransition,  bool diedInTutorial = false)
    {
        StartCoroutine(GameObject.Find("BackgroundMusicPlayer").GetComponent<BGMController>().FadeOutMusicCoroutine());

        int sceneIndex;

        sceneIndex = GlobalData.sceneNames.IndexOf(sceneName);

        if(sceneIndex == -1)
        {
            Debug.LogError("That scene does not exist or is not in the build settings!");
            return;
        }

        if (GameObject.Find("HUD") != null)
        {
            GameObject.Find("HUD").GetComponent<HUDController>().FadeHUD(false);
        }

        if (doGoodTransition == true)
        {
            StartCoroutine(LoadSceneGood(sceneIndex, defaultTransitionTime));
        }
        else
        {
            StartCoroutine(LoadSceneBad(sceneIndex, defaultTransitionTime, diedInTutorial));
        }
    }

    public void BeginLoadScene(int sceneIndex, string notificationText,  bool diedInTutorial = false)
    {
        StartCoroutine(GameObject.Find("BackgroundMusicPlayer").GetComponent<BGMController>().FadeOutMusicCoroutine());

        if (GameObject.Find("HUD") != null)
        {
            GameObject.Find("HUD").GetComponent<HUDController>().FadeHUD(false);
        }

         StartCoroutine(LoadSceneGood(sceneIndex, defaultTransitionTime, notificationText));
    }

    public void BeginLoadScene(string sceneName, string notificationText)
    {
        StartCoroutine(GameObject.Find("BackgroundMusicPlayer").GetComponent<BGMController>().FadeOutMusicCoroutine());

        int sceneIndex;

        sceneIndex = GlobalData.sceneNames.IndexOf(sceneName);

        if (sceneIndex == -1)
        {
            Debug.LogError("That scene does not exist or is not in the build settings!");
            return;
        }

        if (GameObject.Find("HUD") != null)
        {
            GameObject.Find("HUD").GetComponent<HUDController>().FadeHUD(false);
        }

        StartCoroutine(LoadSceneGood(sceneIndex, defaultTransitionTime, notificationText));
    }

    IEnumerator FinishLoadScene(float transitionTime, bool isOnStartup)
    {
        if (isOnStartup == true && SceneManager.GetActiveScene().buildIndex == 0)
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

            yield return new WaitForSecondsRealtime(GlobalData.gameIsStarting ? 1f : 0.5f);
            GlobalData.gameIsStarting = false;

            yield return StartCoroutine(FadeCanvasOut(loadingCanvasGroup, transitionTime));
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "The Carcass":
                titleImage.sprite = carcassSprite;
                titleImage.color = carcassColor;
                break;

            case "Daybreak Arboretum":
                titleImage.sprite = daybreakSprite;
                titleImage.color = daybreakColor;
                break;

            case "Delta Crag":
                titleImage.sprite = deltaSprite;
                titleImage.color = deltaColor;
                break;

            case "Impact Barrens":
                titleImage.sprite = impactSprite;
                titleImage.color = impactColor;
                break;

            default:
                yield break;
        }

        titleImage.SetNativeSize();
        titleImageRectTransform.sizeDelta *= 0.383897f;

        yield return new WaitForSecondsRealtime(0.5f);
        StartCoroutine(ActivateTitleCanvas(5f));
        yield return StartCoroutine(FadeCanvasIn(titleCanvasGroup, transitionTime));
        yield return new WaitForSecondsRealtime(titleFadoutTime);
        yield return StartCoroutine(FadeCanvasOut(titleCanvasGroup, transitionTime));

        OnTitleCardFinished?.Invoke(true);
    }

    IEnumerator LoadSceneGood(int sceneIndex, float transitionTime)
    {
        isLoading = true;

        loadBar.fillAmount = 0;
        ChangeFunText(funText);

        yield return StartCoroutine(FadeCanvasIn(loadingCanvasGroup, transitionTime));

        StartCoroutine(StartLoading(sceneIndex));
    }

    IEnumerator LoadSceneGood(int sceneIndex, float transitionTime, string notificationText)
    {
        isLoading = true;

        loadBar.fillAmount = 0;
        ChangeFunText(funText);

        yield return StartCoroutine(FadeCanvasIn(blackCanvasGroup, transitionTime));

        yield return new WaitForSecondsRealtime(transitionTime);

        yield return StartCoroutine(FadeCanvasIn(loadingCanvasGroup, transitionTime));

        StartCoroutine(StartLoading(sceneIndex));
    }

    IEnumerator LoadSceneBad(int sceneIndex, float transitionTime, bool diedInTutorial = false)
    {
        isLoading = true;

        loadBar.fillAmount = 0;
        ChangeFunText(funText);

        StartCoroutine(FadeCanvasIn(blackCanvasGroup, transitionTime));

        ProfileManager profileManager = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();

        if (profileManager.permadeathIsOn[GlobalData.profileNumber] && !diedInTutorial)
        {
            NotificationManager.Instance.Notification("Your colony mourns " + GlobalData.sporePermaDied + "'s <b>death</b>", "<color=#8B0000>-Colony Happiness</color>");
        }
        else
        {
            NotificationManager.Instance.Notification("<i>Rise</i> my child,", "our colony must <b>persist</b>.");
        }

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
        //Application.backgroundLoadingPriority = ThreadPriority.Low;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        do
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadBar.fillAmount = progress;

            yield return null;
        }
        while (!operation.isDone);

        //Application.backgroundLoadingPriority = ThreadPriority.Normal;

        isLoading = false;
    }

    IEnumerator ActivateTitleCanvas(float time)
    {
        float elapsedTime = 0f;
        float t = 0f;
        float originalY = titleImageRectTransform.localPosition.y;
        float originalScale = 1f;

        while (elapsedTime < time)
        {
            t = elapsedTime / time;

            titleImageRectTransform.localPosition = new Vector3(0, Mathf.Lerp(originalY, originalY + 50, t), 0);
            titleImageRectTransform.localScale = new Vector3(Mathf.Lerp(originalScale, originalScale + 0.2f, t), Mathf.Lerp(originalScale, originalScale + 0.2f, t), 1f);

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

        funText.text = funTexts[UnityEngine.Random.Range(0, funTexts.Length)];
        GlobalData.currentFunText = funText.text;
    }
}