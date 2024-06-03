using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeatherSettingsScript : MonoBehaviour
{
    [Header("Fog Settings")]
    [SerializeField] [Min(0f)] [Tooltip("Default value is 0")] float whereFogStarts;
    [SerializeField] [Min(0f)] [Tooltip("Default value is 10")] float whereFogReachesMax;
    [SerializeField] [Range(0f,1f)] [Tooltip("Default value is 0.75")] float fogAlpha;

    [SerializeField] [Tooltip("Default color is White")] Color fogColor;

    [Header("Wind Settings")]

    [SerializeField] [Min(0f)] private float windIntensity = 1f;
    [SerializeField] private float windDirection;

    private void Start()
    {
        UpdateFog();
        UpdateWind();
    }

    private void OnValidate()
    {
        UpdateFog();
        UpdateWind();
    }

    public void UpdateFog()
    {
        Shader.SetGlobalFloat("_WhereFogStarts", whereFogStarts);
        Shader.SetGlobalFloat("_WhereFogReachesMax", whereFogReachesMax);
        Shader.SetGlobalFloat("_FogAlpha", fogAlpha);
        Shader.SetGlobalColor("_FogColor", fogColor);
    }

    public void UpdateWind()
    {
        Shader.SetGlobalFloat("_GlobalWindMultiplier", windIntensity);
        Shader.SetGlobalFloat("_GlobalWindAngle", windDirection);

        Vector3 newRotation = new Vector3(transform.rotation.eulerAngles.x, windDirection, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(newRotation);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WeatherSettingsScript))]
class WeatherSettingsScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Setup Stuff
        base.OnInspectorGUI();

        var weatherSettingsScript = (WeatherSettingsScript)target;
        if (weatherSettingsScript == null) return;

        //Actual Stuff
        if (GUILayout.Button("Update Fog"))
        {
            weatherSettingsScript.UpdateFog();
        }

        if (GUILayout.Button("Update Wind"))
        {
            weatherSettingsScript.UpdateWind();
        }
    }
}
#endif
