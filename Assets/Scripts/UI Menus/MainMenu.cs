using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] SceneLoader sceneLoaderScript;

    ThirdPersonActionsAsset playerInput;

    private void Awake()
    {
        playerInput = new ThirdPersonActionsAsset();

        //NOTE TO SELF: Probably eventually find a way to detect if someone is using a controller
        Cursor.visible = false;
    }

    public void QuitTheGame()
    {
        Application.Quit();
        Debug.Log("QUIT THE GAME");
    }

    public void StartGame()
    {
        if (Convert.ToBoolean(PlayerPrefs.GetInt("IsTutorialFinished")))
        {
            sceneLoaderScript.BeginLoadScene(SceneManager.GetActiveScene().buildIndex + 2, true);
        }
        else
        {
            sceneLoaderScript.BeginLoadScene(SceneManager.GetActiveScene().buildIndex + 1, true);
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playButton.Select();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }
}
