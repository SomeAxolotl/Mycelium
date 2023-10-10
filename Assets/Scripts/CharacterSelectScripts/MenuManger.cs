using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class MenuManger : MonoBehaviour
{
    public GameObject[] Characters;
    public int currentCharacterIndex;
    public CharacterInformation[] Characterinfo;
    public Button purchaseButton;
    public Button StartGamebutton;

    private HUDNutrients hudNutrients;
    private NutrientTracker nutrientTracker;

    void Start()
    {
        foreach(CharacterInformation Char in Characterinfo)
        {
            if(Char.nutrientcost ==0)
            {
                Char.isUnlocked=true;
            }
            else
            {
                Char.isUnlocked = PlayerPrefs.GetInt(Char.CharName, 0)==0 ? false: true;
            }
        }
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedChar", 0);
        foreach(GameObject Char in Characters)
        {
            Char.SetActive(false);
            Characters[currentCharacterIndex].SetActive(true);
        }
        
        hudNutrients = GameObject.Find("HUD").GetComponent<HUDNutrients>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
    public void ChangeNext()
    {
        Characters[currentCharacterIndex].SetActive(false);

        currentCharacterIndex++;
        if(currentCharacterIndex==Characters.Length)
        {
            currentCharacterIndex= 0;
        }
            Characters[currentCharacterIndex].SetActive(true);
            CharacterInformation c = Characterinfo[currentCharacterIndex];
            if(!c.isUnlocked)
            {
                return;
            }
            PlayerPrefs.SetInt("SelectedChar", currentCharacterIndex);
            
        
    }
    public void ChangePrevious()
    {
        Characters[currentCharacterIndex].SetActive(false);

        currentCharacterIndex--;
        if(currentCharacterIndex==-1)
        {
            currentCharacterIndex= Characters.Length -1;
        }
            Characters[currentCharacterIndex].SetActive(true);
              CharacterInformation c = Characterinfo[currentCharacterIndex];
            if(!c.isUnlocked)
            {
                return;
            }
            PlayerPrefs.SetInt("SelectedChar", currentCharacterIndex);
        
    }
    public void UnlockCharacter()
    {
        CharacterInformation c = Characterinfo[currentCharacterIndex];
        PlayerPrefs.SetInt(c.CharName, 1);
        PlayerPrefs.SetInt("SelectedChar", currentCharacterIndex);
        c.isUnlocked = true;
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        nutrientTracker.SubtractNutrients(c.nutrientcost);
        hudNutrients.UpdateNutrientsUI();
        StartGamebutton.Select();
    }
    public void UpdateUI()
    {
        CharacterInformation c = Characterinfo[currentCharacterIndex];
        if (c.isUnlocked)
        {
            purchaseButton.gameObject.SetActive(false);
            
        }
        else
        {
            purchaseButton.gameObject.SetActive(true);
            if(c.nutrientcost <= PlayerPrefs.GetInt("currentNutrients", 0))
            {
                purchaseButton.interactable = true;
            }
            else
            {
                purchaseButton.interactable = false;
                
            }
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Prototype Level");
        
    }
}
