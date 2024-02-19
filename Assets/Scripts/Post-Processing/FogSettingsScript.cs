using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FogSettingsScript : MonoBehaviour
{
    [SerializeField] [Min(0f)] [Tooltip("Default value is 0")] float whereFogStarts;
    [SerializeField] [Min(0f)] [Tooltip("Default value is 10")] float whereFogReachesMax;
    [SerializeField] [Range(0f,1f)] [Tooltip("Default value is 0.75")] float fogAlpha;

    [SerializeField] [Tooltip("Default color is White")] Color fogColor;

    [ContextMenu("UpdateFog")]

    private void Start()
    {
        UpdateFog();
    }

    void UpdateFog()
    {
        Shader.SetGlobalFloat("_WhereFogStarts", whereFogStarts);
        Shader.SetGlobalFloat("_WhereFogReachesMax", whereFogReachesMax);
        Shader.SetGlobalFloat("_FogAlpha", fogAlpha);
        Shader.SetGlobalColor("_FogColor", fogColor);
    }
    private void OnValidate()
    {
        UpdateFog();
    }
}
