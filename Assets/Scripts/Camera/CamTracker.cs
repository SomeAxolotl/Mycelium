using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTracker : MonoBehaviour
{
    public Transform centerPoint;

    private void Update()
    {
        //Null check for Current Player and Center Point
        if (GameObject.FindWithTag("currentPlayer").transform.Find("CenterPoint") != null)
        {
            //Camera Tracker position is set to Center Point position
            centerPoint = GameObject.FindWithTag("currentPlayer").transform.Find("CenterPoint");
            transform.position = centerPoint.position;
        }
    }
}
