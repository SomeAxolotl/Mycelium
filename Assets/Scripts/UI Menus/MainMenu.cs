using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] SceneLoader sceneLoaderScript;
    [SerializeField] ProfileManager profileManagerScript;
    [SerializeField] private List<MenuTypes> menuTypes;

    ThirdPersonActionsAsset playerInput;

    private void Awake()
    {
        playerInput = new ThirdPersonActionsAsset();

        //NOTE TO SELF: Probably eventually find a way to detect if someone is using a controller
        Cursor.visible = false;

        int randomValue = UnityEngine.Random.Range(0,101);
        int currentChance = 0;
        MenuTypes selectedMenu = menuTypes[0];
        foreach(MenuTypes menuType in menuTypes)
        {
            currentChance += menuType.StartChance;
            if(randomValue<=currentChance)
            {
                selectedMenu = menuType;
                Debug.Log("Menu Randomly Selected!");
                break;
            }
        }
        SelectMenu(selectedMenu);
    }

    private void SelectMenu(MenuTypes menu)
    {
        foreach(GameObject item in menu.ObjectsForScene){
            item.SetActive(true);
        }
    }

    public void QuitTheGame()
    {
        Application.Quit();
        Debug.Log("QUIT THE GAME");
    }

    public void StartGame()
    {
        if (profileManagerScript.tutorialIsDone == true)
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

[System.Serializable]
class MenuTypes {
    public List<GameObject> ObjectsForScene;
    [Tooltip("WARNING: ALL CHANCES MUST BE == 100 AND NOT EXCEED")] public int StartChance = 1;
}
