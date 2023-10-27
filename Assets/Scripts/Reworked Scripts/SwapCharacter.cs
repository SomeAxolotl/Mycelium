using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwapCharacter : MonoBehaviour
{
    public List<GameObject> characters;
    public int currentCharacterIndex = 0;
    public CharacterStats currentCharacterStats;
    private InputAction swapCharacter;
    private ThirdPersonActionsAsset playerActionsAsset;
    // Start is called before the first frame update
    void Awake()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        swapCharacter = playerActionsAsset.Player.SwapItem;
        SwitchCharacter(currentCharacterIndex);
    }

    // Update is called once per frame
    void Update()
    {
        //TEMPORARY KEYCODES ~ WILL BE TURNED INTO LEFT AND RIGHT MENU ARROWS IN THE FUTURE
        
        if (Input.GetKeyDown(KeyCode.N) && currentCharacterIndex < characters.Count - 1)
        {
            SwitchToNextCharacter();
        }
        
        if (Input.GetKeyDown(KeyCode.B) && currentCharacterIndex != 0)
        {
            SwitchToLastCharacter();
        }
    }

    public void SwitchCharacter(int index)
    {
        // Switch to the new character
        currentCharacterIndex = index;
        characters[currentCharacterIndex].tag = "currentPlayer";
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = true;
        characters[currentCharacterIndex].transform.parent = gameObject.transform;
        currentCharacterStats = characters[currentCharacterIndex].GetComponent<CharacterStats>();
    }
    void SwitchToNextCharacter()
    {
        characters[currentCharacterIndex].tag = "Player";
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = false;
        transform.DetachChildren();
        int nextCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        SwitchCharacter(nextCharacterIndex);
    }
    void SwitchToLastCharacter()
    {
        characters[currentCharacterIndex].tag = "Player";
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = false;
        transform.DetachChildren();
        int lastCharacterIndex = (currentCharacterIndex - 1) % characters.Count;
        SwitchCharacter(lastCharacterIndex);
    }
}