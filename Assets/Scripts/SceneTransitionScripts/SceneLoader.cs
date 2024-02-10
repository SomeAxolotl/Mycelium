using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] public CanvasGroup enterLevelTransition;
    [SerializeField] public CanvasGroup goodExitLevelTransition;
    [SerializeField] public CanvasGroup badExitLevelTransition;

    float defaultTransitionTime = 1f;

    private void Start()
    {
        StartCoroutine(EnterScene(2f));
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

    IEnumerator EnterScene(float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;
        float i = 0f;

        enterLevelTransition.alpha = 1f;
        enterLevelTransition.blocksRaycasts = true;
        enterLevelTransition.interactable = true;

        while (i < 0.5f)
        {
            i += Time.deltaTime;

            yield return null;
        }

        while (elapsedTime < transitionTime + 0.5f)
        {
            t = elapsedTime / transitionTime;

            enterLevelTransition.alpha = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        enterLevelTransition.blocksRaycasts = false;
        enterLevelTransition.interactable = false;
        Debug.Log("Canvas Faded Out");
    }

    IEnumerator LoadSceneGood(int sceneIndex, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        enterLevelTransition.blocksRaycasts = false;
        enterLevelTransition.interactable = false;

        while (elapsedTime < transitionTime + 0.5f)
        {
            t = elapsedTime / transitionTime;

            goodExitLevelTransition.alpha = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        enterLevelTransition.blocksRaycasts = true;
        enterLevelTransition.interactable = true;

        Debug.Log("Good Canvas Faded In");

        //Load Scene
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator LoadSceneBad(int sceneIndex, float transitionTime)
    {
        float elapsedTime = 0f;
        float t = 0f;

        enterLevelTransition.blocksRaycasts = false;
        enterLevelTransition.interactable = false;

        while (elapsedTime < transitionTime + 0.5f)
        {
            t = elapsedTime / transitionTime;

            badExitLevelTransition.alpha = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        enterLevelTransition.blocksRaycasts = true;
        enterLevelTransition.interactable = true;

        Debug.Log("Bad Canvas Faded In");

        //Load Scene
        SceneManager.LoadScene(sceneIndex);
    }
}
