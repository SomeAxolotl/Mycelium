using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InvisibleMaterial : MonoBehaviour
{    private MeshRenderer thisMeshRenderer;

    void Start()
    {
        thisMeshRenderer = GetComponent<MeshRenderer>();

        if (Application.isPlaying)
        {
            thisMeshRenderer.enabled = false;
        }
    }

    public void ToggleAllVisibility()
    {
        if(Application.isPlaying == false)
        {
            return;
        }

        MeshRenderer mr;

        foreach(GameObject singleObject in GameObject.FindGameObjectsWithTag("InvisibleWalls"))
        {
            mr = singleObject.GetComponent<MeshRenderer>();
            mr.enabled = !mr.enabled;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InvisibleMaterial))]
class InvisibleMaterialEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Setup Stuff
        base.OnInspectorGUI();

        var invisibleMaterial = (InvisibleMaterial)target;
        if (invisibleMaterial == null) return;

        //Actual Stuff
        if (GUILayout.Button("Toggle Visibility For All Walls"))
        {
            invisibleMaterial.ToggleAllVisibility();
        }

        
    }
}
#endif
