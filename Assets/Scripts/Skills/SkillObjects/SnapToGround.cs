using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGround : MonoBehaviour
{
    void Awake(){
        LayerMask groundLayerMask = LayerMask.GetMask("Environment", "Furniture", "Wall");
        Vector3 spawnPosition = transform.position;
        RaycastHit hit;
        Debug.DrawRay(spawnPosition, Vector3.down * 100f, Color.red, 4f);
        if(Physics.Raycast(spawnPosition, Vector3.down, out hit, Mathf.Infinity, groundLayerMask)){
            spawnPosition.y = hit.point.y;
        }
        transform.position = spawnPosition;
    }
}
