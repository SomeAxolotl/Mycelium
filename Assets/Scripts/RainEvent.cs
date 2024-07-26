using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEvent : MonoBehaviour
{
    private void OnEnable()
    {
        StartRain();
    }

    private void StartRain()
    {
        WeatherSettingsScript.Instance.UpdateRain(true);
    }

    private void OnDisable()
    {
        StopRain();
    }

    private void StopRain()
    {
        WeatherSettingsScript.Instance.UpdateRain(false);
    }
}
