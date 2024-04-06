using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CharSelectManagerNew : MonoBehaviour
{
    public SwapCharacter swapCharacter;
    public Button startButton;
    public GameObject UIEnable;
    public GameObject HUD;
    ThirdPersonActionsAsset controls;
    

    public PlayerController playerController;

    private SporeManager sporeManagerScript;
    private ProfileManager profileManagerScript;

    private void Awake()
    {
        controls = new ThirdPersonActionsAsset();
    }

    void Start()
    {
        swapCharacter = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
        sporeManagerScript = GameObject.Find("SporeManager").GetComponent<SporeManager>();
        profileManagerScript = GameObject.Find("ProfileManager").GetComponent<ProfileManager>();
    }

    void OnEnable()
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerController.DisableController();
        startButton.Select();
        HUD.GetComponent<HUDController>().FadeOutHUD();
        controls.UI.Close.performed += ctx => Close();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void Close()
    {
        playerController.EnableController();
        UIEnable.SetActive(false);
        GlobalData.isAbleToPause = true;
        HUD.GetComponent<HUDController>().FadeInHUD();
    }
    public void StartGame()
    {
        sporeManagerScript.Save();
        profileManagerScript.Save();
        playerController.EnableController();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Destroy(player);
        }
        //SceneManager.LoadScene("Prototype Level");
        playerController.EnableController();
        UIEnable.SetActive(false);
        GlobalData.isAbleToPause = true;
        SceneLoader.Instance.BeginLoadScene(3, true);
        
    }
}
