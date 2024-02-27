using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    SceneLoader sceneLoaderScript;
    bool isOnMainMenu;

    private ThirdPersonActionsAsset playerInput = null;

    private void Awake()
    {
        isOnMainMenu = SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0);

        if (isOnMainMenu == false)
        {
            playerInput = new ThirdPersonActionsAsset();
            nutrientTracker = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();
            playerParent = GameObject.FindWithTag("PlayerParent");
            HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<CanvasGroup>();
            hudItem = HUD.GetComponent<HUDItem>();
            sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
            GlobalData.isAbleToPause = true;
            Resume();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnMainMenu == false)
        {
            if (playerInput.Player.Pause.WasPressedThisFrame() && GlobalData.isAbleToPause == true)
            {
                if (GlobalData.isGamePaused == true)
                {
                    Resume();
                    SoundEffectManager.Instance.PlaySound("UISelect", GameObject.FindWithTag("MainCamera").transform.position);
                }
                else
                {
                    Pause();
                    resumeButton.Select();
                }
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
        GlobalData.isGamePaused = false;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        SoundEffectManager.Instance.PlaySound("UISelect", GameObject.FindWithTag("MainCamera").transform.position);
        pauseMenu.SetActive(true);
        HUD.alpha = 0f;
        Time.timeScale = 0f;
        GlobalData.isGamePaused = true;

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
        GameManager.Instance.OnExitToMainMenu();
        sceneLoaderScript.BeginLoadScene(0, true);
    }

    public void GoToHubWorld()
    {
        nutrientTracker.LoseMaterials();
        playerParent.GetComponent<SwapWeapon>().curWeapon.tag = "Weapon";
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject weapon in weapons)
        Destroy(weapon);
        GameManager.Instance.OnExitToHub();
        sceneLoaderScript.BeginLoadScene(2, true);
    }

    public void DeleteAllSaveData()
    {
        string filePath;

        if (Application.isEditor)
        {
            filePath = Application.dataPath + "/SporeData.json";
        }
        else
        {
            filePath = Application.persistentDataPath + "/SporeData.json";
        }

        PlayerPrefs.DeleteAll();
        File.Delete(filePath);
    }

    private void OnEnable()
    {
        if (isOnMainMenu == false)
        {
            playerInput.Enable();
            resumeButton.Select();
        }
    }

    private void OnDisable()
    {
        if (isOnMainMenu == false)
        {
            playerInput.Disable();
        }
    }
}
