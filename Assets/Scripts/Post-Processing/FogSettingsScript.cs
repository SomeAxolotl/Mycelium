using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FogSettingsScript : MonoBehaviour
{
    [SerializeField] [Min(0)] [Tooltip("Default value is 0")] float whereFogStarts;
    [SerializeField] [Min(0)] [Tooltip("Default value is 10")] float whereFogReachesMax;
    [SerializeField] [Range(0,1)] [Tooltip("Default value is 0.75")] float fogAlpha;

    [SerializeField] [Tooltip("Default color is White")] Color fogColor;

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_WhereFogStarts", whereFogStarts);
        Shader.SetGlobalFloat("_WhereFogReachesMax", whereFogReachesMax);
        Shader.SetGlobalFloat("_FogAlpha", fogAlpha);
        Shader.SetGlobalColor("_FogColor", fogColor);
    }
}
