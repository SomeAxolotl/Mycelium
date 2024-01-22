using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] int transitionTime;
    [SerializeField] Image image;

    private void Start()
    {
        transition.SetTrigger("End");
        StartCoroutine(WaitThenColorBlack());
    }

    public void BeginLoadScene(int sceneIndex)
    {
        StartCoroutine(LoadScene(sceneIndex));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        //Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator WaitThenColorBlack()
    {
        yield return new WaitForSeconds(2);
        image.color = Color.black;
    }
}
