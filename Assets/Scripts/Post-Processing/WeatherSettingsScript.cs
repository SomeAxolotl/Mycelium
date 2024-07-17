using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeatherSettingsScript : MonoBehaviour
{
    public static WeatherSettingsScript Instance;

    [Header("Fog Settings")]
    [SerializeField] [Min(0f)] [Tooltip("Default value is 0")] public float whereFogStarts;
    [SerializeField] [Min(0f)] [Tooltip("Default value is 10")] public float whereFogReachesMax;
    [SerializeField] [Range(0f,1f)] [Tooltip("Default value is 0.75")] public float fogAlpha;

    [SerializeField] [Tooltip("Default color is White")] public Color fogColor;

    [Header("Wind Settings")]

    [SerializeField] [Min(0f)] public float windIntensity = 1f;
    [SerializeField] public float windDirection;
    private WindZone zone;

    [Header("Misc.")]
    private bool isRaining = false;
    private ParticleSystem rainParticleObject;

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

    private void Start()
    {
        zone = GetComponent<WindZone>();

        UpdateFog(whereFogStarts, whereFogReachesMax, fogAlpha, fogColor);
        UpdateWind(windIntensity, windDirection);
    }

    private void OnValidate()
    {
        zone = GetComponent<WindZone>();

        UpdateFog(whereFogStarts, whereFogReachesMax, fogAlpha, fogColor);
        UpdateWind(windIntensity, windDirection);
    }

    public void UpdateFog(float startDist, float maxDist, float alpha, Color color)
    {
        Shader.SetGlobalFloat("_WhereFogStarts", startDist);
        Shader.SetGlobalFloat("_WhereFogReachesMax", maxDist);
        Shader.SetGlobalFloat("_FogAlpha", alpha);
        Shader.SetGlobalColor("_FogColor", color);
    }

    public void UpdateWind(float intensity, float direction)
    {
        Shader.SetGlobalFloat("_GlobalWindMultiplier", intensity);
        Shader.SetGlobalFloat("_GlobalWindAngle", direction);

        Vector3 newRotation = new Vector3(transform.rotation.eulerAngles.x, direction, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(newRotation);

        zone.windMain = intensity * 4f;
    }

    public void UpdateRain(bool newRainState)
    {
        if (newRainState == isRaining) return;

        if (newRainState == true)
        {
            isRaining = true;

            Vector3 spawnPosition = GameObject.FindWithTag("currentPlayer").transform.position;
            spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y + 15, spawnPosition.z);

            rainParticleObject = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("RainEffect", spawnPosition, Quaternion.Euler(0, 0, 0));

            StartCoroutine(RainWindInverter());
        }
        else if (newRainState == false)
        {
            isRaining = false;

            if (rainParticleObject == null) return;

            Destroy(rainParticleObject.gameObject);
        }
    }

    private IEnumerator RainWindInverter()
    {
        while(isRaining)
        {
            yield return new WaitForSeconds(Random.Range(0.8f, 2.6f));
            zone.windMain = zone.windMain * -1;
        }
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
        if (GUILayout.Button("Turn Rain On"))
        {
            if (Application.isPlaying == false) return;
            weatherSettingsScript.UpdateRain(true);
        }

        if (GUILayout.Button("Turn Rain Off"))
        {
            if (Application.isPlaying == false) return;
            weatherSettingsScript.UpdateRain(false);
        }
    }
}
#endif
