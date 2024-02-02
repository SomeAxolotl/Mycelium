using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] public Animator enterLevelTransition;
    [SerializeField] public Animator goodExitLevelTransition;
    [SerializeField] public Animator badExitLevelTransition;

    [SerializeField] public float transitionTime;

    private void Start()
    {
        enterLevelTransition.SetTrigger("End");
    }

    public void BeginLoadScene(int sceneIndex, bool doGoodTransition)
    {
        if(doGoodTransition == true)
        {
            StartCoroutine(LoadSceneGood(sceneIndex));
        }
        else
        {
            StartCoroutine(LoadSceneBad(sceneIndex));
        }
    }

    IEnumerator LoadSceneGood(int sceneIndex)
    {
        //Play animation
        goodExitLevelTransition.SetTrigger("Start");

        //Wait
        yield return new WaitForSecondsRealtime(transitionTime);

        //Load Scene
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator LoadSceneBad(int sceneIndex)
    {
        //Play animation
        badExitLevelTransition.SetTrigger("Start");

        //Wait
        yield return new WaitForSecondsRealtime(transitionTime);

        //Load Scene
        SceneManager.LoadScene(sceneIndex);
    }
}
