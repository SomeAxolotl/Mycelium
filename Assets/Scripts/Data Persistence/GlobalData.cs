using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    public static WeaponStatList currentWeaponStats { get; set; }

    //Attribute Stuff
    public static List<AttributeInfo> currentAttribute = new List<AttributeInfo>();

    //Nutrient Deposit Stuff
    public static List<int> currentSporeStats = new List<int>();

    //Save Data Stuff
    public static int profileNumber { get; set; }

    //Happiness Stuff
    public static float happinessStatMultiplier = 1f;
    public static int happinessStatIncrement = 0;
    public static bool areaCleared = false;
    public static int areasClearedThisRun = 0;
    public static string sporeDied = null;
    public static string sporePermaDied = null;

    //Audio Stuff
    public static AudioMixerSnapshot currentAudioMixerSnapshot;

    //Loop Difficulty Stuff
    public static int currentLoop = 1;

    //Coroutine
    public static Coroutine delayTimer = null;

    //Environment Stuff
    public static bool isDay = true;

    //Controller Stuff
    public static InputManager.ControllerNames latestController = InputManager.ControllerNames.XBox;

    //Debug Stuff
    public static bool canShowTooltips = true;
}
