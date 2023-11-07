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
    private PlayerController playerController;
    private SwapWeapon swapWeapon;
    private NewPlayerHealth newPlayerHealth;
    private SkillManager skillManager;
    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        swapWeapon = GetComponent<SwapWeapon>();
        newPlayerHealth = GetComponent<NewPlayerHealth>();
        playerActionsAsset = new ThirdPersonActionsAsset();
        skillManager = GetComponent<SkillManager>();
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
        if(characters[currentCharacterIndex].GetComponent<IdleWalking>().wander != null )
        {
            characters[currentCharacterIndex].GetComponent<IdleWalking>().StartCoroutine("StopWander");
        }
        skillManager.GetSkillLoadout(characters[currentCharacterIndex]);
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = true;
        characters[currentCharacterIndex].transform.parent = gameObject.transform;
        currentCharacterStats = characters[currentCharacterIndex].GetComponent<CharacterStats>();
    }
    public void SwitchToNextCharacter()
    {
        characters[currentCharacterIndex].tag = "Player";
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = false;
        transform.DetachChildren();
        characters[currentCharacterIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
        int nextCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        if (currentCharacterIndex==characters.Count)
        {
            currentCharacterIndex = 0;
        }
        SwitchCharacter(nextCharacterIndex);
        playerController.GetStats();
        newPlayerHealth.GetHealthStats();
        swapWeapon.UpdateCharacter(characters[currentCharacterIndex]);
    }
    public void SwitchToLastCharacter()
    {
        characters[currentCharacterIndex].tag = "Player";
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = false;
        transform.DetachChildren();
        characters[currentCharacterIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
        int lastCharacterIndex = (currentCharacterIndex - 1) % characters.Count;
        if (currentCharacterIndex==characters.Count)
        {
            currentCharacterIndex = 0;
        }
        SwitchCharacter(lastCharacterIndex);
        playerController.GetStats();
        newPlayerHealth.GetHealthStats();
        swapWeapon.UpdateCharacter(characters[currentCharacterIndex]);
    }
}
