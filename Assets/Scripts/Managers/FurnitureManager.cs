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
        switch(furnitureName)
        {
            case "bed":
                return bedIsUnlocked;

            case "drum":
                return drumIsUnlocked;

            case "chair":
                return chairIsUnlocked;

            case "firefly":
                return fireflyIsUnlocked;

            case "gameboard":
                return gameboardIsUnlocked;

            case "fire":
                return fireIsUnlocked;

            default:
                return false;
        }
    }
}
