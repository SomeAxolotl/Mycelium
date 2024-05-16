using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class FABRIK : MonoBehaviour
{
    public int chainLength;

    public Transform target;
    public Transform pole;

    [Header("Solver Options")]
    public int totalIterations;

    public float delta;

    [Range(0, 1)] public float snapBackStrength;

    protected Transform[] bones;
    protected Vector3[] positions;
    protected float[] boneLength;
    protected float completeLength;

    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        ResolveIK();
    }


    void ResolveIK()
    {
        if (target == null)
        {
            return;
        }

        if (boneLength.Length != chainLength)
        {
            Initialize();
        }

        //Get positions
        for (int i = 0; i < bones.Length; i++)
        {
            positions[i] = bones[i].position;
        }

        //Do calculations
        //Stretch towards target if target is too far away
        if ((target.position - bones[0].position).sqrMagnitude >= MathF.Pow(completeLength, 2))
        {
            var direction = (target.position - positions[0]).normalized;

            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = positions[i - 1] + direction * boneLength[i - 1];
            }
        }
        else
        {
            for (int currentIteration = 0; currentIteration < totalIterations; currentIteration++)
            {
                //Backwards algorithm
                for (int i = positions.Length - 1; i > 0; i--)
                {
                    if (i == positions.Length - 1)
                    {
                        positions[i] = target.position;
                    }
                    else
                    {
                        positions[i] = positions[i + 1] + (positions[i] - positions[i + 1]).normalized * boneLength[i];
                    }
                }

                //Forwards algorithm
                for (int i = 1; i < positions.Length; i++)
                {
                    positions[i] = positions[i - 1] + (positions[i] - positions[i - 1]).normalized * boneLength[i - 1];
                }

                //Check if close enough
                if ((positions[positions.Length - 1] - target.position).sqrMagnitude < MathF.Pow(delta, 2))
                {
                    break;
                }
            }
        }

        //Bend towards pole
        if (pole != null)
        {
            for (int i = 1; i < positions.Length - 1; i++)
            {
                var plane = new Plane(positions[i + 1] - positions[i - 1], positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(pole.position);
                var projectedBone = plane.ClosestPointOnPlane(positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - positions[i - 1], projectedPole - positions[i - 1], plane.normal);

                positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (positions[i] - positions[i - 1]) + positions[i - 1];
            }
        }

        //Set Positions
        for (int i = 0; i < positions.Length; i++)
        {
            bones[i].position = positions[i];
        }
    }

    void Initialize()
    {
        //Initialize arrays
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        boneLength = new float[chainLength];

        completeLength = 0;

        //Initialize data
        var current = transform;

        for (int i = bones.Length - 1; i >= 0; i--)
        {
            bones[i] = current;

            if (i == bones.Length - 1)
            {

            }
            else
            {
                boneLength[i] = (bones[i + 1].position - current.position).magnitude;
                completeLength += boneLength[i];
            }

            current = current.parent;
        }
    }

    private void OnDrawGizmos()
    {
        var current = transform;

        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;

            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);

            current = current.parent;
        }
    }
}
