using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;

public class CharSelectManagerNew : MonoBehaviour
{
    public SwapCharacter swapCharacter;
    public Button Test;
    public GameObject UIEnable;
    public GameObject HUD;

    public PlayerController playerController;
    void Start()
    {
        swapCharacter = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        
    }

    void Update()
    {
        playerController.DisableController();
    }
    void OnEnable()
    {
        Test.Select();
    }
    public void Nextcharacter ()
    {
        if(swapCharacter.currentCharacterIndex < swapCharacter.characters.Count - 1)
        {
            swapCharacter.SwitchToNextCharacter();
        }
    }
    public void LastCharacter()
    {
         if (swapCharacter.currentCharacterIndex != 0)
        {
           swapCharacter.SwitchToLastCharacter();
        }
    }
    public void Close()
    {
        playerController.EnableController();
        UIEnable.SetActive(false);
    }
    public void StartGame()
    {
        playerController.EnableController();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Destroy(player);
        }
        SceneManager.LoadScene("Prototype Level");
        
    }
}
