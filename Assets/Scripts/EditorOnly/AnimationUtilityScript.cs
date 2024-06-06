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
    [SerializeField] string variableName;

    EditorCurveBinding[] floatBinds;

    // Start is called before the first frame update
    void Start()
    {
        Keyframe[] keys = new Keyframe[9];

        keys[0] = new Keyframe(0f, 0.518f);
        keys[1] = new Keyframe(0.2833333f, 1.6539f);
        keys[2] = new Keyframe(0.55f, 7.4148f);
        keys[3] = new Keyframe(1.05f, 10.7189f);
        keys[4] = new Keyframe(1.516667f, 13.11f);
        keys[5] = new Keyframe(1.783333f, 8.72275f);
        keys[6] = new Keyframe(1.95f, 0.2886f);
        keys[7] = new Keyframe(2.75f, 0.2886f);
        keys[8] = new Keyframe(3.1f, 7.77f);
    }

    public void PrintAllData()
    {
        floatBinds = AnimationUtility.GetCurveBindings(animClip);

        foreach (EditorCurveBinding bind in floatBinds)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(animClip, bind);
            Debug.Log($"Float Curve - Path: {bind.path}, Property: {bind.propertyName}, Type: {bind.type}");

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

                Debug.Log($"Float Curve - Path: {bind.path}, Property: {bind.propertyName}, Type: {bind.type}");

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

                code = "Keyframe[] " + variableName + " = new Keyframe[" + curve.length +"];\n\n";

                foreach (Keyframe kf in curve.keys)
                {
                    code = code + variableName + "[" + i + "] = new Keyframe(" + kf.time + "f, " + kf.value + "f);\n";

                    i++;
                }
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