using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;

    ThirdPersonActionsAsset playerInput;

    private void Awake()
    {
        playerInput = new ThirdPersonActionsAsset();
    }

    public void QuitTheGame()
    {
        Application.Quit();
        Debug.Log("QUIT THE GAME");
    }

    public void GoToHubWorld()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
