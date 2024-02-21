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

    private void OnTriggerEnter(Collider other){
        StartCoroutine(MoveToPoint(Positions[positionIndex++]));
    }

    IEnumerator MoveToPoint(Transform goal){
        Vector3 startPosition = transform.position;
        for (float t = 0f; t <= 1; t += Time.deltaTime / 2) {
			transform.position = Vector3.Lerp (startPosition, goal.position, t);
			yield return null;
        }
        if(positionIndex+1 > Positions.Count)
            Destroy(this.gameObject);
    }
}
