using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class CarcassEnvironmentManager : MonoBehaviour, ISerializationCallbackReceiver
{
    public static List<string> myList;
    [Header("Environment Selection")]
    [ListToPopup(typeof(CarcassEnvironmentManager), "myList")]
    public string Selection;

    [Header("Environment Managment")]
    public List<Environment> Environments = new List<Environment>();

    public void OnAfterDeserialize() {}

    public void OnBeforeSerialize()
    {
        myList = new List<string>();
        foreach (Environment env in Environments)
        {
            myList.Add(env.Name);
        }
        UpdateEnvironments();
    }

    [ContextMenu("Update Environments")]
    public void UpdateEnvironments()
    {
        foreach(Environment env in Environments)
        {
            if(string.Compare(env.Name, Selection) != 0)
                env.UnloadEnvironment();
        }

        foreach(Environment env in Environments)
        {
            if(string.Compare(env.Name, Selection) == 0)
            {
                env.LoadEnvironment();
                break;
            }
        }
    }
}



[System.Serializable]
public class Environment
{
    public string Name;
    [Header("Skybox")]
    [SerializeField] private GameObject Skybox;
    [SerializeField] private Light DirectionalLight;

    [Header("Fog")]
    [SerializeField] private Color Fog_Color;
    [SerializeField] [Min(0f)] [Tooltip("Default value is 0")] float Fog_Start = 0;
    [SerializeField] [Min(0f)] [Tooltip("Default value is 10")] float Fog_Maximum = 10;
    [SerializeField] [Min(0f)] [Tooltip("Default value is 0")] float Fog_Alpha = 0;

    [Header("Decorations")]
    [SerializeField] private List<GameObject> Decorations = new List<GameObject>();

    public void LoadEnvironment()
    {
        //Enable Gameobjects
        if(Skybox != null) Skybox.SetActive(true);
        if(DirectionalLight != null) DirectionalLight.GameObject().SetActive(true);
        foreach(GameObject Decoration in Decorations)
        {
            Decoration.SetActive(true);
        }
        
        //Update Fog
        Shader.SetGlobalFloat("_WhereFogStarts", Fog_Start);
        Shader.SetGlobalFloat("_WhereFogReachesMax", Fog_Maximum);
        Shader.SetGlobalFloat("_FogAlpha", Fog_Alpha);
        Shader.SetGlobalColor("_FogColor", Fog_Color);
    }

    public void UnloadEnvironment()
    {
        //Disable Gameobjects
        if(Skybox != null) Skybox.SetActive(false);
        if(DirectionalLight != null) DirectionalLight.GameObject().SetActive(false);
        foreach(GameObject Decoration in Decorations)
            Decoration.SetActive(false);
    }
    
}