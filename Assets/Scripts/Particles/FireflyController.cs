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

    private void OnTriggerEnter(Collider other){
        if(!Moving)
            StartCoroutine(MoveToPoint(Positions[positionIndex++]));
    }

    IEnumerator MoveToPoint(Transform goal){
        Moving = true;
        Vector3 startPosition = transform.position;
        for (float t = 0f; t <= 1; t += Time.deltaTime / 2) {
			transform.position = Vector3.Lerp (startPosition, goal.position, t);
			yield return null;
        }
        Moving = false;
        if(positionIndex+1 > Positions.Count)
            Destroy(this.gameObject);
    }

}
