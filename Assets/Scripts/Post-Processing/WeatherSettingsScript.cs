using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeatherSettingsScript : MonoBehaviour
{
    [Header("Fog Settings")]
    [SerializeField] [Min(0f)] [Tooltip("Default value is 0")] public float whereFogStarts;
    [SerializeField] [Min(0f)] [Tooltip("Default value is 10")] public float whereFogReachesMax;
    [SerializeField] [Range(0f,1f)] [Tooltip("Default value is 0.75")] public float fogAlpha;

    [SerializeField] [Tooltip("Default color is White")] Color fogColor;

    [Header("Wind Settings")]

    [SerializeField] [Min(0f)] public float windIntensity = 1f;
    [SerializeField] public float windDirection;

    [Header("Misc.")]
    [SerializeField] public bool activateRain;

    private void Start()
    {
        UpdateFog();
        UpdateWind();

        if(activateRain == true)
        {
            Vector3 spawnPosition = GameObject.FindWithTag("currentPlayer").transform.position;
            spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y + 15, spawnPosition.z);

            ParticleManager.Instance.SpawnParticles("RainEffect", spawnPosition, Quaternion.Euler(0, 0, 0));
        }
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
