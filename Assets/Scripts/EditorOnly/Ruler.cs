using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Ruler : MonoBehaviour
{
    [SerializeField] Transform objectA;
    [SerializeField] Transform objectB;

    public void Measure()
    {
        Debug.Log("Distance: " + Vector3.Distance(objectA.position, objectB.position));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Ruler))]
class RulerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Setup Stuff
        base.OnInspectorGUI();

        var ruler = (Ruler)target;
        if (ruler == null) return;

        //Actual Stuff
        if (GUILayout.Button("Measure"))
        {
            ruler.Measure();
        }


    }
}
#endif
