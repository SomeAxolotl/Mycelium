using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class MenuManger : MonoBehaviour
{
    public GameObject[] Characters;
    public int currentCharacterIndex;
    void Start()
    {
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedChar", 0);
        foreach(GameObject Char in Characters)
        {
            Char.SetActive(false);
            Characters[currentCharacterIndex].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
            PlayerPrefs.SetInt("SelectedChar", currentCharacterIndex);
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
