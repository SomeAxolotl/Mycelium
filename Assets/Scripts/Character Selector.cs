using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] Characters;
    public int currentCharacterIndex;
    public GameObject Player1;
    public GameObject Player2;
    void Start()
    {
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedChar", 0);
        foreach(GameObject Char in Characters)
        {
            Char.SetActive(false);
            Characters[currentCharacterIndex].SetActive(true);
            
        }
        
    }
    void Update()
    {
        if (Player1.activeSelf)
        {
            Player1.tag = "currentPlayer";
        }
        if (Player2.activeSelf)
        {
            Player2.tag = "currentPlayer";
        }
    }

}
