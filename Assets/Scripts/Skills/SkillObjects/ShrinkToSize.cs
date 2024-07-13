using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkToSize : MonoBehaviour
{
    public float shrinkDelay = 1;
    public float duration = 0.5f;

    private Vector3 initialScale;
    private float currTime;

    private void OnEnable(){
        StartCoroutine(ScaleOverTime(duration));
    }

    private IEnumerator ScaleOverTime(float time){
        yield return new WaitForSeconds(shrinkDelay);
        initialScale = transform.localScale;

        currTime = 0f;
        while(currTime < time){
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, currTime / time);
            currTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }
}
