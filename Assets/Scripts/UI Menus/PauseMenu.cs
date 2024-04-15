using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject confirmMenu;
    [SerializeField] Button resumeButton;

    AudioMixerSnapshot unpausedSnapshot;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerSnapshot pausedSnapshot;
    [SerializeField] float muffleTransitionTime = 0.1f;

    CanvasGroup HUD = null;

    HUDItem hudItem;
    NutrientTracker nutrientTracker;
    GameObject playerParent;
    SceneLoader sceneLoaderScript;
    bool isOnMainMenu;
    private string audioTag;
    [HideInInspector] public int profileToDelete;
    [HideInInspector] public Button profileToSelect;

    private SporeManager sporeManagerScript;
    private ProfileManager profileManagerScript;

    private ThirdPersonActionsAsset playerInput = null;

    private void Awake()
    {
        isOnMainMenu = SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0);
        profileManagerScript = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();

        if (isOnMainMenu == false)
        {
            playerInput = new ThirdPersonActionsAsset();
            nutrientTracker = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();
            playerParent = GameObject.FindWithTag("PlayerParent");
            HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<CanvasGroup>();
            hudItem = HUD.GetComponent<HUDItem>();
            sceneLoaderScript = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
            GlobalData.isAbleToPause = true;

            //Resume();
        }

        if(GameObject.Find("SporeManager") != null)
        {
            sporeManagerScript = GameObject.Find("SporeManager").GetComponent<SporeManager>();
        }

        if (GameObject.FindWithTag("Camtracker") == null)
        {
            audioTag = "MainCamera";
        }
        else
        {
            audioTag = "Camtracker";
        }
    }

    void Start()
    {
        GlobalData.currentAudioMixerSnapshot = audioMixer.FindSnapshot("Default");
        GlobalData.currentAudioMixerSnapshot.TransitionTo(0f);
    }

    void OnPause()
    {
        if (isOnMainMenu == true || GlobalData.isAbleToPause == false)
        {
            return;
        }

        if (GlobalData.isGamePaused == true)
        {
            Resume();
            SoundEffectManager.Instance.PlaySound("UISelect", GameObject.FindWithTag(audioTag).transform.position);
        }
        else
        {
            Pause();
            resumeButton.Select();
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        confirmMenu.SetActive(false);
        HUD.GetComponent<HUDController>().FadeInHUD();
        unpausedSnapshot.TransitionTo(muffleTransitionTime);
        GlobalData.currentAudioMixerSnapshot = unpausedSnapshot;
        Time.timeScale = 1f;
        GlobalData.isGamePaused = false;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        unpausedSnapshot = GlobalData.currentAudioMixerSnapshot;

        SoundEffectManager.Instance.PlaySound("UISelect", GameObject.FindWithTag(audioTag).transform.position);
        pauseMenu.SetActive(true);
        HUD.GetComponent<HUDController>().FadeOutHUD();
        pausedSnapshot.TransitionTo(muffleTransitionTime);
        GlobalData.currentAudioMixerSnapshot = pausedSnapshot;
        Time.timeScale = 0f;
        GlobalData.isGamePaused = true;

        resumeButton.Select();

        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

    public void PlayUIMoveSound()
    {
        Time.timeScale = 1f;
        SoundEffectManager.Instance.PlaySound("UIMove", GameObject.FindWithTag(audioTag).transform.position);
        Time.timeScale = 0f;

        //Debug.Log("UI Move");
    }

    public void PlayUISelectSound()
    {
        Time.timeScale = 1f;
        SoundEffectManager.Instance.PlaySound("UISelect", GameObject.FindWithTag(audioTag).transform.position);
        Time.timeScale = 0f;
    }

    public void GoToMainMenu()
    {
        Resume();

        //GameManager.Instance.OnExitToMainMenu();

        if (sporeManagerScript != null)
        {
            sporeManagerScript.Save();
        }
        profileManagerScript.Save();

        sceneLoaderScript.BeginLoadScene(0, true);
    }

    public void GoToHubWorld()
    {
        Resume();

        profileManagerScript.Save();

        GlobalData.currentLoop = 1;
       

        nutrientTracker.LoseMaterials();
        playerParent.GetComponent<SwapWeapon>().curWeapon.tag = "Weapon";
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject weapon in weapons)
        Destroy(weapon);
        //GameManager.Instance.OnExitToHub();
        sceneLoaderScript.BeginLoadScene(2, true);
    }

    public void SelectProfileButton()
    {
        profileToSelect.Select();
    }

    public void DeleteProfile()
    {
        string sporeFilePath;
        string profileFilePath;

        if (Application.isEditor)
        {
            sporeFilePath = Application.dataPath + "/SporeData" + profileToDelete + ".json";
            profileFilePath = Application.dataPath + "/ProfileData" + profileToDelete + ".json";
        }
        else
        {
            sporeFilePath = Application.persistentDataPath + "/SporeData" + profileToDelete + ".json";
            profileFilePath = Application.persistentDataPath + "/ProfileData" + profileToDelete + ".json";
        }

        File.Delete(sporeFilePath);
        File.Delete(profileFilePath);
        profileManagerScript.tutorialIsDone[profileToDelete] = false;
    }

    public void DeleteAllSaveData()
    {
        string sporeFilePath;
        string profileFilePath;

        for(int i = 0; i <= 2; i++)
        {
            if (Application.isEditor)
            {
                sporeFilePath = Application.dataPath + "/SporeData" + i + ".json";
                profileFilePath = Application.dataPath + "/ProfileData" + i + ".json";
            }
            else
            {
                sporeFilePath = Application.persistentDataPath + "/SporeData" + i + ".json";
                profileFilePath = Application.persistentDataPath + "/ProfileData" + i + ".json";
            }

            File.Delete(sporeFilePath);
            File.Delete(profileFilePath);
            profileManagerScript.tutorialIsDone[i] = false;
        }

        //This is to remove old data files off the computer
        if (Application.isEditor)
        {
            sporeFilePath = Application.dataPath + "/SporeData.json";
            profileFilePath = Application.dataPath + "/ProfileData.json";
        }
        else
        {
            sporeFilePath = Application.persistentDataPath + "/SporeData.json";
            profileFilePath = Application.persistentDataPath + "/ProfileData.json";
        }

        File.Delete(sporeFilePath);
        File.Delete(profileFilePath);

        PlayerPrefs.DeleteAll();
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
