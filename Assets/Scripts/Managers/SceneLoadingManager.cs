using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager Instance;

    [SerializeField] private GameObject loadCanvas;
    [SerializeField] private GameObject loadPanelHolder;
    [SerializeField] private Image loadBar;
    [SerializeField] private TMP_Text funText;
    //[SerializeField] private float popDuration = 0.25f;
    public bool isLoading = false;
    public GameObject LoadScreen;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        isLoading = true;
        
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadCanvas.SetActive(true);

        do
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadBar.fillAmount = progress;
            ChangeFunText(progress);

            yield return null;
        } 
        while (!operation.isDone);

        loadCanvas.SetActive(false);

        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        isLoading = false;
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        isLoading = true;
        
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadCanvas.SetActive(true);

        do
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadBar.fillAmount = progress;
            ChangeFunText(progress);

            yield return null;
        } 
        while (!operation.isDone);

        loadCanvas.SetActive(false);

        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        isLoading = false;
    }

    void ChangeFunText(float progress)
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

    float EaseOutQuart(float x)
    {
        return 1f - Mathf.Pow(1f - x, 4);
    }
}
