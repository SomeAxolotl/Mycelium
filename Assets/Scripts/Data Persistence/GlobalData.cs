using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    //Scene Loader Stuff
    public static bool gameIsStarting = true;
    public static string currentFunText = "FUN TEXT NOT SET";
    public static List<string> sceneNames = new List<string>();

    //Pause Stuff
    public static bool isGamePaused { get; set; }
    public static bool isAbleToPause { get; set; }

    //Weapon Stuff
    public static string currentWeapon { get; set; }
    public static float currentWeaponDamage { get; set; }
    public static float currentWeaponKnockback { get; set; }

    //Nutrient Deposit Stuff
    public static List<int> currentSporeStats = new List<int>();

    //Save Data Stuff
    public static int profileNumber { get; set; }

    //Happiness Stuff
    public static float happinessStatMultiplier = 1f;
    public static int happinessStatIncrement = 0;
    public static bool areaCleared = false;
    public static string sporeDied = null;
    public static string sporePermaDied = null;
}
