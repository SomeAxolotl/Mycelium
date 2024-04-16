using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureManager : MonoBehaviour
{
    public static FurnitureManager Instance;

    public bool bedIsUnlocked;
    public bool drumIsUnlocked;
    public bool chairIsUnlocked;
    public bool fireflyIsUnlocked;
    public bool gameboardIsUnlocked;
    public bool fireIsUnlocked;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public bool FurnitureIsUnlocked(string furnitureName)
    {
        Debug.Log(furnitureName);
        switch(furnitureName)
        {
            case "Bed":
                return bedIsUnlocked;

            case "Drum":
                return drumIsUnlocked;

            case "Chair":
                return chairIsUnlocked;

            case "Firefly":
                return fireflyIsUnlocked;

            case "Gameboard":
                return gameboardIsUnlocked;

            case "Bonfire":
                return fireIsUnlocked;

            default:
                Debug.LogError("Invalid furniture name");
                return false;
        }
    }
}
