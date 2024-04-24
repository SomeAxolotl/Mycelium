using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TriggerSettings : MonoBehaviour
{
    [Header("==Options==")]
    [SerializeField] public bool shouldDisableObjects;
    [SerializeField] public bool shouldDestroyObjects;
    [SerializeField] public bool shouldEnableObjects;
    [HideInInspector] public bool shouldInstantiateObjects; //This one isn't ready for use yet -ryan
    [SerializeField] private bool shouldDestroyItself;

    //Lists
    [HideInInspector] public List<GameObject> objectsToDisable;
    [HideInInspector] public List<GameObject> objectsToDestroy;
    [HideInInspector] public List<GameObject> objectsToEnable;
    [HideInInspector] public List<GameObject> objectsToInstantiate;

    //Bool + String (aka the "ListInfo")
    [HideInInspector] public ListInfo showDisableList = new ListInfo(false, "Disable");
    [HideInInspector] public ListInfo showDestroyList = new ListInfo(false, "Destroy");
    [HideInInspector] public ListInfo showEnableList = new ListInfo(false, "Enable");
    [HideInInspector] public ListInfo showInstantiateList = new ListInfo(false, "Instantiate");

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "currentPlayer")
        {
            //Disabling
            if(shouldDisableObjects == true)
            {
                foreach(GameObject singleObject in objectsToDisable)
                {
                    singleObject.SetActive(false);
                }
            }

            //Destroying
            if (shouldDestroyObjects == true)
            {
                foreach (GameObject singleObject in objectsToDestroy)
                {
                    Destroy(singleObject);
                }
            }

            //Enabling
            if (shouldEnableObjects == true)
            {
                foreach (GameObject singleObject in objectsToEnable)
                {
                    singleObject.SetActive(true);
                }
            }

            //Itself
            if (shouldDestroyItself == true)
            {
                Destroy(gameObject);
            }
        }
    }
}

//A class I needed for the Custom Inspector
[Serializable]
public class ListInfo
{
    public bool isShowing;
    public string type;

    public ListInfo(bool show, string type)
    {
        this.isShowing = show;
        this.type = type;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TriggerSettings))]
class TriggerSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Setup Stuff
        base.OnInspectorGUI();

        var triggerSettings = (TriggerSettings)target;
        if (triggerSettings == null) return;

        //Actual Stuff
        EditorGUI.BeginChangeCheck();
        if(Application.isPlaying == false)
        {
            Undo.RecordObject(target, "Change TriggerSetting");
        }

        if (triggerSettings.shouldDisableObjects)
        {
            EditorGUILayout.Space();

            DrawList(triggerSettings.objectsToDisable, triggerSettings.showDisableList);
        }

        if (triggerSettings.shouldDestroyObjects)
        {
            EditorGUILayout.Space();

            DrawList(triggerSettings.objectsToDestroy, triggerSettings.showDestroyList);
        }

        if (triggerSettings.shouldEnableObjects)
        {
            EditorGUILayout.Space();

            DrawList(triggerSettings.objectsToEnable, triggerSettings.showEnableList);
        }

        if (triggerSettings.shouldInstantiateObjects)
        {
            EditorGUILayout.Space();

            DrawList(triggerSettings.objectsToInstantiate, triggerSettings.showInstantiateList);
        }

        if (EditorGUI.EndChangeCheck())
        {
            // If any changes were made, apply them
            EditorUtility.SetDirty(target);
        }
    }

    static void DrawList (List<GameObject> list, ListInfo listInfo)
    {
        GUIStyle customFoldoutStyle = new GUIStyle(EditorStyles.foldout);
        customFoldoutStyle.fontStyle = FontStyle.Bold;

        GUILayout.BeginHorizontal();

        listInfo.isShowing = EditorGUILayout.Foldout(listInfo.isShowing, "==Objects to " + listInfo.type + "==", true, customFoldoutStyle);

        EditorGUIUtility.labelWidth = 28;
        int size = Mathf.Max(0, EditorGUILayout.IntField("Size", list.Count, GUILayout.MaxWidth(70)));
        EditorGUIUtility.labelWidth = 0;

        GUILayout.EndHorizontal();

        while (size > list.Count)
        {
            list.Add(null);
        }

        while (size < list.Count)
        {
            list.RemoveAt(list.Count - 1);
        }

        if (listInfo.isShowing)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(5);

            for (int i = 0; i < list.Count; i++)
            {
                list[i] = EditorGUILayout.ObjectField("Element " + i, list[i], typeof(GameObject), true) as GameObject;
            }

            EditorGUI.indentLevel--;
        }
    }
}
#endif
