using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static bool gameIsStarting = true;
    public static string currentFunText = "FUN TEXT NOT SET";
    public static bool isGamePaused { get; set; }
    public static bool isAbleToPause { get; set; }
    public static WeaponStats currentWeapon { get; set; }
}
