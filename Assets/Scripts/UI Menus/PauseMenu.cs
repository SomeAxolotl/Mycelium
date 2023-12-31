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

    HUDItem hudItem;
    NutrientTracker nutrientTracker;
    GameObject playerParent;

    private ThirdPersonActionsAsset playerInput = null;

    private void Awake()
    {
        playerInput = new ThirdPersonActionsAsset();
        nutrientTracker = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();
        playerParent = GameObject.FindWithTag("PlayerParent");
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<CanvasGroup>();
        hudItem = HUD.GetComponent<HUDItem>();
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
                SoundEffectManager.Instance.PlaySound("UISelect", GameObject.FindWithTag("MainCamera").transform.position);
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
        SoundEffectManager.Instance.PlaySound("UISelect", GameObject.FindWithTag("MainCamera").transform.position);
        pauseMenu.SetActive(true);
        HUD.alpha = 0f;
        Time.timeScale = 0f;
        PauseData.isGamePaused = true;

        resumeButton.Select();

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
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToHubWorld()
    {
        nutrientTracker.LoseMaterials();
        playerParent.GetComponent<SwapWeapon>().curWeapon.tag = "Weapon";
        Instantiate(Resources.Load("Weapons/StartWeapon"), GameObject.FindWithTag("WeaponSlot").transform);
        playerParent.GetComponent<SwapWeapon>().UpdateCharacter(GameObject.FindWithTag("currentPlayer"));
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject weapon in weapons)
        Destroy(weapon);
        if(playerParent.GetComponent<SwapWeapon>().curWeapon != null)
        {
            SceneManager.LoadScene(1); //this is assuming the hubworld scene is index 1
        }
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
