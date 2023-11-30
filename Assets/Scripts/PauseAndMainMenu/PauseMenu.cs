using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject confirmMenu;
    [SerializeField] Button resumeButton;

    CanvasGroup HUD = null;

    private ThirdPersonActionsAsset playerInput = null;

    private void Awake()
    {
        playerInput = new ThirdPersonActionsAsset();
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<CanvasGroup>();
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
        HUD.alpha = 1f;
        Time.timeScale = 1f;
        PauseData.isGamePaused = false;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        HUD.alpha = 0f;
        Time.timeScale = 0f;
        PauseData.isGamePaused = true;
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

    public void PlayUIMoveSound()
    {
        Time.timeScale = 1f;
        SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag("MainCamera").transform.position);
        Time.timeScale = 0f;

        //Debug.Log("UI Move");
    }

    public void PlayUISelectSound()
    {
        Time.timeScale = 1f;
        SoundEffectManager.Instance.PlaySound("UISelect", GameObject.FindWithTag("MainCamera").transform.position);
        Time.timeScale = 0f;

        //Debug.Log("UI Select");
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
        resumeButton.Select();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
