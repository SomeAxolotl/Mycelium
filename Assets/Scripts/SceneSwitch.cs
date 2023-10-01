using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}
