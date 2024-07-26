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
    private PlayerController playerController;
    private SwapWeapon swapWeapon;
    private PlayerHealth playerHealth;
    private PlayerAttack playerAttack;
    private SkillManager skillManager;

    private HUDSkills hudSkills;
    private HUDStats hudStats;

    // Start is called before the first frame update
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        swapWeapon = GetComponent<SwapWeapon>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAttack = GetComponent<PlayerAttack>();
        skillManager = GetComponent<SkillManager>();
        hudSkills = GameObject.Find("HUD").GetComponent<HUDSkills>();
        hudStats = GameObject.Find("HUD").GetComponent<HUDStats>();

        SwitchCharacter(currentCharacterIndex);
    }

    // Update is called once per frame
    void Update()
    {
        //TEMPORARY KEYCODES ~ WILL BE TURNED INTO LEFT AND RIGHT MENU ARROWS IN THE FUTURE
        
        //if (Input.GetKeyDown(KeyCode.N) && currentCharacterIndex < characters.Count - 1)
        //{
        //    SwitchToNextCharacter();
        //}
        
        //if (Input.GetKeyDown(KeyCode.B) && currentCharacterIndex != 0)
        //{
        //    SwitchToLastCharacter();
        //}
    }

    public void SwitchCharacter(int index)
    {
        GameObject oldSpore = GameObject.FindWithTag("currentPlayer");
        // Switch to the new character
        currentCharacterIndex = index;
        characters[currentCharacterIndex].tag = "currentPlayer";
        characters[currentCharacterIndex].GetComponent<WanderingSpore>().enabled = false;
        characters[currentCharacterIndex].GetComponent<SporeInteractableFinder>().enabled = true;
        characters[currentCharacterIndex].GetComponent<SphereCollider>().enabled = true;
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = true;
        characters[currentCharacterIndex].transform.parent = gameObject.transform;
        playerAttack.animator = characters[currentCharacterIndex].GetComponent<Animator>();
        currentCharacterStats = characters[currentCharacterIndex].GetComponent<CharacterStats>();
        //currentCharacterStats.StartCalculateAttributes();
        
        StartCoroutine(UpdateHealth());
        StartCoroutine(UpdateName());
        hudSkills.UpdateHUDIcons();
        hudStats.UpdateSporeTraitName(currentCharacterStats);

        Debug.Log("Swapped characters");
        Actions.SwappedCharacter?.Invoke(oldSpore, characters[currentCharacterIndex]);
    }

    public int GetCharacterIndex(GameObject character)
    {
        return characters.IndexOf(character);
    }
    public GameObject GetCharacterFromIndex(int index)
    {
        return characters[index];
    }

    public void SwitchCharacterGrowMenu(int index)
    {
        GameObject oldSpore = GameObject.FindWithTag("currentPlayer");
        //characters[currentCharacterIndex].GetComponent<CharacterStats>().ShowNametag();
        characters[currentCharacterIndex].tag = "Player";
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = false;
        transform.DetachChildren();
        characters[currentCharacterIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
        characters[currentCharacterIndex].GetComponent<WanderingSpore>().enabled = true;
        characters[currentCharacterIndex].GetComponent<SporeInteractableFinder>().enabled = false;
        characters[currentCharacterIndex].GetComponent<SphereCollider>().enabled = false;
        currentCharacterIndex = index;
        //characters[currentCharacterIndex].GetComponent<CharacterStats>().HideNametag();
        characters[currentCharacterIndex].tag = "currentPlayer";
        characters[currentCharacterIndex].GetComponent<WanderingSpore>().enabled = false;
        characters[currentCharacterIndex].GetComponent<SporeInteractableFinder>().enabled = true;
        characters[currentCharacterIndex].GetComponent<SphereCollider>().enabled = true;
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = true;
        characters[currentCharacterIndex].transform.parent = gameObject.transform;
        swapWeapon.currentCharacter = characters[currentCharacterIndex];
        playerAttack.animator = characters[currentCharacterIndex].GetComponent<Animator>();
        currentCharacterStats = characters[currentCharacterIndex].GetComponent<CharacterStats>();
        currentCharacterStats.StartCalculateAttributes();
        
        StartCoroutine(UpdateHealth());
        StartCoroutine(UpdateName());
        hudSkills.UpdateHUDIcons();
        hudStats.UpdateSporeTraitName(currentCharacterStats);
        playerController.GetStats();
        playerHealth.GetHealthStats();

        Debug.Log("Swapped characters");
        Actions.SwappedCharacter?.Invoke(oldSpore, characters[currentCharacterIndex]);
    }
    public void SwitchToNextCharacter()
    {
        GameObject oldSpore = GameObject.FindWithTag("currentPlayer");

        characters[currentCharacterIndex].tag = "Player";
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = false;
        transform.DetachChildren();
        characters[currentCharacterIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
        characters[currentCharacterIndex].GetComponent<WanderingSpore>().enabled = true;
        characters[currentCharacterIndex].GetComponent<SporeInteractableFinder>().enabled = false;
        characters[currentCharacterIndex].GetComponent<SphereCollider>().enabled = false;
        int nextCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        if (currentCharacterIndex==characters.Count)
        {
            currentCharacterIndex = 0;
        }
        SwitchCharacter(nextCharacterIndex);
        playerController.GetStats();
        playerHealth.GetHealthStats();

        Debug.Log("Swapped characters");
        Actions.SwappedCharacter?.Invoke(oldSpore, characters[currentCharacterIndex]);
    }
    public void SwitchToLastCharacter()
    {
        GameObject oldSpore = GameObject.FindWithTag("currentPlayer");
        
        characters[currentCharacterIndex].tag = "Player";
        characters[currentCharacterIndex].GetComponent<CharacterStats>().enabled = false;
        transform.DetachChildren();
        characters[currentCharacterIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
        characters[currentCharacterIndex].GetComponent<WanderingSpore>().enabled = true;
        characters[currentCharacterIndex].GetComponent<SporeInteractableFinder>().enabled = false;
        characters[currentCharacterIndex].GetComponent<SphereCollider>().enabled = false;
        int lastCharacterIndex = (currentCharacterIndex - 1 + characters.Count) % characters.Count;
        if (currentCharacterIndex==characters.Count)
        {
            currentCharacterIndex = 0;
        }
        SwitchCharacter(lastCharacterIndex);
        playerController.GetStats();
        playerHealth.GetHealthStats();

        Debug.Log("Swapped characters");
        Actions.SwappedCharacter?.Invoke(oldSpore, characters[currentCharacterIndex]);
    }

    IEnumerator UpdateHealth()
    {
        yield return new WaitForEndOfFrame();
        characters[currentCharacterIndex].GetComponentInParent<PlayerHealth>().ResetHealth();
    }
    IEnumerator UpdateName()
    {
        yield return new WaitForEndOfFrame();
        currentCharacterStats.UpdateSporeName();
    }
}
