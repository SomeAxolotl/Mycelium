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
            case "Bed":
                return bedIsUnlocked;

            case "Drum":
                return drumIsUnlocked;

            case "Chair":
                return chairIsUnlocked;

            case "FireflyBottle":
                return fireflyIsUnlocked;

            case "FireflyBottleNight":
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

    public void UnlockAllFurniture()
    {
        bedIsUnlocked = true;
        drumIsUnlocked = true;
        chairIsUnlocked = true;
        fireflyIsUnlocked = true;
        gameboardIsUnlocked = true;
        fireIsUnlocked = true;
    }
}
