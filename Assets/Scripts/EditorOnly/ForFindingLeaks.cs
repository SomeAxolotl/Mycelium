using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForFindingLeaks : MonoBehaviour
{
    SceneLoader sceneLoaderScript;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name != "EmptyScene")
        {
            sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        }
    }

    private void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Comma))
        {
            try
            {
                sceneLoaderScript.BeginLoadScene("EmptyScene", true);
            }
            catch
            {
                SceneManager.LoadScene("EmptyScene");
            }
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            SceneManager.LoadScene("The Carcass");
        }*/
    }
}
