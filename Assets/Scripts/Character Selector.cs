using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
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

}
