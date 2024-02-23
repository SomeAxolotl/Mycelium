using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FireflyController : MonoBehaviour
{
    [SerializeField] private List<Transform> Positions;
    private List<Transform> staticPositions;
    private int positionIndex=0;
    private bool Moving = false;
    private bool QueueToMove = false;

    private void Update(){
        if(Moving==false && QueueToMove==true){
            StartCoroutine(MoveToPoint(Positions[positionIndex++]));
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag=="currentPlayer")
            QueueToMove=true;
    }
    private void OnTriggerExit(Collider other){
        if(other.tag=="currentPlayer")
            QueueToMove=false;
    }

    IEnumerator MoveToPoint(Transform goal){
        Moving = true;
        Vector3 startPosition = transform.position;
        for (float t = 0f; t <= 1; t += Time.deltaTime / 3) {
			transform.position = Vector3.Slerp(startPosition, goal.position, Mathf.SmoothStep(0, 1, t));
			yield return null;
        }
        Moving = false;
        if(positionIndex+1 > Positions.Count)
            Destroy(this.gameObject);
    }

}
