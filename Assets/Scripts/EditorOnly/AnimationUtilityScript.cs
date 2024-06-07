#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationUtilityScript : MonoBehaviour
{
    [SerializeField] AnimationClip animClip;
    [SerializeField] string path;
    [SerializeField] string property;
    [Space(10)]
    [SerializeField] string keysVariableName;
    [SerializeField] string curveVariableName;

    EditorCurveBinding[] floatBinds;

    private void PrintCurveBinding(EditorCurveBinding bind)
    {
        Debug.Log($"Float Curve - Path: <color=#cb7644>{bind.path}</color>, Type: <color=#4ec9ac>{bind.type}</color>, Property: <color=#9bdbfc>{bind.propertyName}</color>");
    }

    public void PrintAllData()
    {
        floatBinds = AnimationUtility.GetCurveBindings(animClip);

        foreach (EditorCurveBinding bind in floatBinds)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(animClip, bind);

            PrintCurveBinding(bind);

            foreach (Keyframe kf in curve.keys)
            {
                Debug.Log($"Time: {kf.time}, Value: {kf.value}");
            }
        }
    }

    public void PrintSpecificData()
    {
        floatBinds = AnimationUtility.GetCurveBindings(animClip);

        foreach (EditorCurveBinding bind in floatBinds)
        {
            if (bind.path == path && bind.propertyName.ToString() == property)
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(animClip, bind);

                PrintCurveBinding(bind);

                foreach (Keyframe kf in curve.keys)
                {
                    Debug.Log($"Time: {kf.time}, Value: {kf.value}");
                }
            }
        }
    }

    public void CopyKeyframesToClipboard()
    {
        string code = "";
        int i = 0;

        floatBinds = AnimationUtility.GetCurveBindings(animClip);
        foreach (EditorCurveBinding bind in floatBinds)
        {
            if(bind.path == path && bind.propertyName.ToString() == property)
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(animClip, bind);

                code = "[HideInInspector] public Keyframe[] " + keysVariableName + " = new Keyframe[" + curve.length + "];\n\n";

                foreach (Keyframe kf in curve.keys)
                {
                    code = code + keysVariableName + "[" + i + "] = new Keyframe(" + kf.time + "f, " + kf.value + "f);\n";

                    i++;
                }

                code = code + curveVariableName + " = new AnimationCurve(" + keysVariableName + ");\n";
            }
        }

        EditorGUIUtility.systemCopyBuffer = code;

        Debug.Log("Copied code to your clipboard");
    }
}

[CustomEditor(typeof(AnimationUtilityScript))]
class AnimationUtilityScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Setup Stuff
        base.OnInspectorGUI();

        var animationUtilityScript = (AnimationUtilityScript)target;
        if (animationUtilityScript == null) return;

        //Actual Stuff
        if (GUILayout.Button("Print All Data"))
        {
            //if (Application.isPlaying == false) return;
            animationUtilityScript.PrintAllData();
        }

        if (GUILayout.Button("Print Specific Data"))
        {
            //if (Application.isPlaying == false) return;
            animationUtilityScript.PrintSpecificData();
        }

        if (GUILayout.Button("Copy Keyframes To Clipboard"))
        {
            //if (Application.isPlaying == false) return;
            animationUtilityScript.CopyKeyframesToClipboard();
        }
    }
}
#endif