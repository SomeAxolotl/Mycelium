using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FireflyController : MonoBehaviour
{
    public List<Transform> targetPositions = new List<Transform>();
    private GameObject fireflySearchRoot;
    private int positionIndex=0;
    private bool isMoving = false;

    void Start()
    {
        fireflySearchRoot = GameObject.FindWithTag("FireflySearchRoot");
        foreach (Transform targetPosition in fireflySearchRoot.GetComponentsInChildren<Transform>())
        {
            if (targetPosition.gameObject.tag == "FireflyTarget")
            {
                targetPositions.Add(targetPosition);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag=="currentPlayer" && isMoving == false)
        {
            StartCoroutine(MoveToPoint(targetPositions[positionIndex++]));
        }
    }

    IEnumerator MoveToPoint(Transform goal)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        for (float t = 0f; t <= 1; t += Time.deltaTime / 3) {
			transform.position = Vector3.Slerp(startPosition, goal.position, Mathf.SmoothStep(0, 1, t));
			yield return null;
        }
        isMoving = false;

        if(positionIndex+1 > targetPositions.Count)
        {
            Destroy(this.gameObject);
        }
    }

}
