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

    private HUDSkills hudSkills;

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
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();

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
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = true;
        characters[currentCharacterIndex].transform.parent = gameObject.transform;
        currentCharacterStats = characters[currentCharacterIndex].GetComponent<CharacterStats>();
        currentCharacterStats.UpdateAnimatorSpeed();
        
        StartCoroutine(UpdateHealth());
        StartCoroutine(UpdateName());
        StartCoroutine(UpdateHUDIcons());
    }

    IEnumerator UpdateHealth()
    {
        yield return new WaitForSeconds(0.1f);
        characters[currentCharacterIndex].GetComponentInParent<NewPlayerHealth>().ResetHealth();
    }
    IEnumerator UpdateName()
    {
        yield return new WaitForSeconds(0.1f);
        currentCharacterStats.UpdateSporeName();
    }

    IEnumerator UpdateHUDIcons()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject player = GameObject.FindWithTag("currentPlayer");
        Transform skillLoadout = player.transform.Find("SkillLoadout");
        //Debug.Log(skillLoadout.GetChild(0).gameObject.name);
        hudSkills.ChangeSkillIcon(skillLoadout.GetChild(0).gameObject.name, 0);
        yield return new WaitForSeconds(0.1f);
        //Debug.Log(skillLoadout.GetChild(1).gameObject.name);
        hudSkills.ChangeSkillIcon(skillLoadout.GetChild(1).gameObject.name, 1);
        yield return new WaitForSeconds(0.1f);
        //Debug.Log(skillLoadout.GetChild(2).gameObject.name);
        hudSkills.ChangeSkillIcon(skillLoadout.GetChild(2).gameObject.name, 2);
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
