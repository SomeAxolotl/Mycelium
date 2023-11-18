using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject confirmMenu;

    GameObject HUD = null;

    private ThirdPersonActionsAsset playerInput = null;

    private void Awake()
    {
        playerInput = new ThirdPersonActionsAsset();
        HUD = GameObject.FindGameObjectWithTag("HUD");
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInput.Player.Pause.WasPressedThisFrame())
        {
            if (PauseData.isGamePaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        confirmMenu.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
        PauseData.isGamePaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0f;
        PauseData.isGamePaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToHubWorld()
    {
        SceneManager.LoadScene(1); //this is assuming the hubworld scene is index 1
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
