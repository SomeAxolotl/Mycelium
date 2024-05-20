using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowToSize : MonoBehaviour
{
    public Vector3 targetScale = Vector3.one;
    public float duration = 0.5f;

    private Vector3 initialScale;
    private float currTime;

    void Start(){
        initialScale = Vector3.zero;
        transform.localScale = initialScale;
        StartCoroutine(ScaleOverTime(targetScale, duration));
    }

    private IEnumerator ScaleOverTime(Vector3 target, float time){
        currTime = 0f;
        while(currTime < time){
            transform.localScale = Vector3.Lerp(initialScale, target, currTime / time);
            currTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = target;
    }
}
