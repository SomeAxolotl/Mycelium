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
    public Button Test;
    public GameObject UIEnable;
    public GameObject HUD;
    ThirdPersonActionsAsset controls;
    

    public PlayerController playerController;

    [SerializeField] SporeManager sporeManagerScript;

    void Start()
    {
        swapCharacter = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
        
        
    }

    void Update()
    {
        
    }
    void OnEnable()
    {
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        playerController.DisableController();
        controls = new ThirdPersonActionsAsset();
        Test.Select();
        controls.UI.Close.performed += ctx => Close();
        
    }

    public void Close()
    {
        playerController.EnableController();
        UIEnable.SetActive(false);
        GlobalData.isAbleToPause = true;
    }
    public void StartGame()
    {
        sporeManagerScript.Save();
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
