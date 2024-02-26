using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FloatingBugMovement : MonoBehaviour
{
    private Vector3 origin = new Vector3();
    [SerializeField] private Vector3 Range;
    private Vector3 targetPosition;
    [SerializeField][Min(1)] private float RotationSpeed = 1;
    [SerializeField] private float Speed = 1;
    private Quaternion targetRotation;
    void Start()
    {
        origin = transform.position;
        targetPosition = new Vector3();
        targetRotation = new Quaternion(0,0,0,0);
        NewTargetPosition();
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * Speed;
        if(Vector3.Distance(transform.position, targetPosition)<=2)
            NewTargetPosition();
    }

    void NewTargetPosition()
    {
        targetPosition = new Vector3(UnityEngine.Random.Range(origin.x-Range.x, origin.x+Range.x), UnityEngine.Random.Range(origin.y-Range.y, origin.y+Range.y), UnityEngine.Random.Range(origin.z-Range.z, origin.z+Range.z));
        targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
    }

}
