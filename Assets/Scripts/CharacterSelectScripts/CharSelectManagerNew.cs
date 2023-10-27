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

    public PlayerController playerController;
    void Start()
    {
        swapCharacter = GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>();
        playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        Test.Select();
    }

    void Update()
    {
        playerController.DisableController();
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
        SceneManager.LoadScene("Prototype Level");
    }
}
